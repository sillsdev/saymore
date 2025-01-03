using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using SayMore.Model;
using SayMore.Model.Files;
using static SayMore.Model.Files.PersonFileType;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class MetadataMigratorTests
	{
		private class SimpleTestMetadataCollection : IStringValueCollection
		{
			private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

			public SimpleTestMetadataCollection(string personId)
			{
				Id = personId;
			}

			public string Id { get; }

			public string GetStringValue(string key, string defaultValue)
			{
				return _data.TryGetValue(key, out var value) ? value : defaultValue;
			}

			public bool TrySetStringValue(string key, string newValue)
			{
				_data[key] = newValue;
				return true;
			}

			public string SetStringValue(string key, string newValue)
			{
				_data[key] = newValue;
				return newValue;
			}
		}

		[Test]
		public void MigrateAmbiguousLanguages_NoAmbiguousLanguages_ReturnsNotNeeded()
		{
			Assert.That(LanguageHelper.IsAmbiguous("en"), Is.False, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("es:Spanish"), Is.False, "Setup problem");

			var metadata = new SimpleTestMetadataCollection("Miguel");
			metadata.SetStringValue(kPrimaryLanguage, "en");
			metadata.SetStringValue(kMothersLanguage, "en");
			metadata.SetStringValue(GetOtherLanguageKey(0), "es:Spanish");

			var migrator = new MetadataMigrator(() => throw new NotImplementedException(
					"This test should not have needed a disambiguator."));
			Assert.That(migrator.MigrateAmbiguousLanguages(metadata),
				Is.EqualTo(MetadataMigrator.Result.NotNeeded));
		}

		[Test]
		public void MigrateAmbiguousLanguages_AllAmbiguousLanguages_SavesDisambiguatedValuesAndReturnsMigrated()
		{
			Assert.That(LanguageHelper.IsAmbiguous("eng"), Is.True, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("aka"), Is.True, "Setup problem");

			var metadata = new SimpleTestMetadataCollection("Bucky");
			metadata.SetStringValue(kPrimaryLanguage, "eng"); // English (eng) or Greek Sign Language (gss)?
			metadata.SetStringValue(kMothersLanguage, "eng");
			metadata.SetStringValue(GetOtherLanguageKey(0), "aka"); // Akan (aka) of Aka (soh)
			metadata.SetStringValue(kFathersLanguage, "aka");

			var disambiguator = new Mock<ILanguageDisambiguator>(MockBehavior.Strict);
			disambiguator.Setup(d => d.IsOkayToDisambiguate()).Returns(true);
			disambiguator.Setup(d => d.Disambiguate("eng", "Bucky", true, false, true)).Returns("en:English");
			disambiguator.Setup(d => d.Disambiguate("aka", "Bucky", false, true, false)).Returns("soh:Aka");

			var migrator = new MetadataMigrator(() => disambiguator.Object);
			Assert.That(migrator.MigrateAmbiguousLanguages(metadata),
				Is.EqualTo(MetadataMigrator.Result.Migrated));
			Assert.That(metadata.GetStringValue(kPrimaryLanguage, "frog lips"),
				Is.EqualTo("en:English"));
			Assert.That(metadata.GetStringValue(kMothersLanguage, "frog lips"),
				Is.EqualTo("en:English"));
			Assert.That(metadata.GetStringValue(GetOtherLanguageKey(0), "monkey legs"),
				Is.EqualTo("soh:Aka"));
			Assert.That(metadata.GetStringValue(kFathersLanguage, "monkey legs"),
				Is.EqualTo("soh:Aka"));
			disambiguator.Verify();
		}

		[Test]
		public void MigrateAmbiguousLanguages_SomeAmbiguousLanguages_SavesDisambiguatedValuesAndReturnsMigrated()
		{
			Assert.That(LanguageHelper.IsAmbiguous("eng"), Is.True, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("aka"), Is.True, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("iii"), Is.False, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("Walloon"), Is.False, "Setup problem");

			var metadata = new SimpleTestMetadataCollection("Sam");
			metadata.SetStringValue(kPrimaryLanguage, "eng"); // English (eng) or Greek Sign Language (gss)?
			metadata.SetStringValue(kMothersLanguage, "eng");
			metadata.SetStringValue(GetOtherLanguageKey(0), "aka"); // Akan (aka) of Aka (soh)
			metadata.SetStringValue(kFathersLanguage, "aka");
			metadata.SetStringValue(GetOtherLanguageKey(1), "iii"); // Unambiguous
			metadata.SetStringValue(GetOtherLanguageKey(2), "Walloon"); // Unambiguous

			var disambiguator = new Mock<ILanguageDisambiguator>(MockBehavior.Strict);
			disambiguator.Setup(d => d.IsOkayToDisambiguate()).Returns(true);
			disambiguator.Setup(d => d.Disambiguate("eng", "Sam", true, false, true)).Returns("en:English");
			disambiguator.Setup(d => d.Disambiguate("aka", "Sam", false, true, false)).Returns("soh:Aka");

			var migrator = new MetadataMigrator(() => disambiguator.Object);
			Assert.That(migrator.MigrateAmbiguousLanguages(metadata),
				Is.EqualTo(MetadataMigrator.Result.Migrated));
			Assert.That(metadata.GetStringValue(kPrimaryLanguage, "frog lips"),
				Is.EqualTo("en:English"));
			Assert.That(metadata.GetStringValue(kMothersLanguage, "frog lips"),
				Is.EqualTo("en:English"));
			Assert.That(metadata.GetStringValue(GetOtherLanguageKey(0), "monkey legs"),
				Is.EqualTo("soh:Aka"));
			Assert.That(metadata.GetStringValue(kFathersLanguage, "monkey legs"),
				Is.EqualTo("soh:Aka"));
			Assert.That(metadata.GetStringValue(GetOtherLanguageKey(1), "monkey legs"),
				Is.EqualTo("iii"));
			Assert.That(metadata.GetStringValue(GetOtherLanguageKey(2), "monkey legs"),
				Is.EqualTo("Walloon"));
			disambiguator.Verify();
		}

		[Test]
		public void MigrateAmbiguousLanguages_SomeAmbiguousLanguagesMultiplePeople_SavesDisambiguatedValuesAndReturnsMigrated()
		{
			Assert.That(LanguageHelper.IsAmbiguous("eng"), Is.True, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("aka"), Is.True, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("iii"), Is.False, "Setup problem");
			Assert.That(LanguageHelper.IsAmbiguous("Walloon"), Is.False, "Setup problem");

			var metadataSam = new SimpleTestMetadataCollection("Sam");
			metadataSam.SetStringValue(kPrimaryLanguage, "eng"); // English (eng) or Greek Sign Language (gss)?
			metadataSam.SetStringValue(kMothersLanguage, "eng");
			metadataSam.SetStringValue(kFathersLanguage, "eng");
			metadataSam.SetStringValue(GetOtherLanguageKey(0), "Walloon"); // Unambiguous

			var metadataPam = new SimpleTestMetadataCollection("Pam");
			metadataPam.SetStringValue(kPrimaryLanguage, "iii"); // Unambiguous
			metadataPam.SetStringValue(GetOtherLanguageKey(0), "aka");
			metadataPam.SetStringValue(GetOtherLanguageKey(1), "eng");
			metadataPam.SetStringValue(kMothersLanguage, "eng");

			var disambiguator = new Mock<ILanguageDisambiguator>(MockBehavior.Strict);
			disambiguator.Setup(d => d.IsOkayToDisambiguate()).Returns(true);
			disambiguator.Setup(d => d.Disambiguate("eng", "Sam", true, true, true)).Returns("en:English");
			disambiguator.Setup(d => d.Disambiguate("aka", "Pam", false, false, false)).Returns("soh:Aka");
			// Should not be asked to re-disambiguate "eng" for Pam because we assume it should be
			// the same as for Sam.

			var migrator = new MetadataMigrator(() => disambiguator.Object);
			Assert.That(migrator.MigrateAmbiguousLanguages(metadataSam),
				Is.EqualTo(MetadataMigrator.Result.Migrated));
			Assert.That(migrator.MigrateAmbiguousLanguages(metadataPam),
				Is.EqualTo(MetadataMigrator.Result.Migrated));
			Assert.That(metadataSam.GetStringValue(kPrimaryLanguage, "frog lips"),
				Is.EqualTo("en:English"));
			Assert.That(metadataSam.GetStringValue(GetOtherLanguageKey(0), "monkey legs"),
				Is.EqualTo("Walloon"));
			Assert.That(metadataPam.GetStringValue(kPrimaryLanguage, "turkey bones"),
				Is.EqualTo("iii"));
			Assert.That(metadataPam.GetStringValue(GetOtherLanguageKey(0), "bonus quiz"),
				Is.EqualTo("soh:Aka"));
			Assert.That(metadataPam.GetStringValue(GetOtherLanguageKey(1), "flap jacks"),
				Is.EqualTo("en:English"));
			Assert.That(metadataPam.GetStringValue(kMothersLanguage, "in the box"),
				Is.EqualTo("en:English"));
			disambiguator.Verify();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void MigrateAmbiguousLanguages_AmbiguousLanguagesCancelled_SavesDisambiguatedValuesAndReturnsCancelled(bool isOkayToDisambiguate)
		{
			Assert.That(LanguageHelper.IsAmbiguous("eng"), Is.True, "Setup problem");

			var metadata = new SimpleTestMetadataCollection("Walter");
			metadata.SetStringValue(kPrimaryLanguage, "eng"); // English (eng) or Greek Sign Language (gss)?

			var disambiguator = new Mock<ILanguageDisambiguator>(MockBehavior.Strict);
			disambiguator.Setup(d => d.IsOkayToDisambiguate()).Returns(isOkayToDisambiguate);
			if (isOkayToDisambiguate)
				disambiguator.Setup(d => d.Disambiguate("eng", "Walter", true, false, false)).Returns((string)null);
			
			var migrator = new MetadataMigrator(() => disambiguator.Object);
			Assert.That(migrator.MigrateAmbiguousLanguages(metadata),
				Is.EqualTo(MetadataMigrator.Result.Cancelled));
			Assert.That(metadata.GetStringValue(kPrimaryLanguage, "frog lips"),
				Is.EqualTo("eng"));
			disambiguator.Verify();
		}
	}
}
