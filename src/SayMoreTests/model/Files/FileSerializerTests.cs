using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using System.Linq;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace SayMoreTests.model.Files
{
	public class FileSerializerTests
	{
		private TemporaryFolder _parentFolder;
		private List<FieldValue> _fields;
		private FileSerializer _serializer;

		[SetUp]
		public void Setup()
		{
			_serializer = new FileSerializer();
			_parentFolder = new TemporaryFolder("fileTypeTest");
			_fields = new List<FieldValue>();

		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}


		[Test,ExpectedException(typeof(FileNotFoundException))]
		public void Load_FileDoesNotExist_Throws()
		{
			LoadFromStandardPlace();
		}


		[Test, ExpectedException(typeof(DirectoryNotFoundException))]
		public void Save_CannotCreateFile_Throws()
		{
			_serializer.Save(_fields, _parentFolder.Combine("notthere", "test.txt"), "x");
		}


		[Test]
		public void SaveThenLoad_NoFields_RoundTripped()
		{
			SaveToStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(0, _fields.Count);
		}

		[Test]
		public void SaveThenLoad_TwoStrings_RoundTripped()
		{
			var valueA = new FieldValue("a", "string", "aaa");
			_fields.Add(valueA);
			var valueB = new FieldValue("b", "string", "bbb");
			_fields.Add(valueB);

			DoRoundTrip();
			Assert.AreEqual(2, _fields.Count);
			Assert.IsTrue(_fields.Any(f => f.Equals(valueA)));
			Assert.IsTrue(_fields.Any(f => f.Equals(valueB)));
		}


		[Test]
		public void SaveThenLoad_StringsWithNewLines_RoundTripped()
		{
			var valueA = new FieldValue("a", "string", "aaa" + Environment.NewLine + "second line");
			_fields.Add(valueA);

			DoRoundTrip();
			Assert.AreEqual(1, _fields.Count);
			Assert.AreEqual(valueA,_fields[0]);
		}

		[Test]
		public void Load_LoadingMultipleTimes_DoesNotIntroduceDuplicates()
		{
			_fields.Add(new FieldValue("a", "string", "aaa"));

			SaveToStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			LoadFromStandardPlace();
			Assert.AreEqual(1, _fields.Count);
		}


		[Test]
		public void SaveThenLoad_StringWithXmlSymbols_RoundTripped()
		{
			_fields.Add(new FieldValue("a", "string", "<mess me up"));
			DoRoundTrip();
			Assert.AreEqual("<mess me up", _fields.First().Value);
		}


		private void DoRoundTrip()
		{
			SaveToStandardPlace();
			_fields.Clear();
			LoadFromStandardPlace();
		}
		private void SaveToStandardPlace()
		{
			_serializer.Save(_fields, _parentFolder.Combine("test.txt"), "x");
		}

		private void LoadFromStandardPlace()
		{
			_serializer.Load(_fields, _parentFolder.Combine("test.txt"), "x");
		}
	}
}
