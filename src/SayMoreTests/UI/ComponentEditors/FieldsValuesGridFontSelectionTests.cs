using System.Drawing;
using NUnit.Framework;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Covers the per-cell font decisions FieldsValuesGrid.OnCellFormatting makes: fixed/
	/// localized app vocabulary (factory field names, dropdown values) gets the UI font;
	/// free-form user-typed content (custom field names, free-text values) gets the Working
	/// Language font. These are extracted as pure functions specifically so they're testable
	/// without needing a real ComponentFile-backed FieldsValuesGridViewModel.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FieldsValuesGridFontSelectionTests
	{
		private readonly Font _uiFont = new Font(FontFamily.GenericSansSerif, 10f);
		private readonly Font _workingLanguageFont = new Font(FontFamily.GenericSerif, 14f);

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldNameFont_FactoryField_ReturnsFactoryFieldFont()
		{
			var factoryFieldFont = new Font(_uiFont, FontStyle.Bold);
			Assert.AreSame(factoryFieldFont,
				FieldsValuesGrid.GetFieldNameFont(false, factoryFieldFont, _workingLanguageFont));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFieldNameFont_CustomField_ReturnsWorkingLanguageFont()
		{
			var factoryFieldFont = new Font(_uiFont, FontStyle.Bold);
			Assert.AreSame(_workingLanguageFont,
				FieldsValuesGrid.GetFieldNameFont(true, factoryFieldFont, _workingLanguageFont));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueFont_ComboBoxCell_ReturnsUIFont()
		{
			Assert.AreSame(_uiFont,
				FieldsValuesGrid.GetValueFont(true, _uiFont, _workingLanguageFont));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueFont_FreeFormCell_ReturnsWorkingLanguageFont()
		{
			Assert.AreSame(_workingLanguageFont,
				FieldsValuesGrid.GetValueFont(false, _uiFont, _workingLanguageFont));
		}
	}
}
