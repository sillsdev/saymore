using System;
using System.Windows.Forms;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	/// <summary>
	/// THis is just for development purposes. The idea is to just show all fields
	/// </summary>
	public partial class DiagnosticsFileInfoControl : UserControl
	{

		public DiagnosticsFileInfoControl(ComponentFile file)
		{
			InitializeComponent();
			Name = "diagnostics";
			foreach (var field in file.MetaDataFieldValues)
			{
				textBox1.Text += Environment.NewLine + field.FieldDefinitionKey + ": " + field.Value;
			}
		}
	}
}
