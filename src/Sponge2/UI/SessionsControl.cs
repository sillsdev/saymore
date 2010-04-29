using System;
using System.IO;
using System.Windows.Forms;
using Sponge2.Model;

namespace Sponge2.UI
{
	public partial class SessionsControl : UserControl
	{
		private readonly SessionsViewModel _model;

		public SessionsControl(SessionsViewModel presentationModel)
		{
			_model = presentationModel;
			InitializeComponent();
			//label1.Text = _model.TestLabel;
		}

		private void SessionsControl_Load(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			//like everything here, this is just a hack
			_componentsListView.Items.Clear();
			_sessionListView.Items.Clear();
			foreach (Session  session in _model.Sessions)
			{
				ListViewItem item = _sessionListView.Items.Add(session.Id);
				var x = item.Tag = session;
				if(x == _model.SelectedSession)
				{
					item.Selected = true;
				}

			}
		}

		private void UpdateComponentList()
		{
			_componentsListView.Items.Clear();
			foreach (ComponentFile file in _model.ComponentsOfCurrentSession)
			{
				ListViewItem item = _componentsListView.Items.Add(Path.GetFileName(file.Path));
				item.Tag = file;
			}
		}

		private void OnNewSessionButtonClick(object sender, EventArgs e)
		{
			_model.CreateNewSession();
			UpdateDisplay();
		}

		private void OnSessionListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_sessionListView.SelectedItems.Count > 0)
			{
				_model.SelectedSession = _sessionListView.SelectedItems[0].Tag as Session;
			}
			UpdateComponentList();
		}
	}
}
