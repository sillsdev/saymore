using System.Linq;
using NUnit.Framework;
using SayMore.Model.Fields;

namespace SayMoreTests.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FieldInstanceTests
	{
		private FieldInstance _field;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_field = new FieldInstance("testField");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasMultipleValues_WhenValueIsEmptyString_False()
		{
			_field.Value = string.Empty;
			Assert.IsFalse(_field.GetHasMultipleValues());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasMultipleValues_WhenValueIsNull_False()
		{
			_field.Value = null;
			Assert.IsFalse(_field.GetHasMultipleValues());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasMultipleValues_WhenSingleValue_False()
		{
			_field.Value = "Ford";
			Assert.IsFalse(_field.GetHasMultipleValues());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasMultipleValues_WhenSingleValueIncludesDelimiters_False()
		{
			_field.Value = "Ford; ; ; ;;";
			Assert.IsFalse(_field.GetHasMultipleValues());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasMultipleValues_WhenMultipleValues_True()
		{
			_field.Value = "Ford; Chevy; GM";
			Assert.IsTrue(_field.GetHasMultipleValues());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValuesFromText_WhenValueIsEmptyString_ReturnsEmptyList()
		{
			Assert.AreEqual(0, FieldInstance.GetMultipleValuesFromText(string.Empty).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValuesFromText_WhenValueIsNull_ReturnsEmptyList()
		{
			Assert.AreEqual(0, FieldInstance.GetMultipleValuesFromText(null).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValuesFromText_WhenSingleValue_ReturnsListOfOne()
		{
			var list = FieldInstance.GetMultipleValuesFromText("Ford").ToList();
			Assert.AreEqual(1, list.Count);
			Assert.IsTrue(list.Contains("Ford"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValuesFromText_WhenSingleValueIncludesDelimiters_ReturnsListOfOne()
		{
			var list = FieldInstance.GetMultipleValuesFromText(" ; ;Ford ; ;;").ToList();
			Assert.AreEqual(1, list.Count);
			Assert.IsTrue(list.Contains("Ford"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValuesFromText_WhenMultipleValues_ReturnsThem()
		{
			var list = FieldInstance.GetMultipleValuesFromText("Ford ; ;Chevy ; Pontiac;;").ToList();
			Assert.AreEqual(3, list.Count);
			Assert.IsTrue(list.Contains("Ford"));
			Assert.IsTrue(list.Contains("Chevy"));
			Assert.IsTrue(list.Contains("Pontiac"));
		}

		[Test]
		public void GetTextFromMultipleValues_EmptyList_ReturnsEmptyString()
		{
			Assert.AreEqual(string.Empty, FieldInstance.GetTextFromMultipleValues(new string[] { }));
		}

		[Test]
		public void GetTextFromMultipleValues_ListOfOne_ReturnsWithoutDelimiter()
		{
			Assert.AreEqual("ferrari", FieldInstance.GetTextFromMultipleValues(new[] { "ferrari" }));
		}

		[Test]
		public void GetTextFromMultipleValues_MultipleItems_ReturnsThemDelimited()
		{
			Assert.AreEqual("ferrari; Lotus; Jaguar",
				FieldInstance.GetTextFromMultipleValues(new[] { "ferrari", "Lotus", "Jaguar" }));
		}

		[Test]
		public void GetTextFromMultipleValues_ListIncludesEmptyItem_ReturnExcludesEmptyItem()
		{
			Assert.AreEqual("ferrari; Lotus",
				FieldInstance.GetTextFromMultipleValues(new[] { "ferrari", "Lotus", "  " }));
		}

		[Test]
		public void GetTextFromMultipleValues_ListIncludesNull_ReturnExcludesNull()
		{
			Assert.AreEqual("ferrari; Jaguar",
				FieldInstance.GetTextFromMultipleValues(new[] { "ferrari", null, "Jaguar" }));
		}
	}
}
