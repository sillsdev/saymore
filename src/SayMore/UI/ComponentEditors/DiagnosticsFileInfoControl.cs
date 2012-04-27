using System;
using SayMore.Model.Files;

namespace SayMore.Utilities.ComponentEditors
{
	/// <summary>
	/// THis is just for development purposes. The idea is to just show all fields
	/// </summary>
	public partial class DiagnosticsFileInfoControl : EditorBase
	{
		public DiagnosticsFileInfoControl(ComponentFile file)
		{
			InitializeComponent();
			Name = "diagnostics";
			TabText = "Info";

			foreach (var field in file.MetaDataFieldValues)
				textBox1.Text += Environment.NewLine + field.FieldId + ": " + field.ValueAsString;
		}
	}
}
