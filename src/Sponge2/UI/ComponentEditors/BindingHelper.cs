using System.ComponentModel;
using System.Windows.Forms;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	public class BindingHelper
	{
		private readonly ComponentFile _file;

		public BindingHelper(ComponentFile componentFile)
		{
			_file = componentFile;
		}

		public void BindTextBox(TextBox box, string key)
		{
			box.Text = _file.GetStringValue(key, string.Empty);
			box.Tag = key;
			box.Validating +=OnValidatingTextBox;
		}

		private void OnValidatingTextBox(object sender, CancelEventArgs e)
		{
			var control = (Control) sender;
			_file.SetValue((string)control.Tag, control.Text.Trim());

			//enchance: don't save so often, leave it to some higher level

			_file.Save();
		}
	}
}