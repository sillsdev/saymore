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
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new BrowserEditor(file), "View"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
			}

			return _providers;
		}
	}
}