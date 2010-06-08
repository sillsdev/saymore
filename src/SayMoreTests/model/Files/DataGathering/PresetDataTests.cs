using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.model.Files.DataGathering
{
	public  class PresetDataTests
	{
		[Test]
		public void MethodBeingTested_Situation_Result()
		{
			using (var file = new TempFile(@"
"))
			{
				var data = new PresetData(file.Path, null);
				Assert.AreEqual(2, data.Dictionary.Count);
			}
		}
	}
}
