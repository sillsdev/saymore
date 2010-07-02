using System;
using System.Collections.Generic;
using SayMore.UI.ComponentEditors;
using SIL.Localization;

namespace SayMore.Model.Files
{
	/// <summary>
	/// This is the type assigned to files which we don't recognize
	/// </summary>
	public class UnknownFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public UnknownFileType(Func<BasicFieldGridEditor.Factory> basicFieldGridEditorFactoryLazy)
			: base("Unknown", path => false)
		{
			_basicFieldGridEditorFactoryLazy = basicFieldGridEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsForUnknownFileTypes
		{
			get { return true; }
		}

		///// ------------------------------------------------------------------------------------
		//public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		//{
		//    if (_editors.Count > 0)
		//    {
		//        foreach (var editor in _editors)
		//            editor.SetComponentFile(file);
		//    }
		//    else
		//    {
		//        var text = LocalizationManager.LocalizeString("MiscFileInfoEditor.ViewTabText", "View");
		//        _editors.Add(new BrowserEditor(file, text, null));

		//        text = LocalizationManager.LocalizeString("MiscFileInfoEditor.PropertiesTabText", "Properties");
		//        _editors.Add(_basicFieldGridEditorFactoryLazy()(file, text, null));

		//        text = LocalizationManager.LocalizeString("MiscFileInfoEditor.NotesTabText", "Notes");
		//        _editors.Add(new NotesEditor(file, text, "Notes"));

		//        //_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
		//    }

		//    return _editors;
		//}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("MiscFileInfoEditor.ViewTabText", "View");
			yield return new BrowserEditor(file, text, null);

			text = LocalizationManager.LocalizeString("MiscFileInfoEditor.PropertiesTabText", "Properties");
			yield return _basicFieldGridEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("MiscFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));

		}
	}
}