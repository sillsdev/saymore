using System.Collections.Generic;
using System.IO;
using System.Linq;
using L10NSharp;
using Moq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.ComponentEditors;
using SayMoreTests.Utilities;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class MetadataSearchTests
	{
		private TemporaryFolder _parentFolder;
		private Mock<PersonInformant> _personInformant;

		// Exposes the sessions a test wants searched, bypassing the need for a full
		// ProjectContext/Project (and the file-system-backed session repository they require).
		private class TestableMetadataSearch : MetadataSearch
		{
			private readonly IEnumerable<Session> _sessions;

			public TestableMetadataSearch(IEnumerable<Session> sessions) : base(null)
			{
				_sessions = sessions;
			}

			protected override IEnumerable<Session> GetAllSessions()
			{
				return _sessions;
			}
		}

		[SetUp]
		public void Setup()
		{
			LocalizationManager.StrictInitializationMode = false;
			_parentFolder = new TemporaryFolder("metadataSearchTest");
			_personInformant = new Mock<PersonInformant>();
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Creates a minimal, file-backed DummySession whose metadata includes a
		/// single searchable field.</summary>
		/// ------------------------------------------------------------------------------------
		private DummySession CreateSessionWithMatchingField(string sessionName, string fieldId, string fieldValue)
		{
			var sessionsFolder = Path.Combine(_parentFolder.Path, Session.kFolderName);
			Directory.CreateDirectory(sessionsFolder);
			var folder = Path.Combine(sessionsFolder, sessionName + "-session");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, sessionName + ".mp3")).Close();

			var session = new DummySession(sessionsFolder, sessionName, _personInformant.Object)
			{
				MediaFiles = new ComponentFile[0]
			};
			// CallBase so the real (virtual) IsMatch/GetSearchableFieldIds logic under test
			// actually runs, instead of the mock's default (unconfigured-member) behavior.
			session.MetaFile.CallBase = true;
			session.MetaFile.Object.MetaDataFieldValues.Add(new FieldInstance(fieldId, fieldValue));
			return session;
		}

		[Test]
		public void SearchSessions_QueryMatchesMultipleSessions_ReturnsAllMatchingSessionIds()
		{
			var session1 = CreateSessionWithMatchingField("Alpha", FileType.kNotesFieldName, "find the needle here");
			var session2 = CreateSessionWithMatchingField("Beta", FileType.kNotesFieldName, "another needle sighting");
			var nonMatchingSession = CreateSessionWithMatchingField("Gamma", FileType.kNotesFieldName, "nothing relevant");

			var search = new TestableMetadataSearch(new[] { session1, session2, nonMatchingSession });

			var results = search.SearchSessions("needle").ToList();

			Assert.That(results, Is.EquivalentTo(new[] { session1.Id, session2.Id }));
		}

		[Test]
		public void SearchSessions_QueryMatchesNoSessions_ReturnsEmpty()
		{
			var nonMatchingSession1 = CreateSessionWithMatchingField("Alpha", FileType.kNotesFieldName, "nothing relevant");
			var nonMatchingSession2 = CreateSessionWithMatchingField("Beta", FileType.kNotesFieldName, "still nothing relevant");

			var search = new TestableMetadataSearch(new[] { nonMatchingSession1, nonMatchingSession2 });

			var results = search.SearchSessions("needle").ToList();

			Assert.That(results, Is.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Creates a minimal, file-backed DummySession whose only session-level
		/// component file is a mocked annotation (.eaf) file with a single transcription-tier
		/// segment containing the given text.</summary>
		/// ------------------------------------------------------------------------------------
		private DummySession CreateSessionWithAnnotationText(string sessionName, string transcriptionText)
		{
			var sessionsFolder = Path.Combine(_parentFolder.Path, Session.kFolderName);
			Directory.CreateDirectory(sessionsFolder);
			var folder = Path.Combine(sessionsFolder, sessionName + "-session");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, sessionName + ".mp3")).Close();

			var transcriptionTier = new TextTier(TextTier.ElanTranscriptionTierId);
			transcriptionTier.Segments.Add(new AnnotationSegment(transcriptionTier, transcriptionText));
			var tiers = new TierCollection { transcriptionTier };

			// CallBase so the real (virtual) IsMatch logic under test actually runs (reading
			// the stubbed Tiers), instead of the mock's default (unconfigured-member) behavior.
			var annotationFile = new Mock<AnnotationComponentFile> { CallBase = true };
			annotationFile.Setup(f => f.Tiers).Returns(tiers);

			return new DummySession(sessionsFolder, sessionName, _personInformant.Object)
			{
				MediaFiles = new ComponentFile[] { annotationFile.Object }
			};
		}

		[Test]
		public void SearchSessions_QueryMatchesAnnotationTranscriptionText_ReturnsSessionId()
		{
			var matchingSession = CreateSessionWithAnnotationText("Delta", "the needle is hidden in the transcript");
			var nonMatchingSession = CreateSessionWithAnnotationText("Epsilon", "nothing relevant here");

			var search = new TestableMetadataSearch(new[] { matchingSession, nonMatchingSession });

			var results = search.SearchSessions("needle").ToList();

			Assert.That(results, Is.EquivalentTo(new[] { matchingSession.Id }));
		}
	}
}
