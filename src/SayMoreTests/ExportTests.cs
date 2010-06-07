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
			exporter.GetFileString(new List<Session>().ToArray());
		}

		[Test]
		public void GetFileString_TwoItemsWithDifferentFields_HeaderCombinesFields()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldValue>(new[]{
				new FieldValue("one","string","uno"),
				new FieldValue("two", "string", "dos")});
			var s2 = new List<FieldValue>(new[]
											{
												new FieldValue("two", "string", "dos"),
												new FieldValue("three", "string", "tres")
											});
			var result = exporter.GetFileString(new[] { s1, s2 });
			Assert.AreEqual("one,two,three", result.Split('\n').First().TrimEnd());
		}

		[Test]
		public void GetFileString_TwoItemsWithDifferentFields_CorrectValueLines()
		{
			var exporter = new ExportCommand();
			var s1 = new List<FieldValue>(new[]{
				new FieldValue("one","string","uno"),
				new FieldValue("two", "string", "dos")});
			var s2 = new List<FieldValue>(new[]
											{
												new FieldValue("two", "string", "dos"),
												new FieldValue("three", "string", "tres")
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
			var s1 = new List<FieldValue>(new[]{
				new FieldValue("a,b","string","1,2")});
			var result = exporter.GetFileString(new[] { s1});
			var lines = result.Split('\n').Select(l => l.TrimEnd()).ToArray();
			Assert.AreEqual("\"a,b\"", lines[0]);
			Assert.AreEqual("\"1,2\"", lines[1]);
		}
	}
}
