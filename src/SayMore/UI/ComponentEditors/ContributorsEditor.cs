using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SIL.Windows.Forms.ClearShare;
using SIL.Windows.Forms.ClearShare.WinFormsUI;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SIL.Core.ClearShare;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.Widgets.BetterGrid;
using static SIL.Windows.Forms.Extensions.ControlExtensions.ErrorHandlingAction;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		public delegate ContributorsEditor Factory(ComponentFile file, string imageKey);

		private bool _active;
		protected ContributorsListControl _contributorsControl;
		protected TableLayoutPanel _tableLayout;
		protected LinkLabel _linkBack;
		protected ContributorsListControlViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, PersonInformant personInformant) :
			base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";

			file.StartingRename += File_StartingRename;

			_model = new ContributorsListControlViewModel(autoCompleteProvider, SaveContributors);
			var dataGridView = new DataGridView();
			dataGridView.Columns[dataGridView.Columns.Add("date", "date")].Visible = false;
			_model.ContributorsGridSettings = GridSettings.Create(dataGridView);

			SetComponentFile(file);

			_contributorsControl = new ContributorsListControl(_model)
			{
				Dock = DockStyle.Fill,
			};

			_contributorsControl.ValidatingContributor += HandleValidatingContributor;

			InitializeGrid(personInformant.GetAllPeopleNames);

			// imageKey == "Contributor" when ContributorsEditor is lazy loaded for the session file type
			if (imageKey != null)
			{
				AddSessionControls();
			}
			else
			{
				Controls.Add(_contributorsControl);
			}

			file.AfterSave += File_AfterSave;

			personInformant.PersonUiIdChanged += HandlePersonsUiIdChanged;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (IsHandleCreated)
			{
				// This fix was added as part of SP-2297. Although not needed to prevent
				// the crash, it is needed to make it clear how to delete a row.
				// Annoyingly, the Contributors Control can't seem to shrink (even if I
				// set the AutoSizeMode to GrowAndShrink). I'm guessing that it somehow
				// thinks its calculated width for the comments column is set in stone,
				// even though its minimum width is 5. Anyway, this logic fixes it:
				if (_contributorsControl.Width >= _contributorsControl.Parent.Width)
				    _contributorsControl.Width = _contributorsControl.Parent.ClientSize.Width - _contributorsControl.Parent.Padding.Horizontal;
			}
		}

		private void File_StartingRename(ComponentFile sender, CancelEventArgs e)
		{
			e.Cancel = !_contributorsControl.Validate();
			if (e.Cancel)
				_contributorsControl.Focus();
		}

		private void AddSessionControls()
		{
			_tableLayout = new TableLayoutPanel
			{
				Name = "_tableLayout",
				Dock = DockStyle.Top,
				AutoSize = true,
				BackColor = Color.Transparent,
				ColumnCount = 1,
				RowCount = 2
			};
			_tableLayout.ColumnStyles.Add(new ColumnStyle());
			_tableLayout.Location = new Point(0, 0);
			_linkBack = new LinkLabel
			{
				Name = "_linkBack",
				Dock = DockStyle.Left,
				AutoSize = true,
				LinkBehavior = LinkBehavior.NeverUnderline,
				BackColor = Color.Transparent,
				ForeColor = Color.Black,
				LinkColor = Color.Black,
				DisabledLinkColor = Color.Black,
				TextAlign = ContentAlignment.TopLeft,
				Anchor = AnchorStyles.Left,
				Text = "<",
				Font = new Font("Segoe UI Symbol", 16)
			};
			_linkBack.LinkClicked += HandleLinkClick;
			_tableLayout.Controls.Add(_linkBack, 0, 0);
			_tableLayout.Controls.Add(_contributorsControl, 0, 1);
			Controls.Add(_tableLayout);
		}

		private void HandleLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var frm = FindForm();
			if (frm == null)
				return;

			var tabPages = ((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.TabPages;
			foreach (TabPage tab in tabPages)
			{
				if (tab.ImageKey != @"Session") continue;
				((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.SelectedTab = tab;
				tab.Focus();
				break;
			}
		}

		/// <remarks>SP-874: Not able to open L10NSharp with Alt-Shift-click</remarks>
		private void InitializeGrid(Func<IEnumerable<string>> getPeopleNames)
		{
			// misc. column header settings
			_contributorsControl.Grid.BorderStyle = BorderStyle.None;
			_contributorsControl.Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			_contributorsControl.Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

			// set the localizable column header text
			string[] headerText =
			{
				@"_L10N_:SessionsView.ContributorsEditor.NameColumnTitle!Name",
				@"_L10N_:SessionsView.ContributorsEditor.RoleColumnTitle!Role",
				@"_L10N_:SessionsView.ContributorsEditor.DateColumnTitle!Date",
				@"_L10N_:SessionsView.ContributorsEditor.CommentColumnTitle!Comments"
			};

			for (var i = 0; i < headerText.Length; i++)
			{
				_contributorsControl.SetColumnHeaderText(i, headerText[i]);

				// this is needed to match the visual behavior of the other grids
				_contributorsControl.Grid.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
			}

			foreach (DataGridViewColumn col in _contributorsControl.Grid.Columns)
			{
				if (!col.Visible)
					continue;
				switch (col.Name)
				{
					case "name":
						InitializeColumnWidth(col, _contributorsControl.Grid, getPeopleNames());
						break;
					case "role":
						col.MinimumWidth = 70;
						col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
						break;
					case "comments":
						col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
						col.FillWeight = 100;
						break;
				}
			}

			// set column header height to match the other grids
			_contributorsControl.Grid.AutoResizeColumnHeadersHeight();
			_contributorsControl.Grid.ColumnHeadersHeight += 8;

			// make it localizable
			L10NSharpExtender locExtender = new L10NSharpExtender();
			locExtender.LocalizationManagerId = "SayMore";
			_contributorsControl.SetLocalizationExtender(locExtender);

			locExtender.EndInit();

			NotifyWhenProjectIsSet();
		}

		private static void InitializeColumnWidth(DataGridViewColumn col, DataGridView grid, IEnumerable<string> possibilities)
		{
			col.MinimumWidth = 100;
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			// If there are already several rows, there's a pretty decent chance that the min of 100 or the
			// AllCells option will get us a sufficiently wide column, so we can avoid the extra trouble of calculating
			// the minimum width based on the widest possible string.
			if (grid.RowCount > 4)
				return;
		
			float minWidth = 0;
			var font = col.DefaultCellStyle.Font ?? grid.DefaultCellStyle.Font ?? grid.Font;
			foreach (var name in possibilities)
			{
				minWidth = Math.Max(minWidth,
					grid.CreateGraphics().MeasureString(name, font).Width);
			}

			if (minWidth > col.MinimumWidth)
				col.MinimumWidth = (int)Math.Ceiling(minWidth);
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
			var contribs = _model.Contributions;
			foreach (var c in contribs.Where(c => c.ContributorName == e.OldId))
				c.ContributorName = e.NewId;

			_model.SetContributionList(contribs);
		}

		static void File_AfterSave(object sender, EventArgs e)
		{
			Program.OnPersonDataChanged();
		}

		/// ------------------------------------------------------------------------------------
		public override void Deactivated()
		{
			base.Deactivated();
			// SP-2267: This happens when a different file is being associated with the
			// contributors editor. Usually we try to prevent this if the editor is dirty and
			// cannot validate and save, but in the case where the associated session was just
			// deleted, we can safely discard the changes as they are now clearly irrelevant.
			// REVIEW: I have checked all the possible scenarios I can think of and I *think*
			// I've handled them properly. But there is still one scenario I have not been able
			// to reproduce (see SP-2186, SP-2265, SP-2266). It is possible that we will get
			// here for that scenario and for other possible future scenarios. Although that
			// might not be optimal, it might manage to avoid a crash.
			_active = false; // see HandleValidatingContributor
			if (_contributorsControl.InEditMode)
				_contributorsControl.Grid.CancelEdit(); // This probably does nothing.
		}

		/// ------------------------------------------------------------------------------------
		public sealed override void SetComponentFile(ComponentFile file)
		{
			_file.StartingRename -= File_StartingRename;
			base.SetComponentFile(file);
			file.StartingRename += File_StartingRename;
			_model.SetContributionList(file.GetValue(SessionFileType.kContributionsFieldName, null) as ContributionCollection);
			_active = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetWorkingLanguageFont(Font font)
		{
			this.SafeInvoke(() =>
				{
					foreach (DataGridViewColumn col in _contributorsControl.Grid.Columns)
					{
						if (col.Name == "comments")
						{
							var style = new DataGridViewCellStyle(col.DefaultCellStyle)
							{
								Font = font
							};
							col.CellTemplate.Style = col.DefaultCellStyle = style;
							// REVIEW: No matter what I do, I can't seem to find a way to force the
							// DGV to repaint using the updated font. (The very first time this
							// grid displays, it uses the default UI font and then after a
							// very pregnant pause, it updates to use the desired font.) Changing
							// this font should be fairly rare, and eventually if the user selects
							// a different session file, a new grid is instantiated with the
							// correct font, so that's probably acceptable, but it would be nice
							// to have this work correctly.
							_contributorsControl.Grid.InvalidateColumn(col.Index);
							break;
						}
					}

				}, $"{GetType().Name}.{nameof(SetWorkingLanguageFont)}", IgnoreAll);
		}

		/// ------------------------------------------------------------------------------------
		private KeyValuePair<string, string> HandleValidatingContributor(ContributorsListControl sender,
			Contribution contribution, CancelEventArgs e)
		{
			// If we are not active, we do not want to return an error, but we also don't want
			// to save. We just want to discard any changes.
			if (!_active)
				return default;

			var kvp = CheckIfContributorIsValid(contribution);
			e.Cancel = !string.IsNullOrEmpty(kvp.Key);

			if (!e.Cancel)
				SaveContributors();

			return kvp;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKToLeaveEditor
		{
			get
			{
				var contribution = _contributorsControl.GetCurrentContribution();

				if (contribution == null ||
					(contribution.IsEmpty && _contributorsControl.InNewContributionRow))
				{
					return true;
				}

				if (_contributorsControl.InEditMode)
				{
					if (!_contributorsControl.Validate())
						return false;
				}

				return string.IsNullOrEmpty(CheckIfContributorIsValid(contribution).Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static KeyValuePair<string, string> CheckIfContributorIsValid(Contribution contribution)
		{
			if (contribution != null)
			{
				if (string.IsNullOrEmpty(contribution.ContributorName))
				{
					return new KeyValuePair<string, string>("name",
						LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorNameMsg", "Enter a name."));
				}

				if (contribution.Role == null)
				{
					return new KeyValuePair<string, string>("role",
						LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorRoleMsg", "Choose a role."));
				}
			}

			return new KeyValuePair<string, string>();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveContributors()
		{
			_file.SetValue(SessionFileType.kContributionsFieldName, _model.Contributions, out var failureMessage);
			// We maintain this redundant field mainly for compatibility with older versions of SayMore and
			// other programs (e.g., SayMoreJs) that use the file format. There may also still be a few
			// things in SayMore itself which expect it to exist, though we are trying to eliminate them.
			_file.SetValue(SessionFileType.kParticipantsFieldName, GetParticipants(false), out failureMessage);
			_file.Save();

			var frm = FindForm();
			if (frm == null)
				return;

			// Set the people list whenever changes happen in Contributors list
			// We want to display these with their roles.
			foreach (var editor in Program.GetControlsOfType<SessionBasicEditor>(Program.ProjectWindow))
				editor.SetPeople(GetParticipants(true));

			if (failureMessage != null)
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		/// --------------------------------------------------------------------------------------
		/// Get the participants list from the Sessions contributions
		/// --------------------------------------------------------------------------------------
		private string GetParticipants(bool withRoles)
		{
			var participants = new StringBuilder();
			var names = new HashSet<string>();
			foreach (Contribution contributor in _model.Contributions)
			{
				if (!withRoles && names.Contains(contributor.ContributorName))
					continue;
				names.Add(contributor.ContributorName);
				if (participants.Length > 0)
					participants.Append("; ");
				participants.Append(contributor.ContributorName);
				if (withRoles)
					participants.Append(" (").Append(contributor.Role.Name).Append(")");
			}

			return participants.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				TabText = LocalizationManager.GetString(
					"CommonToMultipleViews.ContributorsEditor.TabText", "Contributors");
			}

			base.HandleStringsLocalized(lm);
		}
	}
}
