using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SayMore;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace Sponge2Tests
{
	public class ExportTests
	{
		[Test]
		public void GetFileString_EmptyList_DoesntCrash()
		{
			var exporter = new ExportCommand();
			exporter.GetFileString(new List<Event>().ToArray());
		}

		[Test]
		public void GetFileString_TwoItemsWithDifferentFields_HeaderCombinesFields()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldInstance>(new[]{
				new FieldInstance("one","string","uno"),
				new FieldInstance("two", "string", "dos")});
			var s2 = new List<FieldInstance>(new[]
											{
												new FieldInstance("two", "string", "dos"),
												new FieldInstance("three", "string", "tres")
											});
			var result = exporter.GetFileString(new[] { s1, s2 });
			Assert.AreEqual("one,two,three", result.Split('\n').First().TrimEnd());
		}

		[Test]
		public void GetFileString_TwoItemsWithDifferentFields_CorrectValueLines()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldInstance>(new[]{
				new FieldInstance("one","string","uno"),
				new FieldInstance("two", "string", "dos")});
			var s2 = new List<FieldInstance>(new[]
											{
												new FieldInstance("two", "string", "dos"),
												new FieldInstance("three", "string", "tres")
											});
			var result = exporter.GetFileString(new[] { s1, s2 });
			var lines = result.Split('\n').Select(l=>l.TrimEnd()).ToArray();
			Assert.AreEqual("uno,dos", lines[1]);
			Assert.AreEqual(",dos,tres", lines[2]);
		}

		[Test]
		public void GetFileString_KeysAndFieldsHaveCommas_ProperlyQuoted()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldInstance>(new[]{
				new FieldInstance("a,b","string","1,2")});
			var result = exporter.GetFileString(new[] { s1});
			var lines = result.Split('\n').Select(l => l.TrimEnd()).ToArray();
			Assert.AreEqual("\"a,b\"", lines[0]);
			Assert.AreEqual("\"1,2\"", lines[1]);
		}


		/// <summary>
		/// excel generally doesn't like newlines
		/// </summary>
		[Test]
		public void GetFileString_FieldHasNewLine_ChangedToSpaces()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldInstance>(new[]{
				new FieldInstance("a","string","1"+Environment.NewLine+"2")});
			var result = exporter.GetFileString(new[] { s1 });
			var lines = result.Split('\n').Select(l => l.TrimEnd()).ToArray();
		   Assert.AreEqual("1 2", lines[1]);
		}
	}
}
