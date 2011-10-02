using System;
using System.Collections.Generic;
using SayMore.UI.ComponentEditors;

namespace SayMore.Model.Files
{
	/// <summary>
	/// This is the type assigned to files which we don't recognize
	/// </summary>
	public class UnknownFileType : FileType
	{
		protected readonly Func<ContributorsEditor.Factory> _contributorsEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public UnknownFileType(
			Func<BasicFieldGridEditor.Factory> basicFieldGridEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Unknown", path => false)
		{
			_basicFieldGridEditorFactoryLazy = basicFieldGridEditorFactoryLazy;
			_contributorsEditorFactoryLazy = contributorsEditorFactoryLazy;
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
			var text = Program.GetString("Model.Files.UnknownFileType.ViewTabText", "View");
			yield return new BrowserEditor(file, text, null);

			yield return _basicFieldGridEditorFactoryLazy()(file, GetPropertiesTabText(), null);
			yield return _contributorsEditorFactoryLazy()(file, GetContributionsTabText(), null);
			yield return new NotesEditor(file, GetNotesTabText());
		}
	}
}