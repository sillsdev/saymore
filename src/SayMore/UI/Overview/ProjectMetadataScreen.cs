using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Palaso.UI.WindowsForms.WritingSystems;
using SayMore.UI.ComponentEditors;
using SayMore.UI.ProjectWindow;
using SIL.Archiving.IMDI.Lists;

namespace SayMore.UI.Overview
{
	public partial class ProjectMetadataScreen : EditorBase, ISayMoreView
	{

		public ProjectMetadataScreen()
		{
			InitializeComponent();

			// position the Description and Vernacular label correctly
			var rowHeight = _tableLayout.GetRowHeights()[0];
			_tableLayout.RowStyles[2].SizeType = SizeType.Absolute;
			_tableLayout.RowStyles[2].Height = rowHeight;
			_tableLayout.RowStyles[4].SizeType = SizeType.Absolute;
			_tableLayout.RowStyles[4].Height = rowHeight;

			// continent list
			var continentList = ListConstructor.GetList(ListType.Continents);
			_continent.DataSource = continentList;
			_continent.DisplayMember = "Text";
			_continent.ValueMember = "Value";
			SizeContinentComboBox(_continent);

			// country list
			var countryList = ListConstructor.GetList(ListType.Countries);
			_country.DataSource = countryList;
			_country.DisplayMember = "Text";
			_country.ValueMember = "Value";
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

		private void _linkSelectVernacular_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (var dialog = new LookupISOCodeDialog { Force3LetterCodes = true })
			{
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK)
					_labelSelectedVernacular.Text = dialog.SelectedLanguage.Code + @": " + dialog.SelectedLanguage.DesiredName;
			}

		}

		private void ProjectMetadataScreen_Leave(object sender, EventArgs e)
		{
			Save();
		}

		private void ProjectMetadataScreen_Load(object sender, EventArgs e)
		{
			// show values from project file
			var project = Program.CurrentProject;

			if (project == null) return;

			_projectTitle.Text = project.Title;
			_fundingProjectTitle.Text = project.FundingProjectTitle;
			_description.Text = project.ProjectDescription;
			_labelSelectedVernacular.Text = project.VernacularISO3CodeAndName;
			_location.Text = project.Location;
			_region.Text = project.Region;
			_country.Text = project.Country;

			foreach (var item in _continent.Items.Cast<object>().Where(i => i.ToString() == project.Continent))
				_continent.SelectedItem = item;

			_contactPerson.Text = project.ContactPerson;
			_contentType.Text = project.ContentType;
			_applications.Text = project.Applications;
			_dateAvailable.SetValue(project.DateAvailable);
			_rightsHolder.Text = project.RightsHolder;
			_depositor.Text = project.Depositor;
			_relatedPublications.Text = project.RelatedPublications;
		}

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

		internal void Save()
		{
			// check for changes
			var changed = false;
			var project = Program.CurrentProject;

			if (_projectTitle.Text != project.Title ||
				_fundingProjectTitle.Text != project.FundingProjectTitle ||
				_description.Text != project.ProjectDescription ||
				_labelSelectedVernacular.Text != project.VernacularISO3CodeAndName ||
				_location.Text != project.Location ||
				_region.Text != project.Region ||
				_country.Text != project.Country ||
				_continent.Text != project.Continent ||
				_contactPerson.Text != project.ContactPerson ||
				_contentType.Text != project.ContentType ||
				_applications.Text != project.Applications ||
				_dateAvailable.Text != project.DateAvailable ||
				_rightsHolder.Text != project.RightsHolder ||
				_depositor.Text != project.Depositor ||
				_relatedPublications.Text != project.RelatedPublications
				)
				changed = true;

			if (!changed) return;

			// save changes
			project.Title = _projectTitle.Text;
			project.FundingProjectTitle = _fundingProjectTitle.Text;
			project.ProjectDescription = _description.Text;
			project.VernacularISO3CodeAndName = _labelSelectedVernacular.Text;
			project.Location = _location.Text;
			project.Region = _region.Text;
			project.Country = _country.Text;
			project.Continent = _continent.Text;
			project.ContactPerson = _contactPerson.Text;
			project.ContentType = _contentType.Text;
			project.Applications = _applications.Text;

			project.DateAvailable = _dateAvailable.GetISO8601DateValueOrNull();
			project.RightsHolder = _rightsHolder.Text;
			project.Depositor = _depositor.Text;
			project.RelatedPublications = _relatedPublications.Text;
			project.Save();
		}
	}
}
