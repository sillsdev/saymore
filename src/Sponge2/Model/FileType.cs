using System;
using System.Linq;

namespace Sponge2.Model
{
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editting, or viewing that file.
	/// </summary>
	public class FileType
	{
		private readonly Func<string, bool> _isMatchPredicate;

		public static FileType Create(string name, string matchForEndOfFileName)
		{
			return new FileType(name, p=>p.EndsWith(matchForEndOfFileName));
		}

		public static FileType Create(string name, string[] matchesForEndOfFileName)
		{
			return new FileType(name, p => matchesForEndOfFileName.Any(ext => p.EndsWith(ext)));
		}

		public FileType(string name, Func<string, bool>isMatchPredicate)
		{
			Name = name;
			_isMatchPredicate = isMatchPredicate;
		}

		public string Name { get; private set; }

		public bool IsMatch(string path)
		{
			return _isMatchPredicate(path);
		}
	}
}