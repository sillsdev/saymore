using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.model.Files.DataGathering
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
		public void GetPresets_NoFiles_GetNoPresetsMessageOnly()
		{
			using (var gatherer = CreatePresetGatherer())
			{
				gatherer.Start();
				WaitUntilNotBusy(gatherer);
				Assert.AreEqual(1,gatherer.GetPresets().Count());
			}
		}

		private PresetGatherer CreatePresetGatherer()
		{
			return new PresetGatherer(_folder.Path, new FileType[] { new AudioFileType() },
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
	}
}
