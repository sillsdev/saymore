using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Dialogs;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SIL.Sponge.Utilities;
using SilUtils;

namespace SIL.Sponge.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Container for the sessions view.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SessionsVw : BaseSplitVw
	{
		private readonly Func<IEnumerable<string>> _peopleNameProvider;
		private readonly SpongeProject _currProj;
		private Session _currSession;
		private SessionFile _currSessionFile;
		private SessionFile[] _currSessionFiles;
		private bool _refreshNeeded;
		private readonly string _unknownEventType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionsVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionsVw()
		{
			InitializeComponent();
			UpdateSessionFileInfo(null);

			gridFiles.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridFiles.DefaultCellStyle.BackColor, 10);

			gridFiles.Dock = DockStyle.Fill;

			lblNoSessionsMsg.BackColor = lpSessions.ListView.BackColor;

			_fileInfoNotes.Width = pnlFileInfoNotes.ClientSize.Width - _fileInfoNotes.Left - 1;
			_fileInfoNotes.Height = pnlFileInfoNotes.ClientSize.Height - _fileInfoNotes.Top - 1;

			tblDescription.Paint += SpongeColors.PaintDataEntryBackground;
			splitFileTab.Panel2.Paint += SpongeColors.PaintDataEntryBorder;
			splitFileTab.Panel2.BackColor = SpongeColors.DataEntryPanelBegin;
			_infoPanel.LabeledTextBoxBackgroundColor = SpongeColors.DataEntryPanelBegin;
			_fileInfoNotes.BackColor = SpongeColors.DataEntryPanelBegin;

			if (Sponge.DiscourseTypes != null)
			{
				_unknownEventType = LocalizationManager.LocalizeString("SessionsVw.UnknownEventType",
					"<Unknown>", "Unknown event type displayed in the event type drop-down list.",
					"Views", LocalizationCategory.Other, LocalizationPriority.High);

				_eventType.Items.AddRange(Sponge.DiscourseTypes.ToArray());
				_eventType.Items.Add(_unknownEventType);
			}

			btnNewFromFiles.Parent.Controls.Remove(btnNewFromFiles);
			lpSessions.InsertButton(1, btnNewFromFiles);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionsVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionsVw(SpongeProject _prj, Func<IEnumerable<string>> peopleNameProvider) : this()
		{
			_peopleNameProvider = peopleNameProvider;
			_currProj = (_prj ?? MainWnd.CurrentProject);
			_currProj.ProjectChanged += HandleProjectFoldersChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session file for the current row in the grid on the files tab for the
		/// current session.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFile CurrentSessionFile
		{
			get { return GetSessionFile(gridFiles.CurrentCellAddress.Y); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session file for the specified index from the current session's file list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFile GetSessionFile(int i)
		{
			return (_currSessionFiles == null || i < 0 ||
				i >= _currSessionFiles.Count() ? null : _currSessionFiles[i]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			_currProj.ProjectChanged -= HandleProjectFoldersChanged;

			Settings.Default.SessionFileCols =
				Sponge.StoreGridColumnWidthsInString(gridFiles);

			Settings.Default.SessionVwSplitterPos = splitOuter.SplitterDistance;
			Settings.Default.Save();

			SaveChangesToSession(_currSession);

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This fixes a paint error in .Net that manifests itself when tab controls are
		/// resized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabSessions_SizeChanged(object sender, EventArgs e)
		{
			tabSessions.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Selected event of the tabSessions control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabSessions_Selected(object sender, TabControlEventArgs e)
		{
			if (e.TabPage == tpgDescription && e.Action == TabControlAction.Deselecting &&
				!SaveChangesToSession(_currSession))
			{
				tabSessions.Selected -= tabSessions_Selected;
				tabSessions.SelectedTab = tpgDescription;
				tabSessions.Selected += tabSessions_Selected;
				return;
			}

			if (e.Action == TabControlAction.Selected)
				LoadTabPage(e.TabPage);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the selected session changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lpSessions_SelectedItemChanged(object sender, object newItem)
		{
			if (newItem != _currSession)
			{
				if (_currSession != null && !SaveChangesToSession(_currSession))
				{
					lpSessions.SelectItem(_currSession, false);
					return;
				}

				_currSession = newItem as Session;
				LoadTabPage(tabSessions.SelectedTab);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the selected tab page with relevant session data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadTabPage(TabPage tpg)
		{
			if (tpg == tpgDescription)
				LoadDescriptionTabFromSession(_currSession);
			else
			{
				SaveChangesToSession(_currSession);

				if (tpg == tpgFiles)
				{
					RefreshFileList();
					UpdateSessionFileInfo(CurrentSessionFile);
				}
			}
		}

		#region Methods for adding and deleting sessions
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle adding a new session via clicking on the new button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object lpSessions_NewButtonClicked(object sender)
		{
			using (var dlg = new NewSessionDlg(_currProj.Folder, _currProj.IsoCode))
			{
				if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
				{
					var newSession = _currProj.AddSession(dlg.NewSessionName);
					newSession.AddFiles(dlg.SessionFiles);
					lblNoSessionsMsg.Visible = false;
					lpSessions.ListView.Focus();
					return newSession;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle deleting a session via clicking on the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool BeforeSessionsDeleted(object sender, List<object> itemsToDelete)
		{
			if (itemsToDelete == null || itemsToDelete.Count == 0)
				return false;

			var msg = LocalizationManager.LocalizeString("SessionsVw.DeleteConfirmationMsg",
				"Are you sure you want to delete the selected sessions and their content?",
				"Question asked when delete button is clicked on the 'Files' tab of the Sessions view.",
				"Views", LocalizationCategory.GeneralMessage, LocalizationPriority.High);

			return (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle deleting sessions from the disk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AfterSessionsDeleted(object sender, List<object> itemsToDelete)
		{
			bool enableFileWatchingState = _currProj.EnableFileWatching;
			_currProj.EnableFileWatching = false;

			foreach (Session session in itemsToDelete)
				Directory.Delete(session.Folder, true);

			_currProj.EnableFileWatching = enableFileWatchingState;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the size of the empty session message and move the session folder link
		/// accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFileListPanelSizeChanged(object sender, EventArgs e)
		{
			using (var g = lblEmptySessionMsg.CreateGraphics())
			{
				var sz = new Size(lblEmptySessionMsg.Width, int.MaxValue);
				var dy = TextRenderer.MeasureText(g, lblEmptySessionMsg.Text,
					lblEmptySessionMsg.Font, sz, TextFormatFlags.WordBreak).Height;

				lblEmptySessionMsg.Height = dy;
				lnkSessionPath.Top = lblEmptySessionMsg.Bottom + 5;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the MoreActionButtonClicked event of the _infoPanel control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _infoPanel_MoreActionButtonClicked(object sender, EventArgs e)
		{
			var btn = sender as Control;
			if (btn == null)
				return;

			var pt = btn.PointToScreen(new Point(0, btn.Height));
			_fileContextMenu.Items.Clear();
			_fileContextMenu.Items.AddRange(CurrentSessionFile.GetContextMenuItems(_id.Text).ToArray());
			_fileContextMenu.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Start an OS process to browse the file system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkSessionPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(lnkSessionPath.Text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the CellValueNeeded event of the gridFiles control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridFiles_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var currSessionFile = GetSessionFile(e.RowIndex);

			if (currSessionFile == null)
				e.Value = null;
			else
			{
				e.Value = ReflectionHelper.GetProperty(currSessionFile,
					gridFiles.Columns[e.ColumnIndex].DataPropertyName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the CellValuePushed event of the gridFiles control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridFiles_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			var currSessionFile = GetSessionFile(e.RowIndex);

			if (currSessionFile != null)
				currSessionFile.Tags = e.Value as string;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the RowEnter event of the gridFiles control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridFiles_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			// This event gets fired even when the grid gets focus when the user clicks
			// on the row that is already current. In that case we don't want to bother
			// doing anything, therefore we'll ignore that situation.
			if (e.RowIndex != gridFiles.CurrentCellAddress.Y)
				UpdateSessionFileInfo(GetSessionFile(e.RowIndex));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the session file info.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateSessionFileInfo(SessionFile sessionFile)
		{
			SaveCurrentSessionFileInfo();

			if (sessionFile == null || tabSessions.SelectedTab != tpgFiles)
			{
				_infoPanel.Visible = false;
				return;
			}

			_infoPanel.Icon = sessionFile.LargeIcon;
			_infoPanel.FileName = sessionFile.FileName;
			_fileInfoNotes.Text = sessionFile.Notes;
			_infoPanel.Initialize(sessionFile.Data);
			_infoPanel.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the current session file info.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveCurrentSessionFileInfo()
		{
			if (_currSessionFile != null)
			{
				_infoPanel.Save(_currSessionFile.Data);
				_currSessionFile.Notes = _fileInfoNotes.Text.Trim();
				_currSessionFile.Save();
				_currSessionFile = null;
			}
		}

		#region Methods for dragging and dropping files on files tab
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Files the list drag drop.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FileListDragDrop(object sender, DragEventArgs e)
		{
			var droppedFiles = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (droppedFiles == null)
				return;

			// REVIEW: What should we do when dragging a folder? Should session folders be
			// allowed to contain subfolders?

			switch (e.Effect)
			{
				case DragDropEffects.Copy: _currSession.AddFiles(droppedFiles); break;
				//case DragDropEffects.Move: TODO: Handle move when dropping folders.
				default: return;
			}

			RefreshFileList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the DragOver event of the pnlGrid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FileListDragOver(object sender, DragEventArgs e)
		{
			const int leftMouse = 1;
			//const int shift = 4 + leftMouse;
			//const int ctrl = 8 + leftMouse;

			if (!e.Data.GetDataPresent(DataFormats.FileDrop) || (e.KeyState & leftMouse) == 0 ||
				lpSessions.CurrentItem == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			// TODO: Uncomment when move is supported.
			//e.Effect = (e.KeyState == shift ? DragDropEffects.Move : DragDropEffects.Copy);
			e.Effect = DragDropEffects.Copy;
		}

		#endregion

		#region Methods for refreshing sessions and sessions file lists
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fired when there's a change in the files or folders of the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleProjectFoldersChanged(object sender, EventArgs e)
		{
			if (!_isViewActive)
			{
				// The view is not active so don't update it yet. Set a flag so that when
				// the view does become active again, it will be Wait until the view
				// becomes active again.
				_refreshNeeded = true;
				return;
			}

			SaveCurrentSessionFileInfo();
			RefreshSessionList();
			RefreshFileList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the list of sessions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshSessionList()
		{
			lpSessions.Items = _currProj.Sessions.ToArray();
			lblNoSessionsMsg.Visible = (lpSessions.Items.Length == 0);

			// RefreshFileList will be called automatically when there are some sessions
			// (as a result of setting the lpSessions.Items property above). However, when
			// there are no sessions, it won't be called automatically so we need to force
			// the issue here.
			if (lpSessions.Items.Length == 0)
				RefreshFileList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the file list for the current session.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshFileList()
		{
			Utils.SetWindowRedraw(tabSessions, false);

			// TODO: keep track of currently selected file and try to restore
			// that after rebuilding the list.
			_currSessionFiles = (_currSession == null ? null :
				(from x in _currSession.Files
				 select SessionFile.Create(x)).ToArray());

			lblEmptySessionMsg.Visible = (_currSessionFiles != null && _currSessionFiles.Length == 0);
			splitFileTab.Panel2Collapsed = lblEmptySessionMsg.Visible;
			gridFiles.Visible = (_currSessionFiles != null && _currSessionFiles.Length > 0);
			lnkSessionPath.Visible = (_currSessionFiles != null && _currSessionFiles.Length == 0);
			lnkSessionPath.Text = (_currSession != null && _currSession.Folder != null ?
				_currSession.Folder : string.Empty);

			if (_currSessionFiles != null)
				gridFiles.RowCount = _currSessionFiles.Length;

			Utils.SetWindowRedraw(tabSessions, true);
		}

		#endregion

		#region Overrides
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when views is activated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);

			if (_refreshNeeded || firstTime)
			{
				RefreshSessionList();
				_refreshNeeded = false;
			}

			if (firstTime)
			{
				if (Settings.Default.SessionVwSplitterPos > 0)
					splitOuter.SplitterDistance = Settings.Default.SessionVwSplitterPos;

				Sponge.SetGridColumnWidthsFromString(gridFiles, Settings.Default.SessionFileCols);

				// Setting up drap/drop crashes when there is no hosting form,
				// so make sure we're being hosted before doing so.
				if (FindForm() != null)
				{
					splitFileTab.AllowDrop = true;
					splitFileTab.DragOver += FileListDragOver;
					splitFileTab.DragDrop += FileListDragDrop;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the date field is valid. If it is, then save any outstanding changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			if (_currSession != null && !SaveChangesToSession(_currSession))
				return false;

			return base.IsOKToLeaveView(showMsgWhenNotOK);
		}

		#endregion

		#region Methods for moving data back and forth between session object and form fields
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the fields on the description tab from the specified session.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDescriptionTabFromSession(Session session)
		{
			tblDescription.Enabled = (session != null);
			mmScroll.Clear();

			if (session == null)
			{
				ClearDescriptionTab();
				return;
			}

			_id.Text = session.Id;
			_date.Value = session.Date;
			_title.Text = session.Title;
			_participants.Text = session.Participants;
			_access.Text = session.Access;
			_setting.Text = session.Setting;
			_location.Text = session.Location;
			_situation.Text = session.Situation;
			_synopsis.Text = session.Synopsis;

			if (session.EventType == null)
				_eventType.SelectedItem = _unknownEventType;
			else
				_eventType.SelectedItem = session.EventType;

			mmScroll.AddFiles(_currSession.Files);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified session from the fields on the description tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool SaveChangesToSession(Session session)
		{
			if (session == null)
				return false;

			session.Date = _date.Value;
			session.Title = _title.Text.Trim();
			session.Participants = _participants.Text.Trim();
			session.Access = _access.Text.Trim();
			session.Setting = _setting.Text.Trim();
			session.Location = _location.Text.Trim();
			session.Situation = _situation.Text.Trim();
			session.Synopsis = _synopsis.Text;
			session.EventTypeId = ((_eventType.SelectedItem as string) == _unknownEventType ?
				null : ((DiscourseType)_eventType.SelectedItem).Id);

			return session.ChangeIdAndSave(_id.Text.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the fields on the description tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearDescriptionTab()
		{
			_id.Text = string.Empty;
			_date.Text = string.Empty;
			_title.Text = string.Empty;
			_participants.Text = string.Empty;
			_eventType.SelectedItem = _unknownEventType;
			_access.Text = string.Empty;
			_setting.Text = string.Empty;
			_location.Text = string.Empty;
			_situation.Text = string.Empty;
			_synopsis.Text = string.Empty;
			mmScroll.Clear();
		}

		#endregion

		#region Methods for providing auto-complete (type-ahead) support for some fields.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the auto complete list for the access field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _access_Enter(object sender, EventArgs e)
		{
			var list = (from x in _currProj.Sessions
						orderby x.Access
						select x.Access).ToArray();

			LoadAutoCompleteList(sender as TextBox, list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the auto complete list for the setting field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _setting_Enter(object sender, EventArgs e)
		{
			var list = (from x in _currProj.Sessions
						orderby x.Setting
						select x.Setting).ToArray();

			LoadAutoCompleteList(sender as TextBox, list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the auto complete list for the location field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _location_Enter(object sender, EventArgs e)
		{
			var list = (from x in _currProj.Sessions
						orderby x.Location
						select x.Location).ToArray();

			LoadAutoCompleteList(sender as TextBox, list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified auto complete list for the specified text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void LoadAutoCompleteList(TextBox txtBox, string[] list)
		{
			var acsc = new AutoCompleteStringCollection();
			acsc.AddRange(list);
			txtBox.AutoCompleteCustomSource = acsc;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open new sessions from files dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnNewFromFiles_Click(object sender, EventArgs e)
		{
			using (var viewModel = new NewSessionsFromFileDlgViewModel())
			using (var dlg = new NewSessionsFromFilesDlg(viewModel))
			{
				viewModel.Dialog = dlg;
				_currProj.EnableFileWatching = false;

				if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
				{
					_currProj.RefreshSessionList();
					RefreshSessionList();
					lpSessions.CurrentItem = viewModel.FirstNewSessionAdded;
				}

				_currProj.EnableFileWatching = true;
				FindForm().Focus();
			}
		}

		private void HandleFilesGridCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				gridFiles.CurrentCell = gridFiles.Rows[e.RowIndex].Cells[e.ColumnIndex];

			 //   _infoPanel_MoreActionButtonClicked(null, null);
				Point pt = gridFiles.PointToClient(MousePosition);
				_fileContextMenu.Items.Clear();
				_fileContextMenu.Items.AddRange(CurrentSessionFile.GetContextMenuItems(_id.Text).ToArray());
				_fileContextMenu.Show(gridFiles, pt);
			}
		}

		private void HandleParticipants_Enter(object sender, EventArgs e)
		{
			_participants.AutoCompleteCustomSource = new AutoCompleteStringCollection();
			_participants.AutoCompleteCustomSource.AddRange(_peopleNameProvider().ToArray());
		}

		private void Handle_IdValidating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = !_currSession.ChangeIdAndSave(_id.Text.Trim());
			if (e.Cancel)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Please use a different id.");
		}

		private void HandleFileGridCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			gridFiles.CurrentCell = gridFiles.Rows[e.RowIndex].Cells[e.ColumnIndex];
			CurrentSessionFile.HandleOpenInApp_Click(this,null);
		}
	}
}
