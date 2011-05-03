using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using Palaso.IO;
using Palaso.Progress;
using Palaso.UI.WindowsForms.Progress;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using Timer = System.Threading.Timer;

namespace SayMore.UI.Utilities
{
	public class ArchiveHelper
	{
		private readonly string _eventTitle;
		private Event _event;
		private PersonInformant _personInformant;
		private string _pathOfFolderToArchive;
		private string _eventArchiveFilePath;
		private string _metsFilePath;
		private BackgroundWorker _worker;
		private ZipEntry _zipProgressPrevEntry;
		private Timer _timer;
		private bool _cancelProcess;

		public string RampPackagePath { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ArchiveHelper(Event evnt, PersonInformant personInformant)
		{
			_event = evnt;
			_personInformant = personInformant;
			_eventTitle = _event.MetaDataFile.GetStringValue("title", null);
		}

		#region RAMP calling methods
		/// ------------------------------------------------------------------------------------
		public bool CallRAMP()
		{
			try
			{
				var prs = new Process();
				prs.StartInfo.FileName = RampPackagePath;
				prs.Start();
				prs.WaitForInputIdle(8000);

				// Every 4 seconds we'll check to see if the RAMP package is locked. When
				// it gets unlocked by RAMP, then we'll delete it.
				_timer = new Timer(CheckIfPackageFileIsLocked, RampPackagePath, 2000, 4000);
				return true;
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to open the archive package in RAMP.");
				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CheckIfPackageFileIsLocked(Object packageFile)
		{
			if (ComponentFile.IsFileLocked(packageFile as string))
				return;

			try { File.Delete(RampPackagePath); }
			catch { }
			_timer.Dispose();
			_timer = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public bool CreateRampPackage()
		{
			Application.UseWaitCursor = true;

			var retVal = CreateCopyOfEvent();

			if (retVal)
				retVal = CreateCopyOfParticipants();

			if (retVal)
				retVal = CreateEventArchive();

			if (retVal)
				retVal = CreateMetsFile();

			if (retVal)
				retVal = CreateRampPackageWithEventArchiveAndMetsFile();

			CleanUp();
			Application.UseWaitCursor = false;
			return retVal;
		}

		/// ------------------------------------------------------------------------------------
		public void CleanUp()
		{
			try { Directory.Delete(_pathOfFolderToArchive, true); }
			catch { }

			try { File.Delete(_metsFilePath); }
			catch { }

			try { File.Delete(_eventArchiveFilePath); }
			catch { }

			_event = null;
			_personInformant = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a copy of the event folder in the OS' temp. folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CreateCopyOfEvent()
		{
			var errorMsg = "There was an error attempting to copy the files for the event '{0}'";

			if (!Directory.Exists(_event.FolderPath))
			{
				ReportError(new DirectoryNotFoundException(), errorMsg);
				return false;
			}

			try
			{
				_pathOfFolderToArchive = FolderUtils.CopyFolderToTempFolder(_event.FolderPath);
			}
			catch (Exception e)
			{
				ReportError(e, errorMsg);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a copy of the event's participant folders in the temp folder for the event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CreateCopyOfParticipants()
		{
			try
			{
				foreach (var person in _event.GetAllParticipants().Select(n => _personInformant.GetPersonByName(n)))
					FolderUtils.CopyFolder(person.FolderPath, _pathOfFolderToArchive);
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to copy the participant informaton for the event '{0}'.");
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool CreateEventArchive()
		{
			_eventArchiveFilePath = Path.Combine(Path.GetTempPath(), _event.Id + ".zip");

			try
			{
				_cancelProcess = false;
				_worker = new BackgroundWorker();
				_worker.DoWork += delegate
				{
					CreateZipFile(_eventArchiveFilePath, z => z.AddDirectory(_pathOfFolderToArchive));
				};

				using (var dlg = new ProgressDialog())
				{
					dlg.Overview = string.Format("Creating archive for event '{0}'", _eventTitle ?? _event.Id);
					dlg.BackgroundWorker = _worker;
					dlg.CancelRequested += HandleCancelZipping;
					dlg.ShowDialog();
				}
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to create an archive for the event '{0}'.");
				return false;
			}

			return !_cancelProcess;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sample JSON data:
		///
		/// "id":"8zd4uscmys",
		/// "dc.title":"Wedding on the Lake",
		/// "dc.type.mode": ["Text","Photograph","Video","Music","Speech","Map"],
		/// "dc.contributor"
		/// "dc.description":
		/// "dc.description.abstract":
		/// "format.extent.recording":"1 hour",
		/// "files":
		/// "dc.date.created":"2010-2011 (April 19, 2009)",
		/// "dc.date.modified":"May 25, 2011",
		/// "relation.requires.has":"Y",
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CreateMetsFile()
		{
			try
			{
				var bldr = new StringBuilder();

				foreach (var value in GetMetsPairs())
					bldr.AppendFormat("{0},", value);

				var jsonData = string.Format("{{{0}}}", bldr.ToString().TrimEnd(','));
				jsonData = JSONUtils.EncodeData(jsonData);
				var metsData = Resources.EmptyMets.Replace("<binData>", "<binData>" + jsonData);
				_metsFilePath = Path.Combine(Path.GetTempPath(), "mets.xml");
				File.WriteAllText(_metsFilePath, metsData);
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to create a RAMP/REAP mets file for the event '{0}'.");
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetMetsPairs()
		{
			var createDate = _event.MetaDataFile.GetStringValue("date", null);
			var value = (createDate ?? Guid.NewGuid().ToString()).Replace('/', '_').Replace('\\', '_');
			var rampId = string.Format("{0}_{1}", _event.Id, value);
			yield return JSONUtils.MakeKeyValuePair("id", rampId);
			yield return JSONUtils.MakeKeyValuePair("dc.title", _eventTitle ?? _event.Id);

			value = _event.MetaDataFile.GetStringValue("situation", null);
			if (value != null)
				yield return JSONUtils.MakeKeyValuePair("dc.description", value);

			value = _event.MetaDataFile.GetStringValue("notes", null);
			if (value != null)
			{
				var abs =
					JSONUtils.MakeKeyValuePair(" ", value) + "," +
					JSONUtils.MakeKeyValuePair("lang", string.Empty);

				yield return JSONUtils.MakeArrayFromValues("dc.description.abstract", new[] { abs });
			}

			var contributions = _event.MetaDataFile.GetValue("contributions", null) as ContributionCollection;
			if (contributions != null && contributions.Count > 0)
				yield return JSONUtils.MakeArrayFromValues("dc.contributor", contributions.Select(c => c.GetREAPValue()));

			var file =
				JSONUtils.MakeKeyValuePair(" ", Path.GetFileName(_eventArchiveFilePath)) + "," +
				JSONUtils.MakeKeyValuePair("description", string.Format("'{0}' Event Archive", _eventTitle ?? _event.Id)) + "," +
				JSONUtils.MakeKeyValuePair("relationship", "source");

			yield return JSONUtils.MakeArrayFromValues("files", new[] { file });

			if (createDate != null)
				yield return JSONUtils.MakeKeyValuePair("dc.date.created", createDate);

			yield return JSONUtils.MakeKeyValuePair("relation.requires.has", "Y");

			yield return JSONUtils.MakeArrayFromValues("dc.relation.requires",
				new[] { JSONUtils.MakeKeyValuePair(" ", "SayMore") });
		}

		/// ------------------------------------------------------------------------------------
		public bool CreateRampPackageWithEventArchiveAndMetsFile()
		{
			try
			{
				_cancelProcess = false;
				RampPackagePath = Path.Combine(Path.GetTempPath(), _event.Id + ".ramp");
				_worker = new BackgroundWorker();
				_worker.DoWork += delegate
				{
					CreateZipFile(RampPackagePath, z =>
					{
						z.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
						z.AddFile(_eventArchiveFilePath, string.Empty);
						z.AddFile(_metsFilePath, string.Empty);
					});
				};

				using (var dlg = new ProgressDialog())
				{
					dlg.Overview = string.Format("Creating RAMP/REAP package for event '{0}'", _eventTitle ?? _event.Id);
					dlg.BackgroundWorker = _worker;
					dlg.CancelRequested += HandleCancelZipping;
					dlg.ShowDialog();
				}
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to create an archive for the event '{0}'.");
				return false;
			}

			return !_cancelProcess;
		}

		/// ------------------------------------------------------------------------------------
		private void CreateZipFile(string zipFilePath, Action<ZipFile> addStuffToZipAction)
		{
			using (var zip = new ZipFile())
			{
				_zipProgressPrevEntry = null;

				addStuffToZipAction(zip);
				zip.SaveProgress += HandleZipSaveProgress;
				zip.Save(zipFilePath);

				if (!_cancelProcess)
				{
					_worker.ReportProgress(100);
					Thread.Sleep(700);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZipSaveProgress(object s, SaveProgressEventArgs e)
		{
			if (_zipProgressPrevEntry == e.CurrentEntry || e.CurrentEntry == null ||
				e.CurrentEntry.IsDirectory || _worker.CancellationPending)
			{
				return;
			}

			_zipProgressPrevEntry = e.CurrentEntry;
			int pct = (int)(((float)e.EntriesSaved / e.EntriesTotal) * 100);

			_worker.ReportProgress(pct, new ProgressState
			{
				StatusLabel = string.Format("Adding: {0}", Path.GetFileName(e.CurrentEntry.FileName)),
				NumberOfStepsCompleted = pct,
				TotalNumberOfSteps = 100
			});
		}

		/// ------------------------------------------------------------------------------------
		void HandleCancelZipping(object sender, EventArgs e)
		{
			_cancelProcess = true;
			_worker.CancelAsync();
			while (_worker.IsBusy)
				Application.DoEvents();
		}

		/// ------------------------------------------------------------------------------------
		private void ReportError(Exception e, string msg)
		{
			Application.UseWaitCursor = false;
			Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, msg, _eventTitle ?? _event.Id);
		}
	}
}
