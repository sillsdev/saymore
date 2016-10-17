using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.WritingSystems;
using SayMore.UI.ComponentEditors;
using SayMore.UI.ProjectWindow;
using SIL.Archiving.IMDI.Lists;

namespace SayMore.UI.Overview
{
	public partial class ProjectMetadataScreen : EditorBase, ISayMoreView, ISaveable
	{
		private IMDIItemList _countryList;

		public ProjectMetadataScreen()
		{
			Logger.WriteEvent("ProjectMetadataScreen constructor");

			InitializeComponent();

			// position the Description and Vernacular label correctly
			var rowHeight = _tableLayout.GetRowHeights()[0];
			_tableLayout.RowStyles[2].SizeType = SizeType.Absolute;
			_tableLayout.RowStyles[2].Height = rowHeight;
			_tableLayout.RowStyles[4].SizeType = SizeType.Absolute;
			_tableLayout.RowStyles[4].Height = rowHeight;

			// continent list
			var continentList = ListConstructor.GetClosedList(ListType.Continents, true, ListConstructor.RemoveUnknown.RemoveAll);
			_continent.DataSource = continentList;
			_continent.DisplayMember = "Text";
			_continent.ValueMember = "Value";
			SizeContinentComboBox(_continent);

			// Data-binding doesn't work correctly for country  because it is an open list.
			// Items populated in HandleStringsLocalized.
			_countryList = ListConstructor.GetList(ListType.Countries, false, Localize, ListConstructor.RemoveUnknown.RemoveAll);

			_linkHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/User_Interface/Tabs/About_This_Project_User_Interface_terms.htm");
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();

			if (_country == null || _countryList == null)
			{
				Load += (o, args) => ResetCountryList();
				return;
			}
			ResetCountryList();
		}

		/// ------------------------------------------------------------------------------------
		private void ResetCountryList()
		{
			//int iCountry = _country.SelectedIndex;
			var selectedCountry = _country.SelectedItem as IMDIListItem;

			//_country.Items.Clear();
			_country.DataSource = null;
			_countryList.Localize(Localize);
			_countryList.Sort();
			//_country.Items.AddRange(_countryList.Select(c => c.Text).Cast<object>().ToArray());

			_country.DataSource = _countryList;
			_country.DisplayMember = "Text";
			_country.ValueMember = "Value";

			if (selectedCountry != null)
				_country.SelectedItem = _countryList.FindByValue(selectedCountry.Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a localized version of the string for an IMDI list item
		/// </summary>
		/// <param name="listName">the list name</param>
		/// <param name="item">list item value (i.e., what gets stored in the meta-data if
		/// this item is chosen)</param>
		/// <param name="property">Which property </param>
		/// <param name="defaultValue"></param>
		/// 2) ;
		/// 3) "Definition" or "Text"; 4) default (English) value.
		private string Localize(string listName, string item, string property, string defaultValue)
		{
			// SP-844: List from Arbil contains "Holy Seat" rather than "Holy See"
			var value = LocalizationManager.GetDynamicString("SayMore",
				string.Format("CommonToMultipleViews.ListItems.{0}.{1}.{2}", listName, item, property),
				defaultValue);

			//if (value.StartsWith("Holy Seat")) value = value.Replace("Holy Seat", "Holy See");

			return value;
		}

		#region ISayMoreView Members

		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			throw new NotImplementedException();
		}

		public void ViewActivated(bool firstTime)
		{
			throw new NotImplementedException();
		}

		public void ViewDeactivated()
		{
			throw new NotImplementedException();
		}

		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			throw new NotImplementedException();
		}

		public Image Image
		{
			get { throw new NotImplementedException(); }
		}

		public ToolStripMenuItem MainMenuItem
		{
			get { throw new NotImplementedException(); }
		}

		public string NameForUsageReporting
		{
			get { throw new NotImplementedException(); }
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		private void _linkSelectVernacular_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			// REVIEW: Do we need Force3LetterCodes? Can't find it in new implementation.
			// using (var dialog = new LanguageLookupDialog { Force3LetterCodes = true, ShowDesiredLanguageNameField = true })
			using (var dialog = new LanguageLookupDialog { IsDesiredLanguageNameFieldVisible = true })
			{
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK && dialog.SelectedLanguage != null)
					_labelSelectedVernacular.Text = dialog.SelectedLanguage.LanguageTag + @": " + dialog.DesiredLanguageName; // REVIEW: This line was modified to get it to build
			}

		}

