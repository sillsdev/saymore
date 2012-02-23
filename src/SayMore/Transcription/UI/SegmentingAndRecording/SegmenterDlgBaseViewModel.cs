using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using NAudio.Wave;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.NewEventsFromFiles;
using SayMore.UI.Utilities;

namespace SayMore.Transcription.UI
{
	public class SegmenterDlgBaseViewModel : IDisposable
	{
		public ComponentFile ComponentFile { get; protected set; }
		public WaveStream OrigWaveStream { get; protected set; }
		public bool HaveSegmentBoundaries { get; set; }
		public Action UpdateDisplayProvider { get; set; }
		public TierCollection Tiers { get; protected set; }
		public TimeTier TimeTier { get; protected set; }

		public string TempOralAnnotationsFolder { get; protected set; }
		public string OralAnnotationsFolder { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public SegmenterDlgBaseViewModel(ComponentFile file)
		{
			ComponentFile = file;
			OrigWaveStream = new WaveFileReader(ComponentFile.PathToAnnotatedFile);

			Tiers = file.GetAnnotationFile() != null ?
				file.GetAnnotationFile().Tiers.Copy() : new TierCollection(ComponentFile.PathToAnnotatedFile);

			TimeTier = Tiers.GetTimeTier();

			if (TimeTier == null)
			{
				TimeTier = new TimeTier(ComponentFile.PathToAnnotatedFile);
				Tiers.Insert(0, TimeTier);
			}

			OralAnnotationsFolder = ComponentFile.PathToAnnotatedFile +
				Settings.Default.OralAnnotationsFolderAffix;

			TempOralAnnotationsFolder = CopyOralAnnotationsToTempLocation();
			TimeTier.SetAudioSegmentFileFolder(TempOralAnnotationsFolder);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Dispose()
		{
			if (OrigWaveStream != null)
			{
				OrigWaveStream.Close();
				OrigWaveStream.Dispose();
			}

			try
			{
				Directory.Delete(TempOralAnnotationsFolder, true);
			}
			catch { }
		}

		#region Methods for copying oral annotation recorded segment files to and from temp. location
		/// ------------------------------------------------------------------------------------
		public string CopyOralAnnotationsToTempLocation()
		{
			var tmpFolder = Path.Combine(Path.GetTempPath(), "SayMoreOralAnnotations");

			if (Directory.Exists(OralAnnotationsFolder))
				CopyAnnotationFiles(OralAnnotationsFolder, tmpFolder);
			else
				FileSystemUtils.CreateDirectory(tmpFolder);

			return tmpFolder;
		}

		/// ------------------------------------------------------------------------------------
		private bool CopyAnnotationFiles(string sourceFolder, string targetFolder)
		{
			FileSystemUtils.RemoveDirectory(targetFolder);

			int retryCount = 0;
			Exception error = null;

			while (retryCount < 10)
			{
				try
				{
					FileSystemUtils.CreateDirectory(targetFolder);

					var pairs = Directory.GetFiles(sourceFolder, "*.wav", SearchOption.TopDirectoryOnly)
						.Select(f => new KeyValuePair<string, string>(f, Path.Combine(targetFolder, Path.GetFileName(f))))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					if (pairs.Count > 0)
					{
						var model = new CopyFilesViewModel(pairs);
						model.Start();
					}

					return true;
				}
				catch (Exception e)
				{
					Application.DoEvents();
					retryCount++;
					error = e;
				}
			}

			ErrorReport.NotifyUserOfProblem(error,
				"Error trying to copy oral annotation files from '{0}' to '{1}'",
				sourceFolder, targetFolder);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SaveNewOralAnnoationsInPermanentLocation()
		{
			return CopyAnnotationFiles(TempOralAnnotationsFolder, OralAnnotationsFolder);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundariesChanged { get; protected set; }

		/// ------------------------------------------------------------------------------------
		protected virtual string ProgramAreaForUsageReporting
		{
			get { return "ManualSegmentation"; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool WereChangesMade
		{
			get { return SegmentBoundariesChanged; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual WaveStream GetStreamFromAudio(string audioFilePath)
		{
			Exception error;

			return (WaveFileUtils.GetOneChannelStreamFromAudio(audioFilePath, out error) ??
				new WaveFileReader(audioFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> GetSegmentEndBoundaries()
		{
			return   TimeTier.Segments.Select(s => TimeSpan.FromSeconds(s.End));
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetEndOfLastSegment()
		{
			return (TimeTier.Segments.Count == 0 ? TimeSpan.Zero :
				TimeSpan.FromSeconds(TimeTier.Segments[TimeTier.Segments.Count - 1].End));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the time between the proposed end time and the closest
		/// boundary to it's left will make a long enough segment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsSegmentLongEnough(TimeSpan proposedEndTime)
		{
			var propEndTime = (float)proposedEndTime.TotalSeconds;
			var minSize = Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds / 1000f;

			for (int i = TimeTier.Segments.Count - 1; i >= 0; i--)
			{
				if (TimeTier.Segments[i].End < propEndTime)
					return (propEndTime - TimeTier.Segments[i].End >= minSize);
			}

			return (propEndTime >= minSize);
		}

		/// ------------------------------------------------------------------------------------
		public int GetSegmentCount()
		{
			return TimeTier.Segments.Count;
		}

		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundaryMoved(TimeSpan oldEndTime, TimeSpan newEndTime)
		{
			if (oldEndTime == newEndTime)
				return false;

			var result = TimeTier.ChangeSegmentsEndBoundary(
				(float)oldEndTime.TotalSeconds, (float)newEndTime.TotalSeconds);

			if (result != BoundaryModificationResult.Success)
				return false;

			SegmentBoundariesChanged = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<TimeSpan> InsertNewBoundary(TimeSpan newBoundary)
		{
			if (Tiers.InsertTierSegment((float)newBoundary.TotalSeconds) == BoundaryModificationResult.Success)
				SegmentBoundariesChanged = true;

			return GetSegmentEndBoundaries();
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool DeleteBoundary(TimeSpan boundary)
		{
			var seg = TimeTier.GetSegmentHavingEndBoundary((float)boundary.TotalSeconds);

			if (!Tiers.RemoveTierSegments(TimeTier.GetIndexOfSegment(seg)))
				return false;

			SegmentBoundariesChanged = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool CanMoveBoundary(TimeSpan boundaryToAdjust, int millisecondsToMove)
		{
			var secondsToMove = Math.Abs(millisecondsToMove) / 1000f;
			var boundary = (float)boundaryToAdjust.TotalSeconds;

			return (millisecondsToMove < 0 ?
				TimeTier.CanBoundaryMoveLeft(boundary, secondsToMove) :
				TimeTier.CanBoundaryMoveRight(boundary, secondsToMove, (float)OrigWaveStream.TotalTime.TotalSeconds));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the time tier contains more segments than all the other text tiers, this method
		/// will add a number of segments to each text tier so each tier contains the same
		/// number of segments. Added text segments are filled with an empty string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateMissingTextSegmentsToMatchTimeSegmentCount()
		{
			foreach (var textTier in Tiers.OfType<TextTier>()
				.Where(t => t.Segments.Count < TimeTier.Segments.Count))
			{
				while (textTier.Segments.Count < TimeTier.Segments.Count)
					textTier.AddSegment(string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeUpdateDisplayAction()
		{
			if (UpdateDisplayProvider != null)
				UpdateDisplayProvider();
		}
	}
}
