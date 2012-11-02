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
using Localization;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.IO;
using Palaso.Progress;
using Palaso.UI.WindowsForms.Progress;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Utilities;
using SilTools;
using Timer = System.Threading.Timer;
using ZipFile = Ionic.Zip.ZipFile;

namespace SayMore.UI.Utilities
{
	public class ArchivingDlgViewModel
	{
#if !__MonoCS__
		[DllImport("User32.dll")]
		private static extern IntPtr SetForegroundWindow(int hWnd);

		[DllImport("User32.dll")]
		private static extern bool BringWindowToTop(int hWnd);
#endif

		private readonly string _sessionTitle;
		private Session _session;
		private PersonInformant _personInformant;
		private string _metsFilePath;
		private string _tempFolder;
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
		public ArchivingDlgViewModel(Session session, PersonInformant personInformant)
		{
			_session = session;
			_personInformant = personInformant;
			_sessionTitle = _session.MetaDataFile.GetStringValue("title", null) ?? _session.Id;

			LogBox = new LogBox();
			LogBox.TabStop = false;
			LogBox.ShowMenu = false;
			LogBox.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);

			foreach (var orphanedRampPackage in Directory.GetFiles(Path.GetTempPath(), "*.ramp"))
			{
				try { File.Delete(orphanedRampPackage); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(out int maxProgBarValue, Action incrementProgressBarAction)
		{
			IsBusy = true;
			_incrementProgressBarAction = incrementProgressBarAction;

			var text = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SearchingForRampMsg",
				"Searching for the RAMP program...");

			LogBox.WriteMessage(text);
			Application.DoEvents();
			_rampProgramPath = FileLocator.GetFromRegistryProgramThatOpensFileType(".ramp") ??
				FileLocator.LocateInProgramFiles("ramp.exe", true, "ramp");

			LogBox.Clear();

			if (_rampProgramPath == null)
			{
				text = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.RampNotFoundMsg",
					"The RAMP program cannot be found!");

				LogBox.WriteMessageWithColor("Red", text + Environment.NewLine);
			}

			_fileLists = GetFilesToArchive();
			DisplayInitialSummary();
			IsBusy = false;

			// One for analyzing each list, one for copying each file, one for saving each file in the zip file
			// and one for the mets.xml file.
			maxProgBarValue = _fileLists.Count + 2 * _fileLists.SelectMany(kvp => kvp.Value).Count() + 1;

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
			var filesInDir = Directory.GetFiles(_session.FolderPath);

			var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingSessionFilesProgressMsg", "Adding Files for Session '{0}'");
			var msgKey = Path.GetFileName(filesInDir[0]);
			_progressMessages[msgKey] = string.Format(fmt, _sessionTitle);

			var fileList = new Dictionary<string, IEnumerable<string>>();
			fileList[string.Empty] = filesInDir.Where(IncludeFileInArchive);

			fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingContributorFilesProgressMsg", "Adding Files for Contributor '{0}'");

			foreach (var person in _session.GetAllParticipants()
				.Select(n => _personInformant.GetPersonByName(n)).Where(p => p != null))
			{
				filesInDir = Directory.GetFiles(person.FolderPath);
				fileList[person.Id] = filesInDir.Where(IncludeFileInArchive);

				msgKey = GetPathToContributorFileInArchive(person.Id, filesInDir[0]);

				_progressMessages[msgKey] = string.Format(fmt, person.Id);
			}

			return fileList;
		}

		/// ------------------------------------------------------------------------------------
		private bool IncludeFileInArchive(string path)
		{
			return (Path.GetExtension(path).ToLower() != ".pfsx");
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayInitialSummary()
		{
			if (_fileLists.Count > 1)
			{
				LogBox.WriteMessage(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg1",
					"The following session and contributor files will be added to your archive."));
			}
			else
			{
				LogBox.WriteWarning(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.NoContributorsForSessionMsg",
					"There are no contributors for this session."));

				LogBox.WriteMessage(Environment.NewLine +
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg2",
						"The following session files will be added to your archive."));
			}

