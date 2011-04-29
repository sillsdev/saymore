using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Ionic.Zip;
using Palaso.IO;
using Palaso.Progress;
using Palaso.UI.WindowsForms.Progress;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.Utilities
{
	public class ArchiveHelper : IDisposable
	{
		private readonly Event _event;
		private readonly PersonInformant _personInformant;
		private readonly string _eventTitle;
		private string _pathOfFolderToArchive;
		private string _eventArchiveFilePath;
		private string _metsFilePath;
		private BackgroundWorker _worker;
		private ZipEntry _zipProgressPrevEntry;

		public string RampPackagePath { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ArchiveHelper(Event evnt, PersonInformant personInformant)
		{
			_event = evnt;
			_personInformant = personInformant;
			_eventTitle = _event.MetaDataFile.GetStringValue("title", null);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			try { Directory.Delete(_pathOfFolderToArchive, true); }
			catch { }

			try { File.Delete(_metsFilePath); }
			catch { }

			try { File.Delete(_eventArchiveFilePath); }
			catch { }

			try { File.Delete(RampPackagePath); }
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		public bool CallRAMP()
		{
			var prs = new Process();
			prs.StartInfo.UseShellExecute = false;
			//prs.StartInfo.FileName = "\"" + RampPackagePath + "\"";
			prs.StartInfo.FileName = @"C:\Program Files\dev.RAMP\dev.RAMP.exe";
			prs.StartInfo.Arguments = RampPackagePath;

			try
			{
				prs.Start();

				// Wait until the process has started.
				while (!prs.HasExited && prs.Id == 0)
					Thread.Sleep(100);

			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to open the archive package in RAMP.");
				return false;
			}

			// Make sure the RAMP package is released before going any further.
			ComponentFile.WaitForFileRelease(RampPackagePath, false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool Archive()
		{
			try
			{
				_pathOfFolderToArchive = FolderUtils.CopyFolderToTempFolder(_event.FolderPath);
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to copy the files for the event '{0}'");
				return false;
			}

			if (_pathOfFolderToArchive == null)
				return false;

			if (!CreateCopyOfEventContainingParticipants())
				return false;

			if (!CreateEventArchive())
				return false;

			if (!CreateMetsFile())
				return false;

			return CreateRampPackageWithEventArchiveAndMetsFile();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a copy of the event's folder in the temp folder, and then copies into that
		/// all the folder associated with each participants.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CreateCopyOfEventContainingParticipants()
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
				using (var dlg = new ProgressDialog())
				{
					dlg.Overview = string.Format("Creating archive for event '{0}'", _eventTitle ?? _event.Id);
					dlg.BackgroundWorker = _worker = new BackgroundWorker();
					_worker.DoWork += ZipUpFolder;
					dlg.ShowDialog();
				}
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to create an archive for the event '{0}'.");
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void ZipUpFolder(object sender, DoWorkEventArgs e)
		{
			using (var zip = new ZipFile())
			{
				_zipProgressPrevEntry = null;
				zip.SaveProgress += HandleZipSaveProgress;
				zip.AddDirectory(_pathOfFolderToArchive);
				zip.Save(_eventArchiveFilePath);
				_worker.ReportProgress(100);
				Thread.Sleep(700);
			}
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

			if (_eventTitle != null)
				yield return JSONUtils.MakeKeyValuePair("dc.title", _eventTitle);

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
		private bool CreateRampPackageWithEventArchiveAndMetsFile()
		{
			try
			{
				RampPackagePath = Path.Combine(Path.GetTempPath(), _event.Id + ".ramp");

				using (var dlg = new ProgressDialog())
				{
					dlg.Overview = string.Format("Creating RAMP/REAP package for event '{0}'", _eventTitle ?? _event.Id);
					dlg.BackgroundWorker = _worker = new BackgroundWorker();
					_worker.DoWork += ZipUpRampFiles;
					dlg.ShowDialog();
				}
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to create an archive for the event '{0}'.");
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void ZipUpRampFiles(object sender, DoWorkEventArgs e)
		{
			using (var zip = new ZipFile())
			{
				_zipProgressPrevEntry = null;
				zip.SaveProgress += HandleZipSaveProgress;
				zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
				zip.AddFile(_eventArchiveFilePath, string.Empty);
				zip.AddFile(_metsFilePath, string.Empty);
				zip.Save(RampPackagePath);
				_worker.ReportProgress(100);
				Thread.Sleep(700);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZipSaveProgress(object s, SaveProgressEventArgs args)
		{
			if (_zipProgressPrevEntry == args.CurrentEntry || args.CurrentEntry == null || args.CurrentEntry.IsDirectory)
				return;

			_zipProgressPrevEntry = args.CurrentEntry;
			int pct = (int)(((float)args.EntriesSaved / args.EntriesTotal) * 100);

			_worker.ReportProgress(pct, new ProgressState
			{
				StatusLabel = string.Format("Adding: {0}", Path.GetFileName(args.CurrentEntry.FileName)),
				NumberOfStepsCompleted = pct,
				TotalNumberOfSteps = 100
			});
		}

		/// ------------------------------------------------------------------------------------
		private void ReportError(Exception e, string msg)
		{
			Palaso.Reporting.ErrorReport.ReportNonFatalExceptionWithMessage(e, msg, _eventTitle ?? _event.Id);
		}
	}
}
