using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.WritingSystems;
using SayMore.UI.ComponentEditors;
using SayMore.UI.LowLevelControls;
using SayMore.UI.ProjectWindow;
using SIL.Archiving.IMDI.Lists;
using SIL.WritingSystems;
using static System.String;

namespace SayMore.UI.Overview
{
	public partial class ProjectMetadataScreen : EditorBase, ISayMoreView, ISaveable
	{
		private const string kLanguageTagAndNameSeparator = ": ";
		private string _fmtFontForWorkingLanguage;
		private readonly IMDIItemList _countryList;

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
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			base.HandleStringsLocalized(lm);

			if (lm != null && lm.Id != ApplicationContainer.kSayMoreLocalizationId)
				return;

			if (_linkSelectFontForWorkingLanguage == null)
			{
				Load += (o, args) =>
				{
					Debug.Assert(_linkSelectFontForWorkingLanguage != null);
					_fmtFontForWorkingLanguage = _linkSelectFontForWorkingLanguage.Text;
					UpdateWorkingLanguageFontDisplay();
				};
			}
			else
			{
				_fmtFontForWorkingLanguage = _linkSelectFontForWorkingLanguage.Text;
				UpdateWorkingLanguageFontDisplay();
			}

			if (_country == null || _countryList == null)
				Load += (o, args) => ResetCountryList();
			else
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
			var value = LocalizationManager.GetDynamicString("SayMore",
				$"CommonToMultipleViews.ListItems.{listName}.{item}.{property}",
				defaultValue);

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

		public Image Image => throw new NotImplementedException();

		public ToolStripMenuItem MainMenuItem => throw new NotImplementedException();

		public string NameForUsageReporting => throw new NotImplementedException();

		#endregion

		/// ------------------------------------------------------------------------------------
		private void ProjectMetadataScreen_LostFocus(object sender, EventArgs e)
		{
			Save();
		}

		/// ------------------------------------------------------------------------------------
		private void ProjectMetadataScreen_Load(object sender, EventArgs e)
		{
			// show values from project file
			var project = Program.CurrentProject;

			if (project == null)
				return;

			_projectTitle.Text = project.Title;
			_fundingProjectTitle.Text = project.FundingProjectTitle;

			// SP-815: Line breaks are not being displayed after reopening project
			if (project.ProjectDescription == null)
					project.ProjectDescription = Empty;
			_description.Text = project.ProjectDescription.Replace("\n", Environment.NewLine);

			_labelSelectedContentLanguage.Text = project.VernacularISO3CodeAndName;
			_labelSelectedWorkingLanguage.Text = project.AnalysisISO3CodeAndName;
			SetWorkingLanguageFont(project.WorkingLanguageFont);

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
			try
			{
				_dateAvailable.SetValue(project.DateAvailable);
			}
			catch (AmbiguousDateException dateError)
			{
				var msg = Format(LocalizationManager.GetString("ProjectView.AmbiguousDateNote",
						"An ambiguous date ({0}) was loaded, probably produced by a bug in an " +
						"old version of {1}. Please ensure the date was interpreted correctly.",
						"Param 1: date value as stored in project file; " +
						"Param 2: \"SayMore\" (product name). "),
					dateError.Message, ProductName);

				_errorProvider.SetError(_dateAvailable, msg);
			}

			_rightsHolder.Text = project.RightsHolder;
			_depositor.Text = project.Depositor;

			foreach (Control control in Controls)
			{
				control.Validated += delegate { Save(); };
			}
		}

		private void _dateAvailable_Validated(object sender, EventArgs e)
		{
			_errorProvider.SetError(_dateAvailable, null);
		}

		protected override void SetWorkingLanguageFont(Font font)
		{
			_projectTitle.Font = font;
			_description.Font = font;
			_contactPerson.Font = font;
			_location.Font = font;
			_region.Font = font;
			_fundingProjectTitle.Font = font;
			_rightsHolder.Font = font;
			_depositor.Font = font;
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
			var changed = _projectTitle.Text != project.Title ||
				_fundingProjectTitle.Text != project.FundingProjectTitle ||
				_description.Text != project.ProjectDescription ||
				_labelSelectedContentLanguage.Text != project.VernacularISO3CodeAndName ||
				_labelSelectedWorkingLanguage.Text != project.AnalysisISO3CodeAndName ||
				_location.Text != project.Location ||
				_region.Text != project.Region ||
				country != project.Country ||
				_continent.Text != project.Continent ||
				_contactPerson.Text != project.ContactPerson ||
				_dateAvailable.Text != project.DateAvailable ||
				_rightsHolder.Text != project.RightsHolder ||
				_depositor.Text != project.Depositor ||
				!_projectTitle.Font.Equals(project.WorkingLanguageFont);

			if (!changed)
				return;

			// save changes
			project.Title = _projectTitle.Text;
			project.FundingProjectTitle = _fundingProjectTitle.Text;
			project.ProjectDescription = _description.Text;
			project.VernacularISO3CodeAndName = _labelSelectedContentLanguage.Text;
			project.AnalysisISO3CodeAndName = _labelSelectedWorkingLanguage.Text;
			project.WorkingLanguageFont = _projectTitle.Font;
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

		private void _linkSelectWorkingLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			LanguageInfo currentLanguageInfo = null;
			if (!IsNullOrEmpty(_labelSelectedWorkingLanguage.Text))
			{
				var lookup = new LanguageLookup(true);
				var languageNameParts = _labelSelectedWorkingLanguage.Text
					.Split(new [] {kLanguageTagAndNameSeparator}, StringSplitOptions.None);
				currentLanguageInfo = lookup.GetLanguageFromCode(languageNameParts[0]);
			}

			// REVIEW: Do we need Force3LetterCodes? Can't find it in new implementation.
			using (var dialog = new LanguageLookupDialog())
			{
				dialog.SelectedLanguage = currentLanguageInfo;
				dialog.IsDesiredLanguageNameFieldVisible = true;
				if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedLanguage != null)
				{
					_labelSelectedWorkingLanguage.Text = dialog.SelectedLanguage.LanguageTag +
						kLanguageTagAndNameSeparator + dialog.DesiredLanguageName;
				}
			}
		}

		private void UpdateWorkingLanguageFontDisplay()
		{
			var i = _labelSelectedWorkingLanguage.Text?.IndexOf(kLanguageTagAndNameSeparator, StringComparison.OrdinalIgnoreCase) ?? -1;
			if (i > 0)
			{
				_linkSelectFontForWorkingLanguage.Show();
				var languageName = _labelSelectedWorkingLanguage.Text
					.Substring(i + kLanguageTagAndNameSeparator.Length);
				_linkSelectFontForWorkingLanguage.Text = 
					Format(_fmtFontForWorkingLanguage, languageName);
				return;
			}
			
			_linkSelectFontForWorkingLanguage.Hide();
		}

		private void _linkSelectContentLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			// REVIEW: Do we need Force3LetterCodes? Can't find it in new implementation.
			using (var dialog = new LanguageLookupDialog { IsDesiredLanguageNameFieldVisible = true })
			{
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK && dialog.SelectedLanguage != null)
					_labelSelectedContentLanguage.Text = $@"{dialog.SelectedLanguage.LanguageTag}: {dialog.DesiredLanguageName}";
			}
		}

		private void _linkSelectFontForWorkingLanguage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var newFont = FontDialogHelper.SelectFont(FindForm(), _projectTitle.Font);
			if (newFont != null)
				SetWorkingLanguageFont(newFont);
		}

		private void WorkingLanguageChanged(object sender, EventArgs e)
		{
			UpdateWorkingLanguageFontDisplay();
		}
	}
}
