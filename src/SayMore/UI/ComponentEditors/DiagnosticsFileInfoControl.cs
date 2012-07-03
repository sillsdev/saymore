using System;
using System.Linq;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// <summary>
	/// This is just for development purposes. The idea is to just show all fields
	/// </summary>
	public partial class DiagnosticsFileInfoControl : EditorBase
	{
		public DiagnosticsFileInfoControl(ComponentFile file)
		{
			InitializeComponent();
			Name = "diagnostics";
			TabText = "Info";

			foreach (var field in file.StandardMetaDataFieldValues)
				textBox1.Text += Environment.NewLine + field.FieldId + ": " + field.ValueAsString;
			if (file.CustomMetaDataFieldValues.Any())
			{
				textBox1.Text += Environment.NewLine + Environment.NewLine + "Custom fields: ";

				foreach (var field in file.CustomMetaDataFieldValues)
					textBox1.Text += Environment.NewLine + field.FieldId + ": " + field.ValueAsString;
			}
		}
	}
}
