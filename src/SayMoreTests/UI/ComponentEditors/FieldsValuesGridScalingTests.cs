using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using NUnit.Framework;
using SIL.TestUtilities;
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
		private FieldsValuesGrid CreateGrid()
		{
			const string fileName = "test.txt";
			File.WriteAllText(_parentFolder.Combine(fileName), "hello");

			var fileTypes = new[] { FileType.Create("Text", ".txt"), new UnknownFileType(null, null) };

			var file = new ComponentFile(null, _parentFolder.Combine(fileName), fileTypes,
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);
			file.Save();

			_fieldGatherer = new FieldGatherer(_parentFolder.Path, fileTypes, path => null);
			var model = new FieldsValuesGridViewModel(file, null, _fieldGatherer, null);
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
	}
}
