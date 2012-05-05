using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.Code;
using SayMore.Utilities.LowLevelControls;

namespace SayMore.Utilities.NewEventsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Handles the logic and functionality of copying large files and giving visual feedback.
	/// Used with a view which does the actual
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CopyFilesViewModel : IProgressViewModel, IDisposable
	{
		private const int kUpdateStatus = -1;

		private readonly KeyValuePair<string, string>[] _sourceDestinationPathPairs;
		private readonly long _totalBytes;
		private BackgroundWorker _worker;
		private long _totalBytesCopied;
		private Exception _encounteredError;
		private readonly bool _overwrite;

		public event EventHandler<ProgressFinishedArgs> OnFinished;
		public event EventHandler OnUpdateProgress;
		public event EventHandler OnUpdateStatus;

		public int IndexOfCurrentFile { get; private set; }
		public Action<string, string> FileCopyFailedAction { get; set; }

		/// ------------------------------------------------------------------------------------
		public CopyFilesViewModel(IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs)
			: this(sourceDestinationPathPairs, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		public CopyFilesViewModel(IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs,
			bool overwrite)
		{
			var notFilesToCopyMsg = LocalizationManager.GetString(
				"Miscellaneous.CopyFilesControl.NoFilesToCopyMsg", "No Files To Copy");

			_sourceDestinationPathPairs = sourceDestinationPathPairs.ToArray();
			Guard.Against(_sourceDestinationPathPairs.Length == 0, notFilesToCopyMsg);
			_totalBytes = 0;

			foreach (var pair in _sourceDestinationPathPairs)
				_totalBytes += new FileInfo(pair.Key).Length;

			IndexOfCurrentFile = -1;
			BeforeFileCopiedAction = source => { };

			_overwrite = overwrite;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_worker != null)
				_worker.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		public int CurrentProgressValue
		{
			get { return (int)(_totalBytes == 0 ? 0 : 100 * (_totalBytesCopied/(double)_totalBytes)); }
		}

		/// ------------------------------------------------------------------------------------
		public int MaximumProgressValue
		{
			get { return 100; }
		}

		/// ------------------------------------------------------------------------------------
		public bool Copying
		{
			get { return IndexOfCurrentFile >= 0; }
		}

		/// ------------------------------------------------------------------------------------
		public bool Finished
		{
			get { return IndexOfCurrentFile == -2; }
		}

		/// ------------------------------------------------------------------------------------
		public string StatusString
		{
			get
			{
				if (_encounteredError != null)
				{
					// TODO: these won't be user friendly
					return string.Format(LocalizationManager.GetString("Miscellaneous.CopyFilesControl.CopyFailedMsg",
						"Copying failed: {0}"), _encounteredError.Message);
				}

				if (Copying)
				{
					return string.Format(LocalizationManager.GetString("Miscellaneous.CopyFilesControl.CopyingProgressMsg",
						"Copying {0} of {1} Files, ({2} of {3} Megabytes)"),
						1 + IndexOfCurrentFile, _sourceDestinationPathPairs.Count(),
						Megs(_totalBytesCopied), Megs(_totalBytes));
				}

				return (Finished ?
					LocalizationManager.GetString("Miscellaneous.CopyFilesControl.CopyingFinishedMsg", "Finished") :
					LocalizationManager.GetString("Miscellaneous.CopyFilesControl.CopyWaitingMsg", "Waiting to start..."));
			}

			set { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called just before a file is copied.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Action<string> BeforeFileCopiedAction { get; set; }

		/// ------------------------------------------------------------------------------------
		private string Megs(long bytes)
		{
			return (bytes / (1024 * 1024)).ToString(CultureInfo.InvariantCulture);
		}

		/// ------------------------------------------------------------------------------------
		public void Start()
		{
			_worker = new BackgroundWorker();
			_worker.WorkerSupportsCancellation = true;
			_worker.WorkerReportsProgress = true;
			_worker.ProgressChanged += HandleWorkerProgressChanged;
			_worker.RunWorkerCompleted += HandleWorkerFinished;
			_worker.DoWork += DoCopying;
			_worker.RunWorkerAsync();
			while (_worker.IsBusy) { Application.DoEvents(); }
		}

		/// ------------------------------------------------------------------------------------
		public static void Copy(string srcFile, string dstFile)
		{
			Copy(srcFile, dstFile, false);
		}

		/// ------------------------------------------------------------------------------------
		public static void Copy(string srcFile, string dstFile, bool overwrite)
		{
			var model = new CopyFilesViewModel(
				new[] { new KeyValuePair<string, string>(srcFile, dstFile) }, overwrite);

			model.DoCopying(null, null);
		}

		/// ------------------------------------------------------------------------------------
		public void DoCopying(object sender, DoWorkEventArgs args)
		{
			try
			{
				for (IndexOfCurrentFile = 0; IndexOfCurrentFile < _sourceDestinationPathPairs.Count(); IndexOfCurrentFile++)
				{
					if (_worker != null && _worker.CancellationPending)
						continue;

					var pair = _sourceDestinationPathPairs[IndexOfCurrentFile];
					ReportProgress(kUpdateStatus, pair.Key);
					var sourceFileInfo = new FileInfo(pair.Key);

					if (!_overwrite && CheckIfDestFileExists(sourceFileInfo, pair.Value))
						continue;

					if (_encounteredError != null)
					{
						Cancel();
						return;
					}

					try
					{
						CopyFile(sourceFileInfo, pair.Value);
					}
					catch (Exception)
					{
						if (FileCopyFailedAction != null)
							FileCopyFailedAction(pair.Key, pair.Value);

						throw;
					}
				}

				IndexOfCurrentFile = -2;
			}
			catch (Exception e)
			{
				_encounteredError = e;
				Cancel();
			}
			finally
			{
				IndexOfCurrentFile = -2;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ReportProgress(int percent, string filename)
		{
			if (_worker == null)
				return;

			if (filename == null)
				_worker.ReportProgress(percent);
			else
				_worker.ReportProgress(percent, filename);
		}

		/// ------------------------------------------------------------------------------------
		public void Cancel()
		{
			if (_worker != null)
				_worker.CancelAsync();
		}

		/// ------------------------------------------------------------------------------------
		private bool CheckIfDestFileExists(FileInfo srcFile, string dstFile)
		{
			if (!File.Exists(dstFile))
				return false;

			var finfo = new FileInfo(dstFile);
			if (finfo.CreationTimeUtc == srcFile.CreationTimeUtc &&
				finfo.Length == srcFile.Length &&
				finfo.LastWriteTimeUtc == srcFile.LastWriteTimeUtc)
			{
				// enhance.. would be better if we have a reporting method we could talk to,
				// that this would be up to the UI how/when/whether to display something.
				var msg = LocalizationManager.GetString("Miscellaneous.CopyFilesControl.UnchangedFileMsg",
					"The file {0} appears unchanged since it was copied before, so it will be skipped.");

				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(msg, Path.GetFileName(dstFile));
				_totalBytesCopied += finfo.Length;
				ReportProgress(1, null);
				return true;
			}

			var fmt = LocalizationManager.GetString("Miscellaneous.CopyFilesControl.FileExistsMsg", "The file '{0}' already exists");
			_encounteredError = new ApplicationException(string.Format(fmt, dstFile));

			Cancel();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void CopyFile(FileInfo srcFile, string dstFile)
		{
			if (BeforeFileCopiedAction != null)
				BeforeFileCopiedAction(srcFile.FullName);

			var buffer = new byte[1000 * 1024];

			using (var source = new FileStream(srcFile.FullName, FileMode.Open))
			using (var dest = new FileStream(dstFile, (_overwrite ? FileMode.Create : FileMode.CreateNew)))
			{
				int bytesRead;
				do
				{
					bytesRead = source.Read(buffer, 0, buffer.Length);

					if (bytesRead > 0)
					{
						dest.Write(buffer, 0, bytesRead);
						_totalBytesCopied += bytesRead;
					}

					ReportProgress(1, null);
				}
				while (bytesRead > 0);
			}

			new FileInfo(dstFile)
			{
				CreationTimeUtc = srcFile.CreationTimeUtc,
				LastWriteTimeUtc = srcFile.LastWriteTimeUtc,
			};

			srcFile.Attributes = (FileAttributes)(srcFile.Attributes - FileAttributes.Archive);//enhance... could be under control of the client
		}

		/// ------------------------------------------------------------------------------------
		void HandleWorkerFinished(object sender, RunWorkerCompletedEventArgs e)
		{
			if (OnFinished != null)
				OnFinished.Invoke(this, new ProgressFinishedArgs(false, _encounteredError));
		}

		/// ------------------------------------------------------------------------------------
		void HandleWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage == kUpdateStatus)
			{
				if (OnUpdateStatus != null)
					OnUpdateStatus(this, EventArgs.Empty);
			}
			else if (OnUpdateProgress != null)
			{
				OnUpdateProgress(this, EventArgs.Empty);
			}
		}
	}
}
