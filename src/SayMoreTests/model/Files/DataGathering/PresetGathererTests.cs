using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.Model.Files.DataGathering
{
	[TestFixture]
	[Timeout(5000)]//each gets no more than 5 seconds
	public class PresetGathererTests
	{
		private TemporaryFolder _folder;

		[SetUp]
		public void Setup()
		{
			var r = new Random();
			_folder = new TemporaryFolder("testPresetGathererFolder"+r.Next());
		}
		[TearDown]
		public void TearDown()
		{
			_folder.Dispose();
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetPresets_NoFiles_GetNoPresetsMessageOnly()
		{
			using (var gatherer = CreatePresetGatherer())
			{
				gatherer.Start();
				WaitUntilNotBusy(gatherer);
				Assert.AreEqual(1,gatherer.GetPresets().Count());
			}
		}


		[Test]
		[Category("SkipOnTeamCity")]
		public void Background_SidecarDataChanged_PresetChanged()
		{
			WriteTestWavWithSidecar(@"original");
			using (var processor = CreateProcessor())
			{
				using (processor.ExpectNewDataAvailable())
				{
					processor.Start();
					WaitUntilNotBusy(processor);
				}
				Assert.AreEqual("original", processor.FirstValueOfFirstPreset());
				using (processor.ExpectNewDataAvailable())
				{
					WriteOnlySidecar(@"changed");
				}
				Assert.AreEqual(1, processor.GetPresets().Count());
				Assert.AreEqual("changed", processor.FirstValueOfFirstPreset());
			}
		}

		private PresetGatherer CreatePresetGatherer()
		{
			return new PresetGatherer(_folder.Path,
				new FileType[] { new AudioFileType(() => null, () => null) },
				MakePresetFromContentsOfFile);
		}

		private void WaitUntilNotBusy(PresetGatherer gatherer)
		{
			while(gatherer.Busy)
			{
				Thread.Sleep(100);
			}
		}

		private PresetData MakePresetFromContentsOfFile(string path)
		{
			path += (!path.EndsWith(".meta") ? ".meta" : string.Empty);
			return new PresetData(path, p =>
									{
										var dict = new Dictionary<string, string>();
										//here, the preset value is always just simply the contents of the file
										//Of course, the real PresetData (which isn't the clas under test)
										//uses the sidecar file, not the media file itself.

										dict.Add("contents", File.ReadAllText(p));

										return dict;
									});
		}

		private string WriteTestWavWithSidecar(string contents)
		{
			var path = _folder.Combine("test.wav");
			File.WriteAllText(path, contents);
			File.WriteAllText(path+".meta", contents);
			return path;
		}

		private void WriteOnlySidecar(string contents)
		{
			File.WriteAllText(_folder.Combine("test.wav.meta"), contents);
		}

		private PresetGatherer CreateProcessor()
		{
			return new PresetGatherer(_folder.Path,
				new FileType[] { new AudioFileType(() => null, () => null) },
				MakePresetFromContentsOfFile);
		}
	}

	public static class PresetGathererExtensionsForTesting
	{
		public static ExpectedEvent ExpectNewDataAvailable(this PresetGatherer gatherer)
		{
			var x = new ExpectedEvent();
			gatherer.NewDataAvailable += x.Event;
			return x;
		}

		public static string FirstValueOfFirstPreset(this PresetGatherer gatherer)
		{
			return gatherer.GetPresets().First().Value.Values.First();
		}
	}
}