			var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ArchivingProgressMsg", "     {0}: {1}",
				"The first parameter is 'Session' or 'Contributor'. The second parameter is the session or contributor name.");

			foreach (var kvp in _fileLists)
			{
				var element = (kvp.Key == string.Empty ?
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SessionElementName", "Session") :
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ContributorElementName", "Contributor"));

				LogBox.WriteMessage(Environment.NewLine + string.Format(fmt, element,
					(kvp.Key == string.Empty ? _sessionTitle : kvp.Key)));

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
			if (!File.Exists(RampPackagePath))
			{
				ErrorReport.NotifyUserOfProblem("Eeeek. SayMore prematurely removed .ramp package.");
				return false;
			}

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
				ReportError(e, LocalizationManager.GetString("DialogBoxes.ArchivingDlg.StartingRampErrorMsg",
					"There was an error attempting to open the archive package in RAMP."));

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
			if (!FileSystemUtils.IsFileLocked(packageFile as string))
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
				LogBox.WriteMessageWithColor(Color.DarkGreen, Environment.NewLine +
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ReadyToCallRampMsg",
					"Ready to hand the package to RAMP"));
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
				_tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				Directory.CreateDirectory(_tempFolder);
				_metsFilePath = Path.Combine(_tempFolder, "mets.xml");
				File.WriteAllText(_metsFilePath, metsData);
			}
			catch (Exception e)
			{
				ReportError(e, LocalizationManager.GetString("DialogBoxes.ArchivingDlg.CreatingInternalReapMetsFileErrorMsg",
					"There was an error attempting to create a RAMP/REAP mets file for the session '{0}'."));

				return false;
			}

			if (_incrementProgressBarAction != null)
				_incrementProgressBarAction();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetMetsPairs()
		{
			yield return JSONUtils.MakeKeyValuePair("dc.title", _sessionTitle);
			yield return JSONUtils.MakeKeyValuePair("broad_type", "wider_audience");
			yield return JSONUtils.MakeKeyValuePair("dc.type.scholarlyWork", "Data set");
			yield return JSONUtils.MakeKeyValuePair("dc.subject.silDomain", "LING:Linguistics", true);
			yield return JSONUtils.MakeKeyValuePair("type.domainSubtype.LING", "language documentation (LING)", true);

			var value = _session.MetaDataFile.GetStringValue("date", null);
			if (!string.IsNullOrEmpty(value))
				yield return JSONUtils.MakeKeyValuePair("dc.date.created", value);

			//// Return the session's situation as the the package's description.
			//value = _session.MetaDataFile.GetStringValue("situation", null);
			//if (value != null)
			//{
			//    var desc = JSONUtils.MakeKeyValuePair(" ", value) + "," +
			//        JSONUtils.MakeKeyValuePair("lang", string.Empty);

			//    yield return JSONUtils.MakeArrayFromValues("dc.description", new[] { desc });
			//}

			// Return the session's note as the abstract portion of the package's description.
			value = _session.MetaDataFile.GetStringValue("synopsis", null);
			if (!string.IsNullOrEmpty(value))
			{
				var abs = JSONUtils.MakeKeyValuePair(" ", value) + "," +
					JSONUtils.MakeKeyValuePair("lang", string.Empty);

				yield return JSONUtils.MakeArrayFromValues("dc.description.abstract", new[] { abs });
			}

			// Return JSON array of contributors
			var contributions = _session.MetaDataFile.GetValue("contributions", null) as ContributionCollection;
			if (contributions != null && contributions.Count > 0)
			{
				yield return JSONUtils.MakeArrayFromValues("dc.contributor",
					contributions.Select(GetContributorsMetsPair));
			}

			// Return total duration of source audio/video recordings.
			var recExtent = GetRecordingExtent(_session.GetComponentFiles().Where(file =>
				file.GetAssignedRoles().FirstOrDefault(r =>
					r.Id == ComponentRole.kSourceComponentRoleId) != null)
					.Where(f => !string.IsNullOrEmpty(f.DurationString)).Select(f => f.DurationString));

			if (!string.IsNullOrEmpty(recExtent))
			{
				yield return JSONUtils.MakeKeyValuePair("format.extent.recording",
					string.Format("Total Length of Source Recordings: {0}", recExtent));
			}

			if (_fileLists != null)
			{
				// Return a list of types found in session's files (e.g. Text, Video, etc.).
				value = GetMode(_fileLists.SelectMany(f => f.Value));
				if (value != null)
					yield return value;

				// Return JSON array of session and contributor files with their descriptions.
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
				if (FileSystemUtils.GetIsAudio(file))
					list.Add("Speech");
				if (FileSystemUtils.GetIsVideo(file))
					list.Add("Video");
				if (FileSystemUtils.GetIsText(file))
					list.Add("Text");
				if (FileSystemUtils.GetIsImage(file))
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

			var totalDuration = TimeSpan.Zero;

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
						"SayMore Session File" : "SayMore Contributor File");

					if (file.ToLower().EndsWith(Settings.Default.SessionFileExtension))
						description = "SayMore Session Metadata (XML)";
					else if (file.ToLower().EndsWith(Settings.Default.PersonFileExtension))
						description = "SayMore Contributor Metadata (XML)";
					else if (file.ToLower().EndsWith(Settings.Default.MetadataFileExtension))
						description = "SayMore File Metadata (XML)";

					var fileName = Path.GetFileName(file).Replace(" ", "+");

					yield return JSONUtils.MakeKeyValuePair(" ", fileName) + "," +
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
				RampPackagePath = Path.Combine(Path.GetTempPath(), _session.Id + ".ramp");

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
				ReportError(e, LocalizationManager.GetString(
					"DialogBoxes.ArchivingDlg.CreatingZipFileErrorMsg",
					"There was a problem starting process to create zip file."));

				return false;
			}
			finally
			{
				_worker = null;
			}

			if (!File.Exists(RampPackagePath))
			{
				ErrorReport.NotifyUserOfProblem("Ack. SayMore failed to actually make the .ramp package.");
				return false;
			}

			return !_cancelProcess && !_workerException;
		}

		/// ------------------------------------------------------------------------------------
		private void CreateZipFileInWorkerThread(object sender, DoWorkEventArgs e)
		{
			try
			{
				// Before adding the files to the RAMP (zip) file, we need to copy all the
				// files to a temp folder, flattening out the directory structure and renaming
				// the files as needed to comply with REAP guidelines.
				// REVIEW: Are multiple periods and/or non-Roman script really a problem?

				_worker.ReportProgress(0, LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PreparingFilesMsg",
					"Analyzing component files"));

				var filesToCopyAndZip = new Dictionary<string, string>();
				foreach (var list in _fileLists)
				{
					_worker.ReportProgress(1 /* actual value ignored, progress just increments */,
						string.IsNullOrEmpty(list.Key) ? _session.Id: list.Key);
					foreach (var file in list.Value)
					{
						string newFileName = Path.GetFileName(file);
						newFileName = newFileName.Replace(" ", "+");
						if (list.Key != string.Empty)
							newFileName = "__Contributors__" + newFileName;
						filesToCopyAndZip[file] = Path.Combine(_tempFolder, newFileName);
					}
					if (_cancelProcess)
						return;
				}

				_worker.ReportProgress(0, LocalizationManager.GetString("DialogBoxes.ArchivingDlg.CopyingFilesMsg",
					"Copying files"));

				foreach (var fileToCopy in filesToCopyAndZip)
				{
					if (_cancelProcess)
						return;
					_worker.ReportProgress(1 /* actual value ignored, progress just increments */,
						Path.GetFileName(fileToCopy.Key));
					// Don't use File.Copy because it's asynchronous.
					if (fileToCopy.Key.EndsWith(AnnotationFileHelper.kAnnotationsEafFileSuffix))
					{
						// Fix EAF file to refer to modified name.
						AnnotationFileHelper annotationFileHelper;
						try
						{
							annotationFileHelper = AnnotationFileHelper.Load(fileToCopy.Key);
						}
						catch (Exception error)
						{
							ErrorReport.NotifyUserOfProblem(error, LocalizationManager.GetString(
								"DialogBoxes.ArchivingDlg.FileExcludedFromRAMP", "File excluded from RAMP package."));
							continue;
						}
						var mediaFileName = annotationFileHelper.MediaFileName;
						if (mediaFileName != null && mediaFileName.Contains(" "))
						{
							annotationFileHelper.SetMediaFile(mediaFileName.Replace(" ", "+"));
							annotationFileHelper.Root.Save(fileToCopy.Value);
							continue;
						}
					}
					CopyFile(fileToCopy.Key, fileToCopy.Value);
				}

				_worker.ReportProgress(0, LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SavingFilesInRAMPMsg",
					"Saving files in RAMP package"));

				using (var zip = new ZipFile())
				{
					// RAMP packages must not be compressed or RAMP can't read them.
					zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
					zip.AddFiles(filesToCopyAndZip.Values);
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
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.CreatingArchiveErrorMsg",
						"There was an error attempting to create an archive for the session '{0}'.")));

				_workerException = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		const int CopyBufferSize = 64 * 1024;
		static void CopyFile(string src, string dest)
		{
			using (var outputFile = File.OpenWrite(dest))
			{
				using (var inputFile = File.OpenRead(src))
				{
					var buffer = new byte[CopyBufferSize];
					int bytesRead;
					while ((bytesRead = inputFile.Read(buffer, 0, CopyBufferSize)) != 0)
					{
						outputFile.Write(buffer, 0, bytesRead);
					}
				}
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
			if (_progressMessages.TryGetValue(e.CurrentEntry.FileName, out msg))
				LogBox.WriteMessage(Environment.NewLine + msg);

			_worker.ReportProgress(e.EntriesSaved + 1, Path.GetFileName(e.CurrentEntry.FileName));
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
			{
				if (e.ProgressPercentage == 0)
				{
					LogBox.WriteMessageWithColor(Color.DarkGreen, Environment.NewLine + e.UserState);
					return;
				}

				LogBox.WriteMessageWithFontStyle(FontStyle.Regular, "\t" + e.UserState);
			}

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
				LogBox.WriteMessageWithColor(Color.Red, Environment.NewLine +
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.CancellingMsg", "Canceling..."));

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
			LogBox.WriteError(msg, _sessionTitle);
			LogBox.WriteException(e);
		}

		#region Clean-up methods
		/// ------------------------------------------------------------------------------------
		public void CleanUp()
		{
			try { Directory.Delete(_tempFolder, true); }
			catch { }

			_session = null;
			_personInformant = null;
		}

		/// ------------------------------------------------------------------------------------
		public void CleanUpTempRampPackage()
		{
			// Comment out as a test !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//try { File.Delete(RampPackagePath); }
			//catch { }

			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
		}

		#endregion
	}
}
