using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Palaso.IO;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SilTools;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class SubTitleExporterTests
	{
		private TemporaryFolder _testFolder;

		[SetUp]
		public void TestSetup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;

//			_testFolder = new TemporaryFolder("Saymore.SubTitleExporterTests");
//			PortableSettingsProvider.SettingsFileFolder = _testFolder.Path;
		}

		[Test]
		public void Export_NoSegments_MakesEmptyFile()
		{
			using (var temp = TempFile.CreateAndGetPathButDontMakeTheFile())
			{
				SRTFormatSubTitleExporter.Export(temp.Path, new TextTier(TextTier.ElanTranscriptionTierId));
				Assert.IsTrue(File.Exists(temp.Path));
				Assert.AreEqual(0,File.ReadAllText(temp.Path).Length);
			}
		}

		[Test]
		public void Export_FileLocked_Throws()
		{
			using (var temp = TempFile.CreateAndGetPathButDontMakeTheFile())
			using (File.OpenWrite(temp.Path))
			{
				Assert.Throws<IOException>(() =>
											{
												SRTFormatSubTitleExporter.Export(temp.Path, CreateTier());
											});
			}
		}


		[Test]
		public void Export_Nominal_WritesExpectedContents()
		{
			using (var temp = TempFile.CreateAndGetPathButDontMakeTheFile())
			{
				SRTFormatSubTitleExporter.Export(temp.Path, CreateTier());
				Debug.WriteLine(File.ReadAllText(temp.Path));
				var lines = GetLines(temp).ToArray();
				Assert.AreEqual((3*4)-1, lines.Count());
				Assert.AreEqual("1", lines[0]);
				Assert.AreEqual("00:00:00,00 --> 00:00:01,00", lines[1]);
				Assert.AreEqual("one", lines[2]);
				Assert.AreEqual("", lines[3]);  //blank line
				Assert.AreEqual("2", lines[4]);
				Assert.AreEqual("00:00:01,45 --> 00:00:02,67", lines[5]);
				Assert.AreEqual("two", lines[6]);
				Assert.AreEqual("", lines[7]);  //blank line
			}
		}

		private static IEnumerable<string> GetLines(TempFile temp)
		{
			var lines = File.ReadAllText(temp.Path).Trim().Split(new char[] {'\n'});
			return lines.Select(line => line.Trim());
		}

		private TextTier CreateTier()
		{
			var tier = new TextTier(TextTier.ElanTranscriptionTierId);
			tier.Segments.Add(new Segment(tier){Start=0, End=1,Text="one"});
			tier.Segments.Add(new Segment(tier) { Start = (float) 1.456, End = (float) 2.6789, Text = "two" });
			tier.Segments.Add(new Segment(tier) { Start = 2, End = 3, Text = "three" });

			return tier;
		}
	}
}
