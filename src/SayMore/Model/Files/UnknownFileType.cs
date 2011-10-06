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

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new BrowserEditor(file, null);
			yield return _basicFieldGridEditorFactoryLazy()(file, null);
			yield return _contributorsEditorFactoryLazy()(file, null);
			yield return new NotesEditor(file);
		}
	}
}