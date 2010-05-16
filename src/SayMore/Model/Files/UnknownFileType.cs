using System.Collections.Generic;
using SayMore.UI.ComponentEditors;

namespace SayMore.Model.Files
{
	/// <summary>
	/// This is the type assigned to files which we don't recognize
	/// </summary>
	public class UnknownFileType : FileType
	{
		public UnknownFileType()
			: base("Unknown", path => true)
		{
		}

		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			yield return new EditorProvider(new DiagnosticsFileInfoControl(file), "Info");
		}

		//public override IEnumerable<FileCommand> Commands
		//{
		//    get
		//    {
		//        foreach (var command in base.Commands)
		//        {
		//            yield return command;
		//        }
		//        //yield return new FileCommand("Open in Program Associated with this File...", FileCommand.HandleOpenInApp_Click);
		//    }
		//}
	}
}