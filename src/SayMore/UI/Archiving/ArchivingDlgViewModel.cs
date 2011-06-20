using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using Palaso.ClearShare;
using Palaso.IO;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using Timer = System.Threading.Timer;
using ZipFile = Ionic.Zip.ZipFile;

namespace SayMore.UI.Archiving
{
	public class ArchivingDlgViewModel
	{
#if !__MonoCS__
		[DllImport("User32.dll")]
		private static extern IntPtr SetForegroundWindow(int hWnd);

		[DllImport("User32.dll")]
		private static extern bool BringWindowToTop(int hWnd);
#endif

		private readonly string _eventTitle;
		private Event _event;
		private PersonInformant _personInformant;
		private string _metsFilePath;
		private BackgroundWorker _worker;
		private Timer _timer;
		private bool _cancelProcess;
		private bool _workerException;
		private readonly Dictionary<string, string> _progressMessages = new Dictionary<string, string>();
		private string _rampProgramPath;
		private Action _incrementProgressBarAction;
		private IDictionary<string, IEnumerable<string>> _fileLists;

		public bool IsBusy { get; private set; }
		public string RampPackagePath { get; private set; }
		public LogBox LogBox { get; private set; }

		#region construction and initialization
		/// ------------------------------------------------------------------------------------
		public ArchivingDlgViewModel(Event evnt, PersonInformant personInformant)
		{
			_event = evnt;
			_personInformant = personInformant;
			_eventTitle = _event.MetaDataFile.GetStringValue("title", null) ?? _event.Id;

			LogBox = new LogBox();
			LogBox.TabStop = false;
			LogBox.ShowMenu = false;

			if (Program.DialogFont.FontFamily.IsStyleAvailable(FontStyle.Bold))
				LogBox.Font = new Font(Program.DialogFont, FontStyle.Bold);
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(out int maxProgBarValue, Action incrementProgressBarAction)
		{
			IsBusy = true;
			_incrementProgressBarAction = incrementProgressBarAction;

			LogBox.WriteMessage("Searching for the RAMP program...");
			Application.DoEvents();
			_rampProgramPath = FileLocator.GetFromRegistryProgramThatOpensFileType(".ramp") ??
				FileLocator.LocateInProgramFiles("ramp.exe", true, "ramp");

			LogBox.Clear();

			if (_rampProgramPath == null)
				LogBox.WriteMessageWithColor("Red", "The RAMP pogram cannot be found!{0}", Environment.NewLine);

			_fileLists = GetFilesToArchive();
			DisplayInitialSummary();
			IsBusy = false;

			// Add one for the mets.xml file.
			maxProgBarValue = _fileLists.SelectMany(kvp => kvp.Value).Count() + 1;

			return (_rampProgramPath != null);
		}

		/// ------------------------------------------------------------------------------------
		private string GetPathToContributorFileInArchive(string personId, string fullFilePath)
		{
			return Path.Combine(Path.Combine("Contributors", personId), Path.GetFileName(fullFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<string>> GetFilesToArchive()
		{
			var filesInDir = Directory.GetFiles(_event.FolderPath);

			var msgKey = Path.GetFileName(filesInDir[0]);
			_progressMessages[msgKey] = string.Format("Adding Files for Event '{0}'", _eventTitle);

			var fileList = new Dictionary<string, IEnumerable<string>>();
			fileList[string.Empty] = filesInDir;

			foreach (var person in _event.GetAllParticipants()
				.Select(n => _personInformant.GetPersonByName(n)).Where(p => p != null))
			{
				filesInDir = Directory.GetFiles(person.FolderPath);
				fileList[person.Id] = filesInDir;

				msgKey = GetPathToContributorFileInArchive(person.Id, filesInDir[0]);
				msgKey = Path.Combine(Path.Combine("Contributors", person.Id), Path.GetFileName(filesInDir[0]));
				_progressMessages[msgKey] = string.Format("Adding Files for Contributor '{0}'", person.Id);
			}

			return fileList;
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayInitialSummary()
		{
			if (_fileLists.Count > 1)
				LogBox.WriteMessage("The following event and contributor files will be added to your archive.");
			else
			{
				LogBox.WriteWarning("There are no contributors for this event.");
				LogBox.WriteMessage(Environment.NewLine + "The following event files will be added to your archive.");
			}

			foreach (var kvp in _fileLists)
			{
				LogBox.WriteMessage(string.Format("{0}     {1}: {2}", Environment.NewLine,
					(kvp.Key == string.Empty ? "Event" : "Contributor"),
					(kvp.Key == string.Empty ? _eventTitle : kvp.Key)));

				foreach (var file in kvp.Value)
					LogBox.WriteMessageWithFontStyle(FontStyle.Regular, "          \u00B7 {0}", Path.GetFileName(file));
			}

			LogBox.ScrollToTop();
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
				prs.StartInfo.Arguments = "\"" + RampPackagePath + "\"";
				if (!prs.Start())
					return false;

				prs.WaitForInputIdle(8000);
				EnsureRampHasFocusAndWaitForPackageToUnlock();
				return true;
			}
			catch (InvalidOperationException)
			{
				EnsureRampHasFocusAndWaitForPackageToUnlock();
				return true;
			}
			catch (Exception e)
			{
				ReportError(e, "There was an error attempting to open the archive package in RAMP.");
				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void EnsureRampHasFocusAndWaitForPackageToUnlock()
		{
#if !__MonoCS__
			var processes = Process.GetProcessesByName("RAMP");
			if (processes.Length >= 1)
			{
				// I can't figure out why neither of these work.
				BringWindowToTop(processes[0].MainWindowHandle.ToInt32());
//				SetForegroundWindow(processes[0].MainWindowHandle.ToInt32());
			}
#else
			// Figure out how to do this in MONO
#endif
			// Every 4 seconds we'll check to see if the RAMP package is locked. When
			// it gets unlocked by RAMP, then we'll delete it.
			_timer = new Timer(CheckIfPackageFileIsLocked, RampPackagePath, 2000, 4000);
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

			var	success = CreateMetsFile();

			if (success)
				success = CreateRampPackage();

			CleanUp();

			if (success)
			{
				LogBox.WriteMessageWithColor(Color.DarkGreen,
					Environment.NewLine + "Ready to hand the package to RAMP");
			}

			IsBusy = false;
			return success;
		}

		#region Methods for creating mets file.
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
			yield return JSONUtils.MakeKeyValuePair("dc.title", _eventTitle);
			yield return JSONUtils.MakeKeyValuePair("broad_type", "wider_audience");
			yield return JSONUtils.MakeKeyValuePair("dc.type.scholarlyWork", "Data set");
			yield return JSONUtils.MakeKeyValuePair("dc.subject.silDomain", "LING:Linguistics", true);
			yield return JSONUtils.MakeKeyValuePair("type.domainSubtype.LING", "language documentation (LING)", true);

			var value = _event.MetaDataFile.GetStringValue("date", null);
			if (!string.IsNullOrEmpty(value))
				yield return JSONUtils.MakeKeyValuePair("dc.date.created", value);

			//// Return the event's situation as the the package's description.
			//value = _event.MetaDataFile.GetStringValue("situation", null);
			//if (value != null)
			//{
			//    var desc = JSONUtils.MakeKeyValuePair(" ", value) + "," +
			//        JSONUtils.MakeKeyValuePair("lang", string.Empty);

			//    yield return JSONUtils.MakeArrayFromValues("dc.description", new[] { desc });
			//}

			// Return the event's note as the abstract portion of the package's description.
			value = _event.MetaDataFile.GetStringValue("synopsis", null);
			if (!string.IsNullOrEmpty(value))
			{
				var abs = JSONUtils.MakeKeyValuePair(" ", value) + "," +
					JSONUtils.MakeKeyValuePair("lang", string.Empty);

				yield return JSONUtils.MakeArrayFromValues("dc.description.abstract", new[] { abs });
			}

			// Return JSON array of contributors
			var contributions = _event.MetaDataFile.GetValue("contributions", null) as ContributionCollection;
			if (contributions != null && contributions.Count > 0)
			{
				yield return JSONUtils.MakeArrayFromValues("dc.contributor",
					contributions.Select(c => GetContributorsMetsPair(c)));
			}

			// Return total duration of original audio/video recordings.
			var recExtent = GetRecordingExtent(_event.GetComponentFiles().Where(file =>
				file.GetAssignedRoles().FirstOrDefault(r =>
					r.Id == Settings.Default.OriginalRecordingComponentRoleId) != null)
					.Where(f => !string.IsNullOrEmpty(f.DurationString)).Select(f => f.DurationString));

			if (!string.IsNullOrEmpty(recExtent))
			{
				yield return JSONUtils.MakeKeyValuePair("format.extent.recording",
					string.Format("Total Length of Original Recordings: {0}", recExtent));
			}

			if (_fileLists != null)
			{
				// Return a list of types found in event's files (e.g. Text, Video, etc.).
				value = GetMode(_fileLists.SelectMany(f => f.Value));
				if (value != null)
					yield return value;

				// Return JSON array of event and contributor files with their descriptions.
				yield return JSONUtils.MakeArrayFromValues("files",
					GetSourceFilesForMetsData(_fileLists));
			}

			//yield return JSONUtils.MakeKeyValuePair("relation.requires.has", "Y");
			//yield return JSONUtils.MakeArrayFromValues("dc.relation.requires",
			//    new[] { JSONUtils.MakeKeyValuePair(" ", "SayMore") });
		}

		/// ------------------------------------------------------------------------------------
		public string GetMode(IEnumerable<string> files)
		{
			if (files == null)
				return null;

			var list = new HashSet<string>();

			foreach (var file in files)
			{
				if (ComponentRole.GetIsAudio(file))
					list.Add("Speech");
				if (ComponentRole.GetIsVideo(file))
					list.Add("Video");
				if (ComponentRole.GetIsText(file))
					list.Add("Text");
				if (ComponentRole.GetIsImage(file))
					list.Add("Photograph");
			}

			return JSONUtils.MakeBracketedListFromValues("dc.type.mode", list);
		}

		/// ------------------------------------------------------------------------------------
		public string GetContributorsMetsPair(Contribution contribution)
		{
			var roleCode = (contribution.Role != null &&
				Settings.Default.RampContributorRoles.Contains(contribution.Role.Code) ?
				contribution.Role.Code : string.Empty);

			return JSONUtils.MakeKeyValuePair(" ", contribution.ContributorName) +
				"," + JSONUtils.MakeKeyValuePair("role", roleCode);
		}

		/// ------------------------------------------------------------------------------------
		public string GetRecordingExtent(IEnumerable<string> durationStrings)
		{
			if (durationStrings == null)
				return null;

			var totalDuration = new TimeSpan(0);

			foreach (var duration in durationStrings)
			{
				TimeSpan span;
				if (TimeSpan.TryParse(duration, out span))
					totalDuration += span;
			}

			return (totalDuration.Ticks == 0 ? null : totalDuration.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSourceFilesForMetsData(IDictionary<string, IEnumerable<string>> fileLists)
		{
			foreach (var kvp in fileLists)
			{
				foreach (var file in kvp.Value)
				{
					var description = (kvp.Key == string.Empty ?
						"SayMore Event File" : "SayMore Contributor File");

					if (file.ToLower().EndsWith(".event"))
						description = "SayMore Event Metadata (XML)";
					else if (file.ToLower().EndsWith(".person"))
						description = "SayMore Contributor Metadata (XML)";
					else if (file.ToLower().EndsWith(".meta"))
						description = "SayMore File Metadata (XML)";

					var filePath = (kvp.Key == string.Empty ? Path.GetFileName(file) :
						GetPathToContributorFileInArchive(kvp.Key, file));

					yield return JSONUtils.MakeKeyValuePair(" ", filePath.Replace('\\', '/')) + "," +
						JSONUtils.MakeKeyValuePair("description", description) + "," +
						JSONUtils.MakeKeyValuePair("relationship", "source");
				}
			}
		}

		#endregion

		#region Creating RAMP package (zip file) in background thread.
		/// ------------------------------------------------------------------------------------
		public bool CreateRampPackage()
		{
			try
			{
				RampPackagePath = Path.Combine(Path.GetTempPath(), _event.Id + ".ramp");

				using (_worker = new BackgroundWorker())
				{
					_cancelProcess = false;
					_workerException = false;
					_worker.ProgressChanged += HandleBackgroundWorkerProgressChanged;
					_worker.WorkerReportsProgress = true;
					_worker.WorkerSupportsCancellation = true;
					_worker.DoWork += CreateZipFileInWorkerThread;
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
		private void CreateZipFileInWorkerThread(object sender, DoWorkEventArgs e)
		{
			try
			{
				using (var zip = new ZipFile())
				{
					// RAMP packages must not be compressed or RAMP can't read them.
					zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;

					foreach (var list in _fileLists)
					{
						zip.AddFiles(list.Value, list.Key == string.Empty ?
							string.Empty : "Contributors\\" + list.Key);
					}

					zip.AddFile(_metsFilePath, string.Empty);
					zip.SaveProgress += HandleZipSaveProgress;
					zip.Save(RampPackagePath);

					if (!_cancelProcess && _incrementProgressBarAction != null)
						Thread.Sleep(800);
				}
			}
			catch (Exception exception)
			{
				_worker.ReportProgress(0, new KeyValuePair<Exception, string>(exception,
					"There was an error attempting to create an archive for the event '{0}'."));

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
			if (_progressMessages.TryGetValue(e.CurrentEntry.FileName.Replace('/', '\\'), out msg))
				LogBox.WriteMessage(Environment.NewLine + msg);

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

			if (!string.IsNullOrEmpty(e.UserState as string))
				LogBox.WriteMessageWithFontStyle(FontStyle.Regular, "\t" + e.UserState);

			if (!_cancelProcess && _incrementProgressBarAction != null)
				_incrementProgressBarAction();
		}

		#endregion

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
			LogBox.WriteError(msg, _eventTitle);
			LogBox.WriteException(e);
		}

		#region Clean-up methods
		/// ------------------------------------------------------------------------------------
		public void CleanUp()
		{
			try { File.Delete(_metsFilePath); }
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
