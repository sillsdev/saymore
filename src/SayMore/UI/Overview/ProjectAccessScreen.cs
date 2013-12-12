
using System.Windows.Forms;
using SIL.Archiving.Generic.AccessProtocol;

namespace SayMore.UI.Overview
{
	public partial class ProjectAccessScreen : UserControl
	{
		public ProjectAccessScreen()
		{
			InitializeComponent();

			// acess protocol list
			var protocols = AccessProtocols.Load();
			protocols.Insert(0, new ArchiveAccessProtocol { ProtocolName = "None" });
			_projectAccess.DataSource = protocols;

			SizeProtocolsComboBox(_projectAccess);
		}

		private void SizeProtocolsComboBox(ComboBox comboBox)
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

		private void _projectAccess_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_projectAccess.SelectedItem == null) return;

			var item = (ArchiveAccessProtocol) _projectAccess.SelectedItem;
			_labelDescription.Text = item.DocumentationFile;
		}

		private void ProjectAccessScreen_Leave(object sender, System.EventArgs e)
		{
			// check for changes
			var changed = false;
			var project = Program.CurrentProject;

			if (_projectAccess.Text != project.AccessProtocol)
				changed = true;

			if (!changed) return;

			// save changes
			project.AccessProtocol = _projectAccess.Text;
			project.Save();
		}
	}
}
