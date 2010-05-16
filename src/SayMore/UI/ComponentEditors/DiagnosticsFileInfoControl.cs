using System;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
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
