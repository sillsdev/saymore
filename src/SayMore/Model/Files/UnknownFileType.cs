using System.Collections.Generic;
using SayMore.UI.ComponentEditors;

namespace SayMore.Model.Files
{
	/// <summary>
	/// This is the type assigned to files which we don't recognize
	/// </summary>
	public class UnknownFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public UnknownFileType() : base("Unknown", path => true)
		{
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count == 0)
			{
				_editors.Add(new BrowserEditor(file, "View", null));
				_editors.Add(new NotesEditor(file, "Notes", "Notes"));
				_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}
			else
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}

			return _editors;
		}
	}
}