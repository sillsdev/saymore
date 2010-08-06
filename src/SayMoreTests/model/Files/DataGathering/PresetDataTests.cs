using System.Collections.Generic;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.Model.Files.DataGathering
{
	public  class PresetDataTests
	{
		[Test]
		public void TODO_THIS_NEEDS_A_NAME()
		{
				var data = new PresetData("twoItems", p =>
														{
															var dict = new Dictionary<string, string>();
															if (p == "oneItems")
															{
																dict.Add("Alpha", "a");
															}
															if (p == "twoItems")
															{
																dict.Add("one", "1");
																dict.Add("two", "2");
															}
															return dict;
														});
				Assert.AreEqual(2, data.Dictionary.Count);
		}
	}
}
