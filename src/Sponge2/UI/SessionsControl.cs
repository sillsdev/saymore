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

			_sessionComponentTab.Font = SystemFonts.IconTitleFont;
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
			_componentGrid.RowCount = _model.ComponentsOfSelectedSession.Count();
			_componentGrid.Invalidate();
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
		private void _componentGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			_model.SetSelectedComponentFile(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		private void _componentGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _componentGrid.Columns[e.ColumnIndex].DataPropertyName;
			var currSessionFile = _model.GetComponentFile(e.RowIndex);

			e.Value = (currSessionFile == null ? null :
				ReflectionHelper.GetProperty(currSessionFile, dataPropName));
		}

		private void HandleAfterSessionAdded(object sender, object itemBeingAdded)
		{

		}
	}
}
