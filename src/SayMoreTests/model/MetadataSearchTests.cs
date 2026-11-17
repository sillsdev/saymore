using System;
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
		/// <summary>Creates a minimal, file-backed DummySession whose metadata includes the
		/// given (untranslated) genre ID.</summary>
		/// ------------------------------------------------------------------------------------
		private DummySession CreateSessionWithGenre(string sessionName, string genreId)
		{
			var session = CreateSessionWithMatchingField(sessionName, SessionFileType.kGenreFieldName, genreId);

			// GetSearchableFieldIds only treats genre as searchable when FileType is a
			// SessionFileType, but the mocked meta file (built via ProjectElementComponentFile's
			// "for mocking only" parameterless constructor) never gets one. ComponentFile.FileType
			// has a protected setter, so reflection is needed to poke a real SessionFileType into
			// the mock from here.
			typeof(ComponentFile).GetProperty(nameof(ComponentFile.FileType))
				?.GetSetMethod(true)
				?.Invoke(session.MetaFile.Object, new object[]
				{
					new SessionFileType(new Lazy<Func<SessionBasicEditor.Factory>>(() => null),
						new Lazy<Func<StatusAndStagesEditor.Factory>>(() => null),
						new Lazy<Func<ContributorsEditor.Factory>>(() => null))
				});

			return session;
		}

		[Test]
		public void SearchSessions_QueryMatchesDisplayedGenreName_ReturnsSessionId()
		{
			LocalizationManager.StrictInitializationMode = false;
			var matchingSession = CreateSessionWithGenre("Zeta", "language_play");
			var nonMatchingSession = CreateSessionWithGenre("Eta", "singing");

			var search = new TestableMetadataSearch(new[] { matchingSession, nonMatchingSession });

			var results = search.SearchSessions("Language Play").ToList();

			Assert.That(results, Is.EquivalentTo(new[] { matchingSession.Id }));
		}

		[Test]
		public void SearchSessions_QueryMatchesStoredGenreId_ReturnsSessionId()
		{
			var matchingSession = CreateSessionWithGenre("Theta", "language_play");
			var nonMatchingSession = CreateSessionWithGenre("Iota", "singing");

			var search = new TestableMetadataSearch(new[] { matchingSession, nonMatchingSession });

			var results = search.SearchSessions("language_play").ToList();

			Assert.That(results, Is.EquivalentTo(new[] { matchingSession.Id }));
		}

		[Test]
		public void SearchSessions_QueryMatchesUnrelatedGenresDisplayedName_ReturnsEmpty()
		{
			LocalizationManager.StrictInitializationMode = false;
			var nonMatchingSession = CreateSessionWithGenre("Kappa", "narrative");

			var search = new TestableMetadataSearch(new[] { nonMatchingSession });

			// "Oratory" is a different genre's displayed name - unrelated to "narrative"'s
			// stored ID or its own displayed name ("Narrative").
			var results = search.SearchSessions("Oratory").ToList();

			Assert.That(results, Is.Empty);
		}

		[Test]
		public void SearchSessions_QuerySpansBoundaryBetweenGenreIdAndDisplayedName_ReturnsEmpty()
		{
			LocalizationManager.StrictInitializationMode = false;
			var nonMatchingSession = CreateSessionWithGenre("Lambda", "language_play");

			var search = new TestableMetadataSearch(new[] { nonMatchingSession });

			// Neither the stored ID ("language_play") nor the displayed name ("Language Play")
			// contains "lay Lang" - it only appears if the two are concatenated together.
			var results = search.SearchSessions("lay Lang").ToList();

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
