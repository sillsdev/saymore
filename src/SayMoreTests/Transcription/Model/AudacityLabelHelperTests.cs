using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;
using SayMoreTests.Model.Files.DataGathering;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class AudacityLabelHelperTests
	{
		private TemporaryFolder _folder;
		private string _audioFilePath;
		private AudacityLabelHelper _helper;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_folder = new TemporaryFolder("AudacityLabelHelperTests");
			_audioFilePath = FileStatisticsTests.CreateRecording(_folder.Path);
			_helper = new AudacityLabelHelper(new string[] { }, _audioFilePath);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_folder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FixUpLabelInfo_LabelsAreRangesStartingAtZero_MakesNoChanges()
		{
			var lblInfo = new List<AudacityLabelInfo>();
			lblInfo.Add(new AudacityLabelInfo { Start = 0f, Length = 10f });
			lblInfo.Add(new AudacityLabelInfo { Start = 11f, Length = 20f });
			lblInfo.Add(new AudacityLabelInfo { Start = 21f, Length = 30f });

			var newLblInfo = _helper.FixUpLabelInfo(lblInfo).ToList();

			Assert.AreEqual(3, newLblInfo.Count);
			Assert.AreEqual(0f, newLblInfo[0].Start);
			Assert.AreEqual(10f, newLblInfo[0].Length);

			Assert.AreEqual(11f, newLblInfo[1].Start);
			Assert.AreEqual(20f, newLblInfo[1].Length);

			Assert.AreEqual(21f, newLblInfo[2].Start);
			Assert.AreEqual(30f, newLblInfo[2].Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FixUpLabelInfo_LabelsAreRangesNotStartingAtZero_AddsInfoAtZero()
		{
			var lblInfo = new List<AudacityLabelInfo>();
			lblInfo.Add(new AudacityLabelInfo { Start = 11f, Length = 20f });
			lblInfo.Add(new AudacityLabelInfo { Start = 21f, Length = 30f });

			var newLblInfo = _helper.FixUpLabelInfo(lblInfo).ToList();

			Assert.AreEqual(3, newLblInfo.Count);
			Assert.AreEqual(0f, newLblInfo[0].Start);
			Assert.AreEqual(11f, newLblInfo[0].Length);

			Assert.AreEqual(11f, newLblInfo[1].Start);
			Assert.AreEqual(20f, newLblInfo[1].Length);

			Assert.AreEqual(21f, newLblInfo[2].Start);
			Assert.AreEqual(30f, newLblInfo[2].Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FixUpLabelInfo_LabelsArePointsStartingAtZero_CalculatesLengths()
		{
			var lblInfo = new List<AudacityLabelInfo>();
			lblInfo.Add(new AudacityLabelInfo { Start = 0f });
			lblInfo.Add(new AudacityLabelInfo { Start = 0.5f });
			lblInfo.Add(new AudacityLabelInfo { Start = 0.9f });

			var newLblInfo = _helper.FixUpLabelInfo(lblInfo).ToList();

			Assert.AreEqual(3, newLblInfo.Count);
			Assert.AreEqual(0.5f, newLblInfo[0].Length);
			Assert.AreEqual(0.4f, newLblInfo[1].Length);

			// The length of last segment is the length of the
			// media file minus start point of the last segment.
			Assert.AreEqual(0.55f, newLblInfo[2].Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleLabelInfo_StartStopPointsOnly_ReturnsCorrectInfoWithNoText()
		{
			var ali = _helper.CreateSingleLabelInfo(new[] { "4.123", "9.123" });

			Assert.AreEqual(4.123f, ali.Start);
			Assert.AreEqual(5f, ali.Length);
			Assert.IsNull(ali.Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateSingleLabelInfo_IncludeText_ReturnsInfoWithText()
		{
			var ali = _helper.CreateSingleLabelInfo(new[] { null, null, "blah" });
			Assert.AreEqual("blah", ali.Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTiers_ReturnsCorrectTierCount()
		{
			Assert.AreEqual(2, _helper.GetTiers().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTiers_ReturnsCorrectTierTypes()
		{
			var tiers = _helper.GetTiers();

			Assert.IsInstanceOf<AudioTier>(tiers.ElementAt(0));
			Assert.AreEqual(TierType.Audio, tiers.ElementAt(0).DataType);
			Assert.IsInstanceOf<TextTier>(tiers.ElementAt(1));
			Assert.AreEqual(TierType.Text, tiers.ElementAt(1).DataType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTiers_ReturnsCorrectSegmentTypesInTiers()
		{
			_helper = new AudacityLabelHelper(new[] { "0.925\t1.075\tblah" }, _audioFilePath);
			var tiers = _helper.GetTiers();

			Assert.IsInstanceOf<IMediaSegment>(tiers.ElementAt(0).GetSegment(0));
			Assert.IsInstanceOf<IMediaSegment>(tiers.ElementAt(0).GetSegment(1));
			Assert.IsInstanceOf<ITextSegment>(tiers.ElementAt(1).GetSegment(0));
			Assert.IsInstanceOf<ITextSegment>(tiers.ElementAt(1).GetSegment(1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTiers_ReturnsCorrectSegmentInfoInTiers()
		{
			_helper = new AudacityLabelHelper(new[] { "0.925\t1.075\tblah" }, _audioFilePath);
			var tiers = _helper.GetTiers();

			Assert.AreEqual(0f, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(0))).MediaStart);
			Assert.AreEqual(0.925f, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(0))).MediaLength);
			Assert.AreEqual(_audioFilePath, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(0))).MediaFile);
			Assert.AreEqual(0.925f, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(1))).MediaStart);
			Assert.AreEqual(0.15f, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(1))).MediaLength);
			Assert.AreEqual(_audioFilePath, ((IMediaSegment)(tiers.ElementAt(0).GetSegment(1))).MediaFile);

			Assert.IsNull(((ITextSegment)(tiers.ElementAt(1).GetSegment(0))).GetText());
			Assert.AreEqual("blah", ((ITextSegment)(tiers.ElementAt(1).GetSegment(1))).GetText());
		}
	}
}
