using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using Palaso.UI.WindowsForms;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class MediaComponentEditor : EditorBase
	{
		protected FieldsValuesGrid _grid;
		protected FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public MediaComponentEditor()
		{
			var moreReliableDesignMode = (DesignMode || GetService(typeof(IDesignerHost)) != null) ||
				(LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			if (!moreReliableDesignMode)
				throw new NotImplementedException("Parameterless constructor should not be used.");
		}

		/// ------------------------------------------------------------------------------------
		public MediaComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeGrid(autoCompleteProvider, fieldGatherer);
			_toolStrip.Renderer = new NoToolStripBorderRenderer();
			_buttonPresets.DropDownItems.Add("HIDE");
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitializeGrid(AutoCompleteValueGatherer autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer, key => !"notes contributions".Contains(key));

			_grid = new FieldsValuesGrid(_gridViewModel, "MediaComponentEditor._grid");
			_grid.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(_grid, 0, 1);
			_grid.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			_gridViewModel.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePresetsDropDownOpening(object sender, EventArgs e)
		{
			for (int i = _buttonPresets.DropDownItems.Count - 1; i > 0; i--)
				_buttonPresets.DropDownItems.RemoveAt(i);

			_buttonPresets.DropDownItems[0].Visible = false;

			foreach (KeyValuePair<string, Dictionary<string, string>> pair in _file.GetPresetChoices())
			{
				// Copy to avoid the dreaded "access to modified closure"
				KeyValuePair<string, Dictionary<string, string>> valuePair = pair;
				_buttonPresets.DropDownItems.Add(pair.Key, null, (obj, sendr) => UsePreset(valuePair.Value));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePresetsDropDownClosed(object sender, EventArgs e)
		{
			_buttonPresets.DropDownItems[0].Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UsePreset(IDictionary<string, string> preset)
		{
			_file.UsePreset(preset);
			_gridViewModel.SetComponentFile(_file);
			_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMoreInfoButtonClick(object sender, EventArgs e)
		{
			using (var dlg = new MediaFileMoreInfoDlg(_file.PathToAnnotatedFile))
				dlg.ShowDialog(this);
		}
	}
}
