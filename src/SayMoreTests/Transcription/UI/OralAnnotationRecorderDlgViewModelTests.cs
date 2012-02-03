using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.UI.Utilities;

namespace SayMoreTests.Transcription.UI
{
	[TestFixture]
	public class OralAnnotationRecorderDlgViewModelTests
	{
		private TemporaryFolder _annotationFileFolder;
		private OralAnnotationRecorderDlgViewModel _model;
		private string _tempAudioFile;
		private Mock<ComponentFile> _componentFile;
		private string _tempAnnotationFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tempAnnotationFolder = Path.Combine(Path.GetTempPath(), "SayMoreOralAnnotations");
			_annotationFileFolder = new TemporaryFolder("OralAnnotationRecorderDlgViewModelTests");

			_tempAudioFile = MPlayerMediaInfoTests.GetLongerTestAudioFile();
			var tier = new TimeOrderTier(_tempAudioFile);
			tier.AddSegment(0f, 5f);
			tier.AddSegment(5f, 10f);
			tier.AddSegment(15f, 20f);
			tier.AddSegment(25f, 30f);

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new[] { tier });

			_componentFile = new Mock<ComponentFile>();
			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns(_tempAudioFile);
		}

		/// ------------------------------------------------------------------------------------
		void CreateModel()
		{
			_model = new OralAnnotationRecorderDlgViewModel(_componentFile.Object, OralAnnotationType.Careful);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_annotationFileFolder.Dispose();
			_annotationFileFolder = null;

			if (_model != null)
				_model.Dispose();

			File.Delete(_tempAudioFile);

			if (Directory.Exists(_tempAnnotationFolder))
				Directory.Delete(_tempAnnotationFolder, true);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CopyOralAnnotationsToTempLocation_FolderDoesNotExist_CreatingModelCreatesIt()
		{
			Assert.IsFalse(Directory.Exists(_tempAnnotationFolder));
			CreateModel();
			Assert.IsTrue(Directory.Exists(_tempAnnotationFolder));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Disposing_RemovesTempLocationFolder()
		{
			CreateModel();
			Assert.IsTrue(Directory.Exists(_tempAnnotationFolder));
			_model.Dispose();
			Assert.IsFalse(Directory.Exists(_tempAnnotationFolder));
		}

		// Write tests for these methods. Also figure out a way to test recording, etc.
		//SelectSegment
		//SaveNewOralAnnoationsInPermanentLocation
		//CopyAnnotationFiles
		//GetDoesAnnotationExistForCurrentSegment
		//GetPathToAnnotationFileForSegment
		//GetPathToCurrentAnnotationFile
		//GetDoesSegmentHaveAnnotationFile
		//GetTimeWherePlaybackShouldStart
		//GetStartOfCurrentSegment
		//GetEndOfCurrentSegment
		//GotoEndOfSegments
		//GetDoesHaveSegments
		//SelectSegmentFromTime
		//SelectSegment
		//SaveNewSegment
		//EraseAnnotation
		//RenameAnnotationForResizedSegment
	}
}
