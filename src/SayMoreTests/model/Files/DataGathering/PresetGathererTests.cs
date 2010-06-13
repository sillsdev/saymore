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
	//TODO: I (jh) didn't do a very good job on these

	public class PresetGathererTests
	{
		[Test]
		public void GetPresets_NoFiles_GetNoPresetsMessageOnly()
		{
			using (var f = new TemporaryFolder("testPresetGathererFolder"))
			{
				using (var gatherer = new PresetGatherer(f.Path, new FileType[] { },
					path => new PresetData(path, p => new Dictionary<string, string>())))
				{
					gatherer.Start();
//					while(gatherer.Status=="Working")
					{
						Thread.Sleep(1000);
					}
					Assert.AreEqual(1,gatherer.GetSuggestions().Count());
				}
			}
		}

		[Test]
		public void GetPresets_SomeFiles_NonEmptyList()
		{
			using (var f = new TemporaryFolder("testPresetGathererFolder"))
			{
				File.WriteAllText(f.Combine("test.wav"), @"blah blah");
				using (var gatherer = new PresetGatherer(f.Path, new FileType[] { new AudioFileType(null) },
					path => new PresetData(path, p =>
					{
						var dict =new Dictionary<string, string>();
						dict.Add("one","1");
						return dict;
					})))

				{
					gatherer.Start();
					//while(gatherer.Status=="Working")
					{
						Thread.Sleep(1000);
					}
					Assert.AreEqual(1, gatherer.GetSuggestions().Count());
				}
			}
		}

	}
}
