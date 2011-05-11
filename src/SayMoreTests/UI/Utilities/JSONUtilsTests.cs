using System.Collections.Generic;
using NUnit.Framework;
using SayMore.UI.Archiving;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class JSONUtilsTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeKeyValuePair_BothAreEmtpy_ReturnsNull()
		{
			Assert.IsNull(JSONUtils.MakeKeyValuePair("", ""));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeKeyValuePair_KeyIsNull_ReturnsNull()
		{
			Assert.IsNull(JSONUtils.MakeKeyValuePair(null, "blah"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeKeyValuePair_ValueIsNull_ReturnsNull()
		{
			Assert.IsNull(JSONUtils.MakeKeyValuePair("blah", null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeKeyValuePair_KeyAndValueAreGood_ReturnsCorrectString()
		{
			Assert.AreEqual("\"key\":\"value\"", JSONUtils.MakeKeyValuePair("key", "value"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeKeyValuePair_BracketValue_ReturnsBracketedValue()
		{
			Assert.AreEqual("[\"key\":\"value\"]", JSONUtils.MakeKeyValuePair("key", "value", true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeArrayFromValues_GoodValues_MakesArray()
		{
			var list = new List<string>();
			list.Add(JSONUtils.MakeKeyValuePair("corvette", "car"));
			list.Add(JSONUtils.MakeKeyValuePair("banana", "fruit"));
			list.Add(JSONUtils.MakeKeyValuePair("spot", "dog"));

			Assert.AreEqual("\"stuff\":{\"0\":{\"corvette\":\"car\"},\"1\":{\"banana\":\"fruit\"},\"2\":{\"spot\":\"dog\"}}",
				JSONUtils.MakeArrayFromValues("stuff", list));
		}
	}
}
