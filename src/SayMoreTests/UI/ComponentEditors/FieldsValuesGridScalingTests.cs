using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.Utilities;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// FieldsValuesGrid implements its own IUiScalable.ApplyScale rather than relying on
	/// UITextScaler's generic DataGridView case (see FieldsValuesGrid.ApplyScale's doc comment),
	/// so the generic case's column-header-font handling doesn't apply to it automatically --
	/// this grid has to do that itself.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class FieldsValuesGridScalingTests
	{
		private TemporaryFolder _parentFolder;
		private FieldGatherer _fieldGatherer;

		/// ------------------------------------------------------------------------------------
		[OneTimeSetUp]
		public void FixtureSetup()
		{
			LocalizationManager.StrictInitializationMode = false;
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("fieldsValuesGridScalingTest");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_fieldGatherer?.Dispose();
			_fieldGatherer = null;
			_parentFolder.Dispose();
			_parentFolder = null;
			Settings.Default.UITextScale = 100;
			UITextScaler.NotifyScaleChanged();
		}

		/// ------------------------------------------------------------------------------------
		private FieldsValuesGrid CreateGrid() => CreateGrid(out _);

		/// ------------------------------------------------------------------------------------
		private FieldsValuesGrid CreateGrid(out FieldsValuesGridViewModel model)
		{
			const string fileName = "test.txt";
			File.WriteAllText(_parentFolder.Combine(fileName), "hello");

			var fileTypes = new[] { FileType.Create("Text", ".txt"), new UnknownFileType(null, null) };

			var file = new ComponentFile(null, _parentFolder.Combine(fileName), fileTypes,
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);
			file.Save();

			_fieldGatherer = new FieldGatherer(_parentFolder.Path, fileTypes, path => null);
			model = new FieldsValuesGridViewModel(file, null, _fieldGatherer, null);
			return new FieldsValuesGrid(model, "testGrid");
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsFieldsValuesGrid_ScalesColumnHeaderFont()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var grid = CreateGrid())
			{
				host.Controls.Add(grid);
				var headerSizeAt100 = grid.ColumnHeadersDefaultCellStyle.Font?.Size ?? grid.Font.Size;

				Settings.Default.UITextScale = 200;
				UITextScaler.NotifyScaleChanged();
				host.ScaleUIFont();

				Assert.IsNotNull(grid.ColumnHeadersDefaultCellStyle.Font,
					"ApplyScale must set an explicit header font so it no longer silently falls back to the grid's (now-scaled) Font.");
				Assert.Greater(grid.ColumnHeadersDefaultCellStyle.Font.Size, headerSizeAt100,
					"FieldsValuesGrid.ApplyScale must rescale the column header font (the 'Field'/'Value' " +
					"headings), not just cell content -- it currently only rescales Font and _factoryFieldFont.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// EnableHeadersVisualStyles defaults to true, which makes the OS theme paint every
		/// header cell and ignore ColumnHeadersDefaultCellStyle.Font entirely -- so the header
		/// font assigned above never actually renders unless this is turned off. It must stay
		/// on at 100% scale so upgrading users see the native themed header they always had.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsFieldsValuesGrid_DisablesHeaderVisualStylesOnlyWhenScaled()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var grid = CreateGrid())
			{
				host.Controls.Add(grid);
				Assert.IsTrue(grid.EnableHeadersVisualStyles,
					"At the default 100% scale, headers must keep the OS-themed look.");

				Settings.Default.UITextScale = 200;
				UITextScaler.NotifyScaleChanged();
				host.ScaleUIFont();

				Assert.IsFalse(grid.EnableHeadersVisualStyles,
					"Visual-style theming ignores ColumnHeadersDefaultCellStyle.Font entirely, so it " +
					"must be turned off whenever the UI text scale isn't 100%, or the scaled header " +
					"font never actually renders.");

				Settings.Default.UITextScale = 100;
				UITextScaler.NotifyScaleChanged();
				host.ScaleUIFont();

				Assert.IsTrue(grid.EnableHeadersVisualStyles,
					"Returning to 100% must restore the native themed header look.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Working-language cell fonts are applied per cell in OnCellFormatting, which no
		/// Control-recursive scaling walk reaches, so the grid itself keeps them tracking the
		/// current UI text scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void FieldsValuesGrid_WorkingLanguageCellFont_RescalesWithUIScale()
		{
			Settings.Default.UITextScale = 100;
			UITextScaler.NotifyScaleChanged();

			using (var workingLanguageFont = new Font("Times New Roman", 14f))
			using (var grid = CreateGrid(out var model))
			{
				model.RowData.Add(new FieldInstance("myCustomField", "free text"));
				grid.RowCount = model.RowData.Count + (grid.AllowUserToAddRows ? 1 : 0);
				grid.SetWorkingLanguageFont(workingLanguageFont);

				var fontAt100 = GetFreeTextValueCellFont(grid);
				Assert.AreEqual(workingLanguageFont.FontFamily, fontAt100.FontFamily);
				Assert.AreEqual(14f, fontAt100.Size, 0.01f);

				Settings.Default.UITextScale = 200;
				UITextScaler.NotifyScaleChanged();

				var fontAt200 = GetFreeTextValueCellFont(grid);
				Assert.AreEqual(workingLanguageFont.FontFamily, fontAt200.FontFamily,
					"Scaling must change only the working-language font's size, never its family.");
				Assert.AreEqual(28f, fontAt200.Size, 0.01f);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the font FieldsValuesGrid.OnCellFormatting selects for the free-text value
		/// cell in the first row, by triggering a real formatting pass for that cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static Font GetFreeTextValueCellFont(FieldsValuesGrid grid)
		{
			Font font = null;
			void Capture(object sender, DataGridViewCellFormattingEventArgs e)
			{
				if (e.ColumnIndex == 1 && e.RowIndex == 0)
					font = e.CellStyle.Font;
			}

			grid.CellFormatting += Capture;
			try
			{
				_ = grid[1, 0].FormattedValue;
			}
			finally
			{
				grid.CellFormatting -= Capture;
			}

			Assert.IsNotNull(font, "Expected a formatting pass for the first row's value cell.");
			return font;
		}
	}
}
