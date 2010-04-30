using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sponge2.UI.ComponentEditors;

namespace Sponge2.Model
{
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// </summary>
	public  class FileType
	{
		private readonly Func<string, bool> _isMatchPredicate;

		public string Name { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string matchForEndOfFileName)
		{
			return new FileType(name, p=> p.EndsWith(matchForEndOfFileName));
		}

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string[] matchesForEndOfFileName)
		{
			return new FileType(name, p => matchesForEndOfFileName.Any(ext => p.EndsWith(ext)));
		}

		/// ------------------------------------------------------------------------------------
		public FileType(string name, Func<string, bool>isMatchPredicate)
		{
			Name = name;
			_isMatchPredicate = isMatchPredicate;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsMatch(string path)
		{
			return _isMatchPredicate(path);
		}

		public virtual IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			yield return new EditorProvider(new SimpleFileInfoControl(file), "Info");
			yield return new EditorProvider(new SimpleFileInfoControl(file), "TEST");
		}
	}

	/// ------------------------------------------------------------------------------------
	public class UnknownFileType : FileType
	{
		public UnknownFileType()
			: base("Unknown", path => true)
		{
		}

		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			yield return new EditorProvider(new SimpleFileInfoControl(file), "Info");
		}
	}

	public class EditorProvider
	{
		private readonly Control _control;

		public EditorProvider(Control control, string tabName)
		{
			_control = control;
			TabName = tabName;
		}

		public string TabName { get; private set; }

		/// <summary>
		/// Note: the caller doesn't own what this returns: don't dispose of it, ever
		/// </summary>
		public Control GetEditor(ComponentFile file)
		{
			//NB: in the future, we can do more complicated things like reusing controls,
			//constructing controls using the DI container, etc.
			//This will be invisible to the client.
			return _control;
		}
	}
}