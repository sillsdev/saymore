using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using Palaso.IO;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using Timer = System.Threading.Timer;
using ZipFile = Ionic.Zip.ZipFile;

namespace SayMore.UI.Archiving
{
	public class ArchivingDlgViewModel
	{
		private readonly string _eventTitle;
		private Event _event;
		private PersonInformant _personInformant;
		private string _eventArchiveFilePath;
		private string _metsFilePath;
		private BackgroundWorker _worker;
		private Timer _timer;
		private bool _cancelProcess;
		private bool _workerException;
		private readonly Dictionary<int, string> _progressMessages = new Dictionary<int,string>();
		private string _rampProgramPath;
		private Action _incrementProgressBarAction;

		public bool IsBusy { get; private set; }
		public string RampPackagePath { get; private set; }
		public LogBox LogBox { get; private set; }

		#region construction and initialization
		/// ------------------------------------------------------------------------------------
		public ArchivingDlgViewModel(Event evnt, PersonInformant personInformant)
		{
			_event = evnt;
			_personInformant = personInformant;
			_eventTitle = _event.MetaDataFile.GetStringValue("title", null);

			LogBox = new LogBox();
			LogBox.TabStop = false;
			LogBox.ShowMenu = false;

			if (Program.DialogFont.FontFamily.IsStyleAvailable(FontStyle.Bold))
				LogBox.Font = new Font(Program.DialogFont, FontStyle.Bold);
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(out int maxProgBarValue, Action incrementProgressBarAction)
		{
			maxProgBarValue = 0;
			IsBusy = true;
			_incrementProgressBarAction = incrementProgressBarAction;

			LogBox.WriteMessage("Searching for the RAMP program...");
			Application.DoEvents();
			_rampProgramPath = FileLocator.GetFromRegistryProgramThatOpensFileType(".ramp") ??
				FileLocator.LocateInProgramFiles("ramp.exe", true, "ramp");

			LogBox.Clear();

			if (_rampProgramPath == null)
				LogBox.WriteMessageWithColor("Red", "The RAMP pogram cannot be found!{0}", Environment.NewLine);

			LogBox.WriteMessageWithFontStyle(FontStyle.Bold, "The following event and contributor files will be added to your archive.");

			var allFilesInArchive = GetFilesToArchive();
			int fileCount = 0;

			foreach (var kvp in allFilesInArchive)
			{
				LogBox.WriteMessage(kvp.Key == string.Empty ?
					string.Format("{0}     Event: {1}", Environment.NewLine, _eventTitle ?? _event.Id) :
					string.Format("{0}     Contributor: {1}", Environment.NewLine, kvp.Key));

				foreach (var file in kvp.Value)
				{
					LogBox.WriteMessageWithFontStyle(FontStyle.Regular, "          \u00B7 {0}", Path.GetFileName(file));
					fileCount++;
				}
			}

			if (allFilesInArchive.Count == 1)
			{
				LogBox.WriteMessage(string.Empty);
				LogBox.WriteWarning("There are no contributors for this event.");
			}

			LogBox.ScrollToTop();
			IsBusy = false;

			if (_rampProgramPath == null)
				return false;

			maxProgBarValue = fileCount + 3;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Each entry in the dictionary is a list of files. The first entry contains a list
		/// of all the event files. All subsequent entries are keyed using a person id and
		/// the values are the lists of files for each person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<string>> GetFilesToArchive()
		{
			var list = new Dictionary<string, IEnumerable<string>>();

			list[string.Empty] = Directory.GetFiles(_event.FolderPath);

			foreach (var person in _event.GetAllParticipants().Select(n => _personInformant.GetPersonByName(n)))
				list[person.Id] = Directory.GetFiles(person.FolderPath);

			return list;
		}

		#endregion

		#region RAMP calling methods
		/// ------------------------------------------------------------------------------------
		public bool CallRAMP()
		{
			try
			{
				var prs = new Process();
				prs.StartInfo.FileName = _rampProgramPath;
				prs.StartInfo.Arguments = RampPackagePath;
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
			if (!ComponentFile.IsFileLocked(packageFile as string))
				CleanUpTempRampPackage();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public bool CreatePackage()
		{
			IsBusy = true;
			LogBox.Clear();

			var success = CreateEventArchive();

			if (success)
				success = CreateMetsFile();

			if (success)
				success = CreateRampPackageWithEventArchiveAndMetsFile();

			if (success && _incrementProgressBarAction != null)
				Thread.Sleep(700);

			CleanUp();

			if (success)
			{
				LogBox.WriteMessageWithColor(Color.DarkGreen,
					Environment.NewLine + "Ready to hand the package to RAMP");
			}

			IsBusy = false;
			return success;
		}

		/// ------------------------------------------------------------------------------------
		public bool CreateEventArchive()
		{
			_progressMessages.Clear();
			_progressMessages[0] = string.Format("Adding Event Files For '{0}'...", _eventTitle ?? _event.Id);
			_progressMessages[Directory.GetFiles(_event.FolderPath).Count() + 1] =
				string.Format("{0}Adding Contributor Files For '{1}'...", Environment.NewLine, _eventTitle ?? _event.Id);

			_eventArchiveFilePath = Path.Combine(Path.GetTempPath(), _event.Id + ".zip");

			return RunWorker(() => CreateZipFile(_eventArchiveFilePath, z =>
			{
				z.AddDirectory(_event.FolderPath);
				foreach (var person in _event.GetAllParticipants().Select(n => _personInformant.GetPersonByName(n)))
					z.AddDirectory(person.FolderPath, Path.Combine("Contributors", person.Id));
			}, "There was an error attempting to create an archive for the event '{0}'."));
		}

		#region Methods for creating mets file.
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

			if (_incrementProgressBarAction != null)
				_incrementProgressBarAction();

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

		#endregion

		/// ------------------------------------------------------------------------------------
		public bool CreateRampPackageWithEventArchiveAndMetsFile()
		{
			_progressMessages.Clear();
			_progressMessages[0] = Environment.NewLine + "Creating RAMP Package Containing 2 Files...";
			RampPackagePath = Path.Combine(Path.GetTempPath(), _event.Id + ".ramp");

			return RunWorker(() => CreateZipFile(RampPackagePath, z =>
			{
				z.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
				z.AddFile(_eventArchiveFilePath, string.Empty);
				z.AddFile(_metsFilePath, string.Empty);
			}, "There was an error attempting to create an archive for the event '{0}'."));
		}

		/// ------------------------------------------------------------------------------------
		public bool RunWorker(Action workAction)
		{
			try
			{
				using (_worker = new BackgroundWorker())
				{
					_cancelProcess = false;
					_workerException = false;
					_worker.ProgressChanged += HandleBackgroundWorkerProgressChanged;
					_worker.WorkerReportsProgress = true;
					_worker.WorkerSupportsCancellation = true;
					_worker.DoWork += delegate { workAction(); };
					_worker.RunWorkerAsync();

					while (_worker.IsBusy)
						Application.DoEvents();
				}
			}
			catch (Exception e)
			{
				ReportError(e, "There was a problem starting process to create zip file.");
				return false;
			}
			finally
			{
				_worker = null;
			}

			return !_cancelProcess && !_workerException;
		}

		/// ------------------------------------------------------------------------------------
		private void CreateZipFile(string zipFilePath, Action<ZipFile> addStuffToZipAction,
			string errorMsg)
		{
			try
			{
				using (var zip = new ZipFile())
				{
					addStuffToZipAction(zip);
					zip.SaveProgress += HandleZipSaveProgress;
					zip.Save(zipFilePath);
				}
			}
			catch (Exception e)
			{
				_worker.ReportProgress(0, new KeyValuePair<Exception, string>(e, errorMsg));
				_workerException = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called by the Save method on the ZipFile class as the zip file is being
		/// saved to the disk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleZipSaveProgress(object s, SaveProgressEventArgs e)
		{
			if (_cancelProcess || e.EventType != ZipProgressEventType.Saving_BeforeWriteEntry)
				return;

			string msg;
			if (_progressMessages.TryGetValue(e.EntriesSaved, out msg))
				LogBox.WriteMessage(msg);

			_worker.ReportProgress(e.EntriesSaved, Path.GetFileName(e.CurrentEntry.FileName));
		}

		/// ------------------------------------------------------------------------------------
		void HandleBackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState == null || _cancelProcess)
				return;

			if (e.UserState is KeyValuePair<Exception, string>)
			{
				var kvp = (KeyValuePair<Exception, string>)e.UserState;
				ReportError(kvp.Key, kvp.Value);
				return;
			}

			var msg = e.UserState as string;

			if (!string.IsNullOrEmpty(msg) && msg != "~zippingDone")
				LogBox.WriteMessageWithFontStyle(FontStyle.Regular, "\t" + e.UserState);

			if (!_cancelProcess && _incrementProgressBarAction != null)
				_incrementProgressBarAction();
		}

		/// ------------------------------------------------------------------------------------
		public void Cancel()
		{
			if (_cancelProcess)
				return;

			_cancelProcess = true;

			if (_worker != null)
			{
				LogBox.WriteMessageWithColor(Color.Red, Environment.NewLine + "Canceling...");
				_worker.CancelAsync();
				while (_worker.IsBusy)
					Application.DoEvents();
			}

			CleanUp();
			CleanUpTempRampPackage();
		}

		/// ------------------------------------------------------------------------------------
		private void ReportError(Exception e, string msg)
		{
			WaitCursor.Hide();
			LogBox.WriteError(msg, _eventTitle ?? _event.Id);
			LogBox.WriteException(e);
		}

		#region Clean-up methods
		/// ------------------------------------------------------------------------------------
		public void CleanUp()
		{
			try { File.Delete(_metsFilePath); }
			catch { }

			try { File.Delete(_eventArchiveFilePath); }
			catch { }

			_event = null;
			_personInformant = null;
		}

		/// ------------------------------------------------------------------------------------
		public void CleanUpTempRampPackage()
		{
			try { File.Delete(RampPackagePath); }
			catch { }

			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
		}

		#endregion
	}
}
