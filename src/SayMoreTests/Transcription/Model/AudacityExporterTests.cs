using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.IO;
using SayMore.Transcription.Model;
using SayMore.Transcription.Model.Exporters;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class AudactiyExporterTests
	{
		[SetUp]
		public void TestSetup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		[Test]
		public void Export_NoSegments_MakesEmptyFile()
		{
			using (var temp = TempFile.CreateAndGetPathButDontMakeTheFile())
			{
				AudacityExporter.Export(temp.Path, new TextTier(TextTier.ElanTranscriptionTierId));
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
												AudacityExporter.Export(temp.Path, CreateTier());
											});
			}
		}


		[Test]
		public void Export_Nominal_WritesExpectedContents()
		{
			using (var temp = TempFile.CreateAndGetPathButDontMakeTheFile())
			{
				AudacityExporter.Export(temp.Path, CreateTier());
				Debug.WriteLine(File.ReadAllText(temp.Path));
				var lines = GetLines(temp).ToArray();
				//Assert.AreEqual(3/*segments*/+1/*blank*/, lines.Count());
				Assert.AreEqual(3, lines.Count());
				Assert.AreEqual("0.000000\t1.000000\tone", lines[0]);
				Assert.AreEqual("1.456000\t2.679000\ttwo", lines[1]);//2.6790 instead of 2.6789 because somewhere else in saymore, the value gets rounded. Not and export issue.
				Assert.AreEqual("2.000000\t3.000000\tthree", lines[2]);
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
			tier.Segments.Add(new AnnotationSegment(tier){Start=0, End=1,Text="one"});
			tier.Segments.Add(new AnnotationSegment(tier) { Start = (float) 1.456, End = (float) 2.6789, Text = "two" });
			tier.Segments.Add(new AnnotationSegment(tier) { Start = 2, End = 3, Text = "three" });

			return tier;
		}
	}
}
