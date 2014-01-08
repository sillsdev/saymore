using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.LowLevelControls;
using SIL.Archiving.Generic.AccessProtocol;
using SIL.Archiving.IMDI.Lists;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionBasicEditor : EditorBase
	{
		public delegate SessionBasicEditor Factory(ComponentFile file, string imageKey);

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGrid _gridAdditionalFields;
		private FieldsValuesGridViewModel _gridViewModel;
		private FieldsValuesGridViewModel _additionalFieldsGridViewModel;
		private readonly PersonInformant _personInformant;
		private readonly AutoCompleteValueGatherer _autoCompleteProvider;
		private bool _genreFieldEntered;
		private List<AccessOption> _accessOptions;

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer,
			PersonInformant personInformant)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "SessionEditor";

			_personInformant = personInformant;
			InitializeGrid(autoCompleteProvider, fieldGatherer);

			_autoCompleteProvider = autoCompleteProvider;
			_autoCompleteProvider.NewDataAvailable += LoadGenreList;

			_binder.TranslateBoundValueBeingRetrieved += HandleBinderTranslateBoundValueBeingRetrieved;

			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);
			_participants.JITListAcquisition = HandleParticipantJustInTimeListAcquisition;

			_id.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelId); };
			_date.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelDate); };

			_genre.Enter += delegate { _genreFieldEntered = true; };
			_genre.Leave += delegate { _genreFieldEntered = false; };
			_genre.KeyPress += HandleGenreKeyPress;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGenreList(object sender, EventArgs e)
		{
			var genreList = new List<string>();

			var autoCompleteProvider = sender as AutoCompleteValueGatherer;

			if (autoCompleteProvider != null)
			{
				// Add the genres in use, factory or not.
				var valueLists = autoCompleteProvider.GetValueLists(false);
				IEnumerable<string> list;
				if (valueLists.TryGetValue(SessionFileType.kGenreFieldName, out list))
					genreList.AddRange(GenreDefinition.GetGenreNameList(list));
			}

			// Add the rest of the factory defaults
			if (GenreDefinition.FactoryGenreDefinitions != null)
			{
				if (genreList.Count > 0)
					genreList.Add("-----");

				genreList.AddRange(GenreDefinition.FactoryGenreDefinitions.Select(gd => gd.Name).ToArray());
			}

			if (genreList.Count == 0 || !_genre.IsHandleCreated)
				return;

			// Do this because we've gotten here when the auto-complete helper has new data available.
			_genre.BeginInvoke((MethodInvoker)delegate
			{
				_genre.Items.Clear();
				_genre.Items.AddRange(genreList.Cast<object>().ToArray());
			});
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGenreKeyPress(object sender, KeyPressEventArgs e)
		{
			if (_genreFieldEntered && !Char.IsControl(e.KeyChar) &&
				_genre.SelectionLength < _genre.Text.Length &&
				_genre.Text.Contains(GenreDefinition.UnknownType.Name))
			{
				_genre.Text = _genre.Text.Replace(GenreDefinition.UnknownType.Name, string.Empty);
				_genre.SelectionLength = 0;
				_genre.SelectionStart = 0;
				_genreFieldEntered = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<PickerPopupItem> HandleParticipantJustInTimeListAcquisition()
		{
			return from name in _personInformant.GetPeopleNamesFromRepository()
				   orderby name
				   select new PickerPopupItem { Text = name, ToolTipText = null };
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			// additional fields grid
			_additionalFieldsGridViewModel = new AdditionalFieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer);

			_gridAdditionalFields = new FieldsValuesGrid(_additionalFieldsGridViewModel);
			_gridAdditionalFields.Dock = DockStyle.Top;
			_gridAdditionalFields.AllowUserToAddRows = false;
			_panelAdditionalGrid.AutoSize = true;
			_panelAdditionalGrid.Controls.Add(_gridAdditionalFields);

			// set country dropdown
			var countryList = ListConstructor.GetList(ListType.Countries);
			countryList.Insert(0, new IMDIListItem(string.Empty, string.Empty)); // add a blank option
			var countryVal = _gridAdditionalFields[1, 2].Value.ToString();
			if (countryList.FindByValue(countryVal) == null)
				countryVal = string.Empty;

			var countryCell = new DataGridViewComboBoxCell
			{
				DataSource = countryList,
				DisplayMember = "Text",
				ValueMember = "Value",
				Value = countryVal
			};

			_gridAdditionalFields[1, 2] = countryCell;

			// set continent dropdown
			var continentList = ListConstructor.GetList(ListType.Continents);
			continentList.Insert(0, new IMDIListItem(string.Empty, string.Empty)); // add a blank option
			var continentVal = _gridAdditionalFields[1, 3].Value.ToString();
			if (continentList.FindByValue(continentVal) == null)
				continentVal = string.Empty;

			var continentCell = new DataGridViewComboBoxCell
			{
				DataSource = continentList,
				DisplayMember = "Text",
				ValueMember = "Value",
				Value = continentVal
			};

			_gridAdditionalFields[1, 3] = continentCell;

			// custom fields grid
			_gridViewModel = new CustomFieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer);

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel);
			_gridCustomFields.Dock = DockStyle.Top;
			_panelGrid.AutoSize = true;
			_panelGrid.Controls.Add(_gridCustomFields);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_additionalFieldsGridViewModel != null)
				_additionalFieldsGridViewModel.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void Activated()
		{
			if (_genre.Items.Count == 0)
				LoadGenreList(_autoCompleteProvider, null);

			if (_accessOptions != null)
				SetAccessCodeListAndValue();

			base.Activated();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("SessionsView.MetadataEditor.TabText", "Session");
			if (_genre != null && !String.IsNullOrEmpty(_genre.Text))
			{
				var genreId = GenreDefinition.TranslateNameToId(_genre.Text);
				if (genreId != _genre.Text)
					_genre.Text = GenreDefinition.TranslateIdToName(genreId);
			}
			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This feels like a complete hack to fix SP-586, but I can't find where the text is
		/// getting selected. When the Genre gets set from the value in the component file, the
		/// text is definitely not selected. I even tried setting a breakpoint in several
		/// methods in the .Net ComboBox code itself. I get to those breakpoints, so I know I've
		/// set them correctly, but between the time the text is set and the time the control
		/// gets painted with the text selected, these breakpoints never get hit.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HideTextSelection(object sender, EventArgs e)
		{
			if (ActiveControl != _genre && (_genre.SelectionStart == 0 && _genre.SelectionLength > 0))
				_genre.SelectionLength = 0;
		}

		/// ------------------------------------------------------------------------------------
		public void SetAccessProtocol()
		{
			if (Program.CurrentProject == null) return;

			var accessProtocol = Program.CurrentProject.AccessProtocol;
			var protocols = AccessProtocols.LoadStandardAndCustom();
			var protocol = protocols.FirstOrDefault(i => i.ProtocolName == accessProtocol);

			// is "None" the selected protocol?
			if ((accessProtocol == "None") || (protocol == null))
			{
				_access.DataSource = null;
				_access.DropDownStyle = ComboBoxStyle.DropDown;
				return;
			}

			// remember the list of possible choices
			_accessOptions = protocol.Choices;
			_access.DropDownStyle = ComboBoxStyle.DropDownList;

			SetAccessCodeListAndValue();
		}

		/// <summary>do this in case the access protocol for the project changed and
		/// the current value of "access" is no longer in the list</summary>
		private void SetAccessCodeListAndValue()
		{
			var currentAccessCode = _file.GetStringValue("access", null);

			if (_access.DropDownStyle == ComboBoxStyle.DropDown)
			{
				if (currentAccessCode == null)
					_access.Text = string.Empty;
				else
					_access.Text = currentAccessCode;
				return;
			}

			if (currentAccessCode == null) return;

			// is the selected item in the list
			var choices = _accessOptions.ToList();
			var found = choices.Any(i => i.ToString() == currentAccessCode);

			if (!found)
			{
				choices.Insert(0, new AccessOption { OptionName = "-----" });
				choices.Insert(0, new AccessOption { OptionName = currentAccessCode });
			}

			_access.DataSource = choices;

			// select the current code
			foreach (var item in _access.Items.Cast<object>().Where(i => i.ToString() == currentAccessCode))
				_access.SelectedItem = item;
		}

		/// <summary>
		/// Replace comma with correct delimiter in MultiValueDropDownBox
		/// </summary>
		private void HandleBinderTranslateBoundValueBeingRetrieved(object sender,
			TranslateBoundValueBeingRetrievedArgs args)
		{
			if (!(args.BoundControl is MultiValueDropDownBox)) return;

			if (args.ValueFromFile.Contains(","))
				args.TranslatedValue = args.ValueFromFile.Replace(",", FieldInstance.kDefaultMultiValueDelimiter.ToString(CultureInfo.InvariantCulture));
		}
	}
}
