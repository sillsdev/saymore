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
		/// single searchable field (one of the tags common to every file type, so the test
		/// doesn't depend on FileType).</summary>
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
			session.MetaFile.Object.MetaDataFieldValues.Add(new FieldInstance(fieldId, fieldValue));
			return session;
		}

		[Test]
		public void SearchSessions_QueryMatchesMultipleSessions_ReturnsAllMatchingSessionIds()
		{
			var session1 = CreateSessionWithMatchingField("Alpha", "name", "find the needle here");
			var session2 = CreateSessionWithMatchingField("Beta", "name", "another needle sighting");
			var nonMatchingSession = CreateSessionWithMatchingField("Gamma", "name", "nothing relevant");

			var search = new TestableMetadataSearch(new[] { session1, session2, nonMatchingSession });

			var results = search.SearchSessions("needle").ToList();

			Assert.That(results, Is.EquivalentTo(new[] { session1.Id, session2.Id }));
		}
	}
}
