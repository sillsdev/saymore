using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Container for the sessions view.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SessionsVw : BaseSplitVw
	{
		private SpongeProject m_currProj;
		private Session m_currSession;
		private SessionFile m_currSessionFile;
		private SessionFile[] m_currSessionFiles;
		private bool m_refreshNeeded;

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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionsVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionsVw(SpongeProject m_prj) : this()
		{
			m_currProj = (m_prj ?? MainWnd.CurrentProject);
			m_currProj.ProjectChanged += HandleProjectFoldersChanged;
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
			return (m_currSessionFiles == null || i < 0 ||
				i >= m_currSessionFiles.Count() ? null : m_currSessionFiles[i]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			m_currProj.ProjectChanged -= HandleProjectFoldersChanged;

			// Persist the width of each column on the files tab.
			var colWidths = new int[gridFiles.Columns.Count];
			for (int i = 0; i < gridFiles.ColumnCount; i++)
				colWidths[i] = gridFiles.Columns[i].Width;

			Settings.Default.SessionFileCols =
				PortableSettingsProvider.GetStringFromIntArray(colWidths);

			Settings.Default.SessionVwSplitterPos = splitOuter.SplitterDistance;
			Settings.Default.SessionFileInfoPanelSplitterPos = m_infoPanel.SplitterPosition;
			Settings.Default.Save();

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
			UpdateSessionFileInfo(CurrentSessionFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle adding a new session via clicking on the new button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object lpSessions_NewButtonClicked(object sender)
		{
			using (var dlg = new NewSessionDlg(m_currProj.ProjectPath))
			{
				if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
				{
					var newSession = m_currProj.AddSession(dlg.NewSessionName);
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
			bool enableFileWatchingState = m_currProj.EnableFileWatching;
			m_currProj.EnableFileWatching = false;

			foreach (Session session in itemsToDelete)
				Directory.Delete(session.FullPath, true);

			m_currProj.EnableFileWatching = enableFileWatchingState;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the selected session changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lpSessions_SelectedItemChanged(object sender, object newItem)
		{
			if (newItem != m_currSession)
			{
				RefreshFileList();
				UpdateSessionFileInfo(CurrentSessionFile);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the size of the empty session message and move the session folder link
		/// accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlGrid_SizeChanged(object sender, EventArgs e)
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
		/// Handles the MoreActionButtonClicked event of the m_infoPanel control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_infoPanel_MoreActionButtonClicked(object sender, EventArgs e)
		{
			var btn = sender as Control;
			if (btn == null)
				return;

			var pt = btn.PointToScreen(new Point(0, btn.Height));
			cmnuMoreActions.Show(pt);
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
			m_currSessionFile = sessionFile;

			if (sessionFile == null || tabSessions.SelectedTab != tpgFiles)
			{
				m_infoPanel.Visible = false;
				return;
			}

			m_infoPanel.Icon = sessionFile.LargeIcon;
			m_infoPanel.FileName = sessionFile.FileName;
			m_infoPanel.Notes = sessionFile.Notes;
			m_infoPanel.Initialize(sessionFile.Data);
			m_infoPanel.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the current session file info.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveCurrentSessionFileInfo()
		{
			if (m_currSessionFile != null)
			{
				m_infoPanel.Save(m_currSessionFile.Data);
				m_currSessionFile.Notes = m_infoPanel.Notes;
				m_currSessionFile.Save();
				m_currSessionFile = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open current session's folder in the OS' file manager. ENHANCE: After opening
		/// the file manager, it would be nice to select the current session file, but that
		/// appears to be harder to accomplish, so I leave that for a future exercise.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuOpenInFileManager_Click(object sender, EventArgs e)
		{
			if (m_currSession != null)
				Process.Start(m_currSession.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open current session file in its associated application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void openInApp_Click(object sender, EventArgs e)
		{
			var sessionFile = CurrentSessionFile;
			if (sessionFile != null)
			{
				var path = Path.Combine(m_currSession.FullPath, sessionFile.FileName);
				try
				{
					Process.Start(path);
				}
				catch (Win32Exception)
				{
					// REVIEW: Is it OK to assume any Win32Exception is no application association?
					Utils.MsgBox(
						string.Format("No application is associated with {0}", sessionFile.FileName));
				}
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

			switch (e.AllowedEffect)
			{
				case DragDropEffects.Copy: m_currSession.AddFiles(droppedFiles); break;
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
			if (!m_isViewActive)
			{
				// The view is not active so don't update it yet. Set a flag so that when
				// the view does become active again, it will be Wait until the view
				// becomes active again.
				m_refreshNeeded = true;
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
			lpSessions.Items = m_currProj.Sessions.ToArray();
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
			m_currSession = lpSessions.CurrentItem as Session;
			m_currSessionFiles = (m_currSession == null ? null :
				(from x in m_currSession.Files
				 select SessionFile.Create(x)).ToArray());

			lblEmptySessionMsg.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length == 0);
			lnkSessionPath.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length == 0);
			gridFiles.Visible = (m_currSessionFiles != null && m_currSessionFiles.Length > 0);

			if (m_currSessionFiles != null)
			{
				gridFiles.RowCount = m_currSessionFiles.Length;
				lnkSessionPath.Text = m_currSession.FullPath;
			}

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

			if (m_refreshNeeded || firstTime)
			{
				RefreshSessionList();
				m_refreshNeeded = false;
			}

			if (firstTime)
			{
				if (Settings.Default.SessionVwSplitterPos > 0)
					splitOuter.SplitterDistance = Settings.Default.SessionVwSplitterPos;

				if (Settings.Default.SessionFileInfoPanelSplitterPos > 0)
					m_infoPanel.SplitterPosition = Settings.Default.SessionFileInfoPanelSplitterPos;

				var colWidths = PortableSettingsProvider.GetIntArrayFromString(
					Settings.Default.SessionFileCols);

				for (int i = 0; i < colWidths.Length && i < gridFiles.ColumnCount; i++)
					gridFiles.Columns[i].Width = colWidths[i];

				// Setting up drap/drop crashes when there is no hosting form,
				// so make sure we're being hosted before doing so.
				if (FindForm() != null)
				{
					pnlGrid.AllowDrop = true;
					pnlGrid.DragOver += FileListDragOver;
					pnlGrid.DragDrop += FileListDragDrop;
				}
			}
		}

		#endregion
	}
}
