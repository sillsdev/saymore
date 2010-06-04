using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Palaso.Code;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Handles the logic and functionality of copying large files and giving visual feedback.
	/// Used with a view which does the actual
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CopyFilesViewModel : IDisposable
	{
		private readonly KeyValuePair<string, string>[] _sourceDestinationPathPairs;
		private readonly long _totalBytes;
		private Thread _workerThread;
		private long _totalBytesCopied;
		private Exception _encounteredError;

		public event EventHandler OnFinished;

		public int IndexOfCurrentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public CopyFilesViewModel(IEnumerable<KeyValuePair<string, string>> sourceDestinationPathPairs)
		{
			Guard.Against(sourceDestinationPathPairs.Count() == 0, "No Files To Copy");
			_sourceDestinationPathPairs = sourceDestinationPathPairs.ToArray();
			_totalBytes = 0;

			foreach (var pair in sourceDestinationPathPairs)
				_totalBytes += new FileInfo(pair.Key).Length;

			IndexOfCurrentFile = -1;
			BeforeCopyingFileRaised = source => { };
		}

		/// ------------------------------------------------------------------------------------
		public int TotalPercentage
		{
			get { return (int)(_totalBytes == 0 ? 0 : 100 * (_totalBytesCopied/(double)_totalBytes)); }
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
					return "Copying failed:" + _encounteredError.Message;//todo: these won't be user friendly
				}

				if (Copying)
				{
					return string.Format("Copying {0} of {1} Files, ({2} of {3} Megabytes)",
						1 + IndexOfCurrentFile, _sourceDestinationPathPairs.Count(),
						Megs(_totalBytesCopied), Megs(_totalBytes));
				}

				return (Finished ? "Finished" : "Waiting to start...");
			}

			set { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called with source and destination paths, just before each copy
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Action<string> BeforeCopyingFileRaised
		{
			get; set;
		}

		/// ------------------------------------------------------------------------------------
		private string Megs(long bytes)
		{
			return (bytes / (1024 * 1024)).ToString();
		}

		/// ------------------------------------------------------------------------------------
		public void Start()
		{
			_workerThread = new Thread(DoCopying);
			_workerThread.Start();
		}

		/// ------------------------------------------------------------------------------------
		public void DoCopying()
		{
			try
			{
				for (IndexOfCurrentFile = 0; IndexOfCurrentFile < _sourceDestinationPathPairs.Count(); IndexOfCurrentFile++)
				{
					KeyValuePair<string, string> pair = _sourceDestinationPathPairs[IndexOfCurrentFile];
					BeforeCopyingFileRaised(pair.Key);
					var buffer = new byte[1000 * 1024];
					var sourceFileInfo = new FileInfo(pair.Key);

					if (File.Exists(pair.Value))
					{
						if (new FileInfo(pair.Value).CreationTimeUtc== sourceFileInfo.CreationTimeUtc &&
							new FileInfo(pair.Value).Length == sourceFileInfo.Length &&
							new FileInfo(pair.Value).LastWriteTimeUtc == sourceFileInfo.LastWriteTimeUtc)
						{
							// enhance.. would be better if we have a reporting method we could talk to,
							// that this would be up to the UI how/when/whether to display something.
							Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
								string.Format("The file {0} appears unchanged since it was copied before, so it will be skipped.", Path.GetFileName(pair.Value)));

							continue;
						}

						_encounteredError = new ApplicationException(string.Format("The file {0} already exists", pair.Value));
						return;
					}

					try
					{
						using (var source = new FileStream(pair.Key,FileMode.Open))
						using (var dest = new FileStream(pair.Value, FileMode.CreateNew))
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
							} while (bytesRead > 0);
						}

						new FileInfo(pair.Value).CreationTimeUtc = sourceFileInfo.CreationTimeUtc;
						new FileInfo(pair.Value).LastWriteTimeUtc = sourceFileInfo.LastWriteTimeUtc;
						sourceFileInfo.Attributes  = (FileAttributes)(sourceFileInfo.Attributes - FileAttributes.Archive);//enhance... could be under control of the client
					}
					catch (Exception e)
					{
						if (File.Exists(pair.Value))
							File.Delete(pair.Value);

						throw;
					}
					//File.Copy(pair.Key, pair.Value, false);
				}

				IndexOfCurrentFile = -2;
			}
			catch(Exception e)
			{
				_encounteredError = e;
			}
			finally
			{
				IndexOfCurrentFile = -2;
				if (OnFinished != null)
					OnFinished.Invoke(_encounteredError, null);
			}
		}

		public void Dispose()
		{
//todo
		}
	}
}
