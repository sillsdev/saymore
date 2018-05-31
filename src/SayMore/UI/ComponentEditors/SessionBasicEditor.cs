using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using SIL.Extensions;
using SIL.Reporting;
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
		private AdditionalFieldsValuesGridViewModel _additionalFieldsGridViewModel;
		private readonly PersonInformant _personInformant;
		private readonly AutoCompleteValueGatherer _autoCompleteProvider;
		private bool _genreFieldEntered;
		private List<AccessOption> _accessOptions;
		private DataGridViewComboBoxEditingControl _moreFieldsComboBox;

		/// ------------------------------------------------------------------------------------
		public SessionBasicEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer,
			PersonInformant personInformant)
			: base(file, null, imageKey)
		{
			Logger.WriteEvent("PersonBasicEditor constructor. file = {0}", file);

			InitializeComponent();
			Name = "SessionEditor";

			_personInformant = personInformant;

			_autoCompleteProvider = autoCompleteProvider;
			_autoCompleteProvider.NewDataAvailable += LoadGenreList;

			_binder.TranslateBoundValueBeingRetrieved += HandleBinderTranslateBoundValueBeingRetrieved;
			_binder.TranslateBoundValueBeingSaved += HandleBinderTranslateBoundValueBeingSaved;

			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);

			_id.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelId); };
			_date.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelDate); };

			_genre.Enter += delegate { _genreFieldEntered = true; };
			_genre.Leave += delegate { _genreFieldEntered = false; };
			_genre.KeyPress += HandleGenreKeyPress;

			InitializeGrid(autoCompleteProvider, fieldGatherer);

			file.AfterSave += file_AfterSave;


			if (_personInformant != null)
				_personInformant.PersonUiIdChanged += HandlePersonsUiIdChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this message from the person informant when a person's UI ID has changed.
		/// When that happens, we just need to update the Text in the participant control. No
		/// change is needed (or desirable) in the underlying metadata.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePersonsUiIdChanged(object sender, ElementIdChangedArgs e)
		{
			var allParticipants = FieldInstance.GetMultipleValuesFromText(_participants.Text);
			var newNames = allParticipants.Select(name => (name == e.OldId ? e.NewId : name));
			_participants.Text = FieldInstance.GetTextFromMultipleValues(newNames);
		}

		static void file_AfterSave(object sender, EventArgs e)
		{
			Program.OnPersonDataChanged();
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
		protected override void OnParentTabControlVisibleChanged()
		{
			OnEditorAndChildrenLostFocus();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			// additional fields grid
			_additionalFieldsGridViewModel = new AdditionalFieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer) {AllowUserToAddRows = false};

			_gridAdditionalFields = new FieldsValuesGrid(_additionalFieldsGridViewModel, "SessionBasicEditor._gridAdditionalFields") { Dock = DockStyle.Top };

			// to get a more helpful exception output than the default DataGrid error message
			_gridAdditionalFields.DataError += _gridAdditionalFields_DataError;

			_panelAdditionalGrid.AutoSize = true;
			_panelAdditionalGrid.Controls.Add(_gridAdditionalFields);

			for (int i = 0; i < _gridAdditionalFields.RowCount; i++)
			{
				var listType = _additionalFieldsGridViewModel.GetListType(i);
				if (listType != null)
					AddDropdownCell(listType, i);
			}

			_gridAdditionalFields.EditingControlShowing += _gridAdditionalFields_EditingControlShowing;
			// custom fields grid
			_gridViewModel = new CustomFieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer);

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel, "SessionBasicEditor._gridCustomFields") { Dock = DockStyle.Top };
			_panelGrid.AutoSize = true;
			_panelGrid.Controls.Add(_gridCustomFields);
		}

		#region Methods to support display of tool-tips for long items in combo-boxes for "additional fields"
		void _gridAdditionalFields_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			_gridAdditionalFields.CurrentCell.ToolTipText = string.Empty;

			if (_moreFieldsComboBox != null)
			{
				if (_moreFieldsToolTip.Active)
					_moreFieldsToolTip.RemoveAll();

				if (_moreFieldsComboBox.IsDisposed)
					_moreFieldsComboBox = null;
				else
				{
					_moreFieldsComboBox.DrawItem -= HandleMoreFieldsComboDrawItem;
					_moreFieldsComboBox.DropDown -= HandleMoreFieldsComboDropDown;
				}
			}

			_moreFieldsComboBox = e.Control as DataGridViewComboBoxEditingControl;
			if (_moreFieldsComboBox == null)
				return;

			bool hasDefinitions = _moreFieldsComboBox.Items.Cast<IMDIListItem>().Any(item => !string.IsNullOrEmpty(item.Definition));
			if (!hasDefinitions)
			{
				_moreFieldsComboBox = null;
				return;
			}

			_moreFieldsComboBox.DrawMode = DrawMode.OwnerDrawFixed;
			_moreFieldsComboBox.DrawItem += HandleMoreFieldsComboDrawItem;
			_moreFieldsComboBox.DropDown += HandleMoreFieldsComboDropDown;
		}

		void HandleMoreFieldsComboDropDownClosed(object sender, EventArgs e)
		{
			var selectedItem = _moreFieldsComboBox.SelectedItem as IMDIListItem;
			if (selectedItem != null && selectedItem.Definition != selectedItem.Text)
				_gridAdditionalFields.CurrentCell.ToolTipText = selectedItem.Definition;
			_moreFieldsToolTip.RemoveAll();
			_moreFieldsComboBox.DropDownClosed -= HandleMoreFieldsComboDropDownClosed;
		}

		void HandleMoreFieldsComboDropDown(object sender, EventArgs e)
		{
			_moreFieldsComboBox.DropDownClosed += HandleMoreFieldsComboDropDownClosed;
			_gridAdditionalFields.CurrentCell.ToolTipText = string.Empty;
		}

		void HandleMoreFieldsComboDrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			string text = _moreFieldsComboBox.GetItemText(_moreFieldsComboBox.Items[e.Index]);
			using (SolidBrush br = new SolidBrush(e.ForeColor))
			{
				e.Graphics.DrawString(text, e.Font, br, e.Bounds);
			}
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				var toolTipText = ((IMDIListItem)_moreFieldsComboBox.Items[e.Index]).Definition;
				if (toolTipText != text && _gridAdditionalFields.CurrentRow != null && _moreFieldsComboBox.DroppedDown)
				{
					// SP-799:  Tooltip text too long for netbook screen
					var form = FindForm();
					if (form != null)
					{
						// get the number of segments
						var screenWidth = Screen.FromHandle(form.Handle).WorkingArea.Width - 100;
						var maxWidth = screenWidth < 600 ? screenWidth : 600;
						var textWidth = e.Graphics.MeasureString(toolTipText, SystemFonts.DefaultFont).Width;
						var segments = (int)(Math.Ceiling(textWidth / maxWidth));

						if (segments > 1)
						{
							var shorterTip = new StringBuilder();
							var testTip = string.Empty;
							var words = toolTipText.Split(new[] {' '});

							foreach (var word in words)
							{
								var testWidth = e.Graphics.MeasureString(testTip + " " + word, SystemFonts.DefaultFont).Width;

								// if the new word makes it too long, start a new line
								if (testWidth > maxWidth)
								{
									shorterTip.AppendLine(testTip);
									testTip = string.Empty;
								}

								// add the new word to the test string
								if (string.IsNullOrEmpty(testTip))
									testTip = word;
								else
									testTip += " " + word;
							}

							// if there are any left overs, add them now
							if (!string.IsNullOrEmpty(testTip))
								shorterTip.AppendLine(testTip);

							toolTipText = shorterTip.ToString();
						}
					}

					_moreFieldsToolTip.Show(toolTipText, _moreFieldsComboBox, e.Bounds.Right,
						e.Bounds.Bottom + _gridAdditionalFields.CurrentRow.Height + 5);
				}
				else
					_moreFieldsToolTip.RemoveAll();
			}
			e.DrawFocusRectangle();
		}
		#endregion

		/// <summary>This gives a more helpful exception output than the default DataGrid error message</summary>
		void _gridAdditionalFields_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			var frm = FindForm();
			if (frm == null) return;

			// this happens if the cell value is not in the dropdown list
			if (e.Exception is ArgumentException) return;

			MessageBox.Show(frm, e.Exception.Message, frm.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			//throw new Exception(e.Exception.Message, e.Exception);
		}

		private void AddDropdownCell(string listType, int row)
		{
			var list = ListConstructor.GetList(listType, true, Localize, ListConstructor.RemoveUnknown.RemoveAll);

			var currentValue = _gridAdditionalFields[1, row].Value.ToString();

			if (list.FindByValue(currentValue) == null)
			{
				currentValue = string.Empty;
				_gridAdditionalFields[1, row].Value = currentValue;
			}

			var cell = new DataGridViewComboBoxCell
			{
				DataSource = list,
				DisplayMember = "Text",
				ValueMember = "Value",
				Value = currentValue,
				FlatStyle = FlatStyle.Flat
			};

			_gridAdditionalFields[1, row] = cell;

			// Added Application.DoEvents() because it was interferring with the background
			// file processor if it needed to download the list files.
			Application.DoEvents();
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

			if (value.StartsWith("Holy Seat")) value = value.Replace("Holy Seat", "Holy See");

			return value;
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

			if (_accessOptions == null)
				SetAccessProtocol();

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

			if (_gridAdditionalFields != null)
			{
				for (int iRow = 0; iRow < _gridAdditionalFields.RowCount; iRow++)
				{
					DataGridViewComboBoxCell comboBoxCell = _gridAdditionalFields[1, iRow] as DataGridViewComboBoxCell;
					if (comboBoxCell != null)
					{
						IMDIItemList list = comboBoxCell.DataSource as IMDIItemList;
						if (list != null)
						{
							list.Localize(Localize);
						}
					}
				}
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

			// All items in the list should be upper-case-initial.
			if ((ActiveControl != _genre)
				&& (!string.IsNullOrEmpty(_genre.Text))
				&& (_genre.Text.Substring(0, 1) != _genre.Text.Substring(0, 1).ToUpper()))
			{
				_genre.Text = _genre.Text.ToUpperFirstLetter();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetPeople(string participantsList)
		{
			_participants.Text = participantsList;
		}

		/// ------------------------------------------------------------------------------------
		public void SetAccessProtocol()
		{
			if (Program.CurrentProject == null)
			{
				GetDataInBackground();
				return;
			}

			var accessProtocol = Program.CurrentProject.AccessProtocol;
			var protocols = AccessProtocols.LoadStandardAndCustom();
			var protocol = protocols.FirstOrDefault(i => i.ProtocolName == accessProtocol);

			// is "None" the selected protocol?
			if ((accessProtocol == "None") || (protocol == null))
			{
				_access.DataSource = null;
				_access.DropDownStyle = ComboBoxStyle.DropDown;
			}
			else
			{
				// remember the list of possible choices
				_accessOptions = protocol.Choices;

				// localize the list
				foreach (var item in _accessOptions)
					item.Description = LocalizationManager.GetDynamicString("SayMore",
						"SessionsView.MetadataEditor.AccessProtocol." + accessProtocol + "." + item.ValueMember, item.DisplayMember, null);

				_access.DropDownStyle = ComboBoxStyle.DropDownList;
			}

			SetAccessCodeListAndValue();
		}

		private void GetDataInBackground()
		{
			using (BackgroundWorker backgroundWorker = new BackgroundWorker())
			{
				backgroundWorker.DoWork += backgroundWorker_DoWork;
				backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
				backgroundWorker.RunWorkerAsync();
			}
		}

		void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			SetAccessProtocol();
		}

		void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var count = 0;
			while ((Program.CurrentProject == null) && (count < 50))
			{
				Thread.Sleep(100);
				count++;
			}
		}

		/// <summary>do this in case the access protocol for the project changed and
		/// the current value of "access" is no longer in the list</summary>
		private void SetAccessCodeListAndValue()
		{
			var currentAccessCode = _file.GetStringValue("access", string.Empty);

			if (_access.DropDownStyle == ComboBoxStyle.DropDown)
			{
				_access.Text = currentAccessCode ?? string.Empty;
				return;
			}

			// get the saved list. use .ToList() to copy the list rather than modify the original
			var choices = _accessOptions.ToList();

			// is the selected item in the list
			var found = choices.Any(i => i.ToString() == currentAccessCode);

			if (!found)
			{
				choices.Insert(0, new AccessOption { OptionName = "-----" });
				choices.Insert(0, new AccessOption { OptionName = currentAccessCode });
			}

			_access.DataSource = choices;
			_access.DisplayMember = "DisplayMember";
			_access.ValueMember = "ValueMember";

			// select the current code
			foreach (var item in _access.Items.Cast<object>().Where(item => ((AccessOption)item).ValueMember == currentAccessCode))
				_access.SelectedItem = item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Replace comma with correct delimiter in MultiValueDropDownBox. Translate full names
		/// of persons into strings acceptable to show in UI (use "code" instead of Full Name
		/// when available).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBinderTranslateBoundValueBeingRetrieved(object sender,
			TranslateBoundValueBeingRetrievedArgs args)
		{
			if (!(args.BoundControl is MultiValueDropDownBox)) return;

			if (args.ValueFromFile.Contains(","))
				args.TranslatedValue = args.ValueFromFile.Replace(",", FieldInstance.kDefaultMultiValueDelimiter.ToString(CultureInfo.InvariantCulture));

			if (args.BoundControl == _participants)
			{
				var val = args.TranslatedValue ?? args.ValueFromFile;
				if (!string.IsNullOrEmpty(val))
				{
					var participantNames = FieldInstance.GetMultipleValuesFromText(val).ToArray();
					for (int index = 0; index < participantNames.Length; index++)
						participantNames[index] = _personInformant.GetUiIdByName(participantNames[index]);

					args.TranslatedValue = FieldInstance.GetTextFromMultipleValues(participantNames);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the binding helper gets to writing field values to the metadata file, we need
		/// to make sure the English values for male and female are written to the file, not
		/// the localized values for male and female (which is what is in the gender combo box).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBinderTranslateBoundValueBeingSaved(object sender,
			TranslateBoundValueBeingSavedArgs args)
		{
			if (args.BoundControl == _participants)
			{
				var participantNames = FieldInstance.GetMultipleValuesFromText(_participants.Text).ToArray();
				for (int index = 0; index < participantNames.Length; index++)
				{
					var person = _personInformant.GetPersonByNameOrCode(participantNames[index]);
					if (person != null)
						participantNames[index] = person.Id;
				}

				args.NewValue = FieldInstance.GetTextFromMultipleValues(participantNames);
			}
		}

		private void HandlePeopleEditClick(object sender, EventArgs e)
		{
			var frm = FindForm();
			if (frm == null)
				return;

			var tabPages = ((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.TabPages;
			foreach (TabPage tab in tabPages)
			{
				if (tab.ImageKey != @"Contributor") continue;
				((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.SelectedTab = tab;
				tab.Focus();
				break;
			}
		}
	}
}
