using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SIL.Sponge.Model;
using SIL.Sponge.Model.MetaData;

namespace SpongeTests.ModelTests
{
	[TestFixture]
	public class PresetProviderTests
	{
		[Test]
		public void GetSuggestions_NoOtherFilesInProject_EmptySuggestionList()
		{
			var p = CreateProvider("");
			Assert.AreEqual(0, p.GetSuggestions().Count());
		}

		[Test]
		public void GetSuggestions_TwoDistingctInThree_GetTwoSuggestions()
		{
			var p = CreateProvider("device=Zoom,rate=24", "device=Marantz, rate=16", "device=Zoom,rate=24");
			Assert.AreEqual(2, p.GetSuggestions().Count());
		}

		private static PresetProvider CreateProvider(params string[] set)
		{
			return PresetProvider.CreateFromTestArray(set);
		}

		[Test,Ignore("TODO")]
		public void GetSuggestions_NoFiles_NoSugestions()
		{

		}
		[Test, Ignore("TODO")]
		public void GetSuggestions_FilesButNoMetaData_NoSugestions()
		{

		}

		[Test, Ignore("TODO")]
		public void GetSuggestions_TwoFiles_TwoSets()
		{

		}

	}
}
