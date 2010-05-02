using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SilUtils;
using Sponge2.UI.ComponentEditors;

namespace Sponge2.Model.Files
{
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// It also tells us which commands to offer in, for example, a context menu.
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

		public virtual IEnumerable<FileCommand> Commands
		{
			get { yield return new FileCommand("Show in File Explorer...", FileCommand.HandleOpenInFileManager_Click); }
			//note: we don't offer "open in app" choice for sponge files
		}

	}
}