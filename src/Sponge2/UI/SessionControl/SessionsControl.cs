using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SilUtils;
using Sponge2.Model;

namespace Sponge2.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionsControl : UserControl
	{
		private readonly SessionsViewModel _model;

		/// ------------------------------------------------------------------------------------
		public SessionsControl(SessionsViewModel presentationModel)
		{
			_model = presentationModel;
			InitializeComponent();

			if (DesignMode)
				return;

			_componentEditorsTabControl.Font = SystemFonts.IconTitleFont;
			_componentGrid.Font = SystemFonts.IconTitleFont;
			LoadSessionList();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadSessionList()
		{
			var sessions = _model.Sessions.Cast<object>().ToList();

			_sessionsListPanel.AddRange(sessions);

			if (sessions.Count > 0)
				_sessionsListPanel.SelectItem(sessions[0], true);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateComponentList()
		{
			// I think there's a bug in the grid that fires the cell value needed event in
			// the process of changing the row count but it fires it for rows that are
			// no longer supposed to exist. This tends to happen when the row count was
			// previously higher than the new value.
			_componentGrid.CellValueNeeded -= HandleComponentFileGridCellValueNeeded;
			_componentGrid.RowCount = _model.ComponentsOfSelectedSession.Count();
			_componentGrid.CellValueNeeded += HandleComponentFileGridCellValueNeeded;

			_componentGrid.Invalidate();
			UpdateComponentEditors();


			//TODO: editor tab (for now, just the first page) isn't currently
			//being displayed, even though the first component shows as highlighted.
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateComponentEditors()
		{
			Utils.SetWindowRedraw(_componentEditorsTabControl, false);

			// GRRR!!! This turns out to steal focus, giving it to the tab control. I think
			// what we're going to have to do is reuse at least one page because removing
			// all but one tab page prevents the problem.

			_componentEditorsTabControl.Selecting -= HandleSelectedComponentEditorTabSelecting;
			_componentEditorsTabControl.TabPages.Clear();

			// At this point, we're just making the tabs and naming them,
			//	so that the entire controls
			// don't need to be build until the user actually tabs to them

			foreach (var provider in _model.GetComponentEditorProviders())
			{
				var page = new ComponentEditorTabPage(provider);
				_componentEditorsTabControl.TabPages.Add(page);

				if (_componentEditorsTabControl.TabPages.Count == 1)
					LoadComponentEditorsForTabPage(page);
			}

			_componentEditorsTabControl.Selecting += HandleSelectedComponentEditorTabSelecting;
			Utils.SetWindowRedraw(_componentEditorsTabControl, true);
		}

		/// ------------------------------------------------------------------------------------
		private object HandleNewSessionButtonClicked(object sender)
		{
			_model.SetSelectedSession(_model.CreateNewSession());
			_sessionsListPanel.AddItem(_model.SelectedSession, true, true);
			return null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSelectedSessionChanged(object sender, object newItem)
		{
			_model.SetSelectedSession(newItem as Session);
			UpdateComponentList();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			// This event is fired even when the grid gains focus without the row actually
			// changing, therefore we should just ignore the event when the row hasn't changed.
			if (e.RowIndex != _componentGrid.CurrentCellAddress.Y)
			{
				_model.SetSelectedComponentFile(e.RowIndex);
				UpdateComponentEditors();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _componentGrid.Columns[e.ColumnIndex].DataPropertyName;
			var currSessionFile = _model.GetComponentFile(e.RowIndex);

			e.Value = (currSessionFile == null ? null :
				ReflectionHelper.GetProperty(currSessionFile, dataPropName));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAfterSessionAdded(object sender, object itemBeingAdded)
		{

		}

		/// ------------------------------------------------------------------------------------
		private void HandleSelectedComponentEditorTabSelecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Selecting)
				LoadComponentEditorsForTabPage(e.TabPage as ComponentEditorTabPage);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadComponentEditorsForTabPage(ComponentEditorTabPage page)
		{
			if (page != null && !page.AreEditorControlsLoaded)
			{
				var control = page.EditorProvider.GetEditor(_model.SelectedComponentFile);
				control.Dock = DockStyle.Fill;
				_componentEditorsTabControl.SelectedTab.Controls.Add(control);
				page.AreEditorControlsLoaded = true;
			}
		}
	}
}