		/// ------------------------------------------------------------------------------------
		private void ProjectMetadataScreen_Leave(object sender, EventArgs e)
		{
			Save();
		}

		/// ------------------------------------------------------------------------------------
		private void ProjectMetadataScreen_Load(object sender, EventArgs e)
		{
			// show values from project file
			var project = Program.CurrentProject;

			if (project == null) return;

			_projectTitle.Text = project.Title;
			_fundingProjectTitle.Text = project.FundingProjectTitle;

			// SP-815: Line breaks are not being displayed after reopening project
			if (project.ProjectDescription == null) project.ProjectDescription = string.Empty;
			_description.Text = project.ProjectDescription.Replace("\n", Environment.NewLine);

			_labelSelectedVernacular.Text = project.VernacularISO3CodeAndName;
			_location.Text = project.Location;
			_region.Text = project.Region;
			int iCountry = -1;
			for (int i = 0; i < _countryList.Count; i++)
			{
				if (_countryList[i].Value == project.Country)
				{
					iCountry = i;
					break;
				}
			}
			if (iCountry >= 0)
				_country.SelectedIndex = iCountry;
			else
				_country.Text = project.Country;

			foreach (var item in _continent.Items.Cast<object>().Where(i => i.ToString() == project.Continent))
				_continent.SelectedItem = item;

			_contactPerson.Text = project.ContactPerson;
			_dateAvailable.SetValue(project.DateAvailable);
			_rightsHolder.Text = project.RightsHolder;
			_depositor.Text = project.Depositor;

			foreach (Control control in Controls)
			{
				control.Validated += delegate { Save(); };
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SizeContinentComboBox(ComboBox comboBox)
		{
			var maxWidth = 0;
			foreach (var item in comboBox.Items)
			{
				var itmWidth = TextRenderer.MeasureText(item.ToString(), comboBox.Font).Width;
				if (itmWidth > maxWidth)
					maxWidth = itmWidth;
			}

			comboBox.Width = maxWidth + 30;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>The country value to persist in metadata</summary>
		/// ------------------------------------------------------------------------------------
		private string SelectedCountry
		{
			get
			{
				if (_country.SelectedIndex >= 0)
					return _countryList[_country.SelectedIndex].Value;
				return _country.Text;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			// check for changes
			var project = Program.CurrentProject;
			string country = SelectedCountry;
			var changed = (_projectTitle.Text != project.Title ||
				_fundingProjectTitle.Text != project.FundingProjectTitle ||
				_description.Text != project.ProjectDescription ||
				_labelSelectedVernacular.Text != project.VernacularISO3CodeAndName ||
				_location.Text != project.Location ||
				_region.Text != project.Region ||
				country != project.Country ||
				_continent.Text != project.Continent ||
				_contactPerson.Text != project.ContactPerson ||
				_dateAvailable.Text != project.DateAvailable ||
				_rightsHolder.Text != project.RightsHolder ||
				_depositor.Text != project.Depositor
				);

			if (!changed) return;

			// save changes
			project.Title = _projectTitle.Text;
			project.FundingProjectTitle = _fundingProjectTitle.Text;
			project.ProjectDescription = _description.Text;
			project.VernacularISO3CodeAndName = _labelSelectedVernacular.Text;
			project.Location = _location.Text;
			project.Region = _region.Text;
			project.Country = country;
			project.Continent = _continent.Text;
			project.ContactPerson = _contactPerson.Text;

			project.DateAvailable = _dateAvailable.GetISO8601DateValueOrNull();
			project.RightsHolder = _rightsHolder.Text;
			project.Depositor = _depositor.Text;
			project.Save();
		}

		private void ProjectMetadataScreen_VisibleChanged(object sender, EventArgs e)
		{
			// In Project page, Country text should not be selected.
			if (_country.SelectionLength > 0)
				_country.SelectionLength = 0;
		}
	}
}
