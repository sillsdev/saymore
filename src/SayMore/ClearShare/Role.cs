using System;
using System.Text;

namespace SayMore.ClearShare
{
	public class Role
	{
		public string Code { get; private set; }
		public string Name { get; private set; }
		public string Definition { get; private set; }
		public override string ToString() { return Name; }

		/// ------------------------------------------------------------------------------------
		public Role(string code, string name, string definition)
		{
			Code = code;
			Name = name;

			if (definition != null)
			{
				var bldr = new StringBuilder(definition.Length);
				foreach (var word in definition.Split(new[] { ' ', '\t', '\n', '\r' },
					StringSplitOptions.RemoveEmptyEntries))
				{
					bldr.AppendFormat("{0} ", word);
				}

				bldr.Length--;
				Definition = bldr.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		public Role Clone()
		{
			return new Role(Code, Name, Definition);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if the contents of this Role are the same as those of the specified
		/// Role.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreContentsEqual(Role other)
		{
			if (other == null)
				return false;

			// REVIEW: Do we really care if name and definition are different. Perhaps only
			// the code really matters.
			return (Code == other.Code && Name == other.Name && Definition == other.Definition);
		}
	}
}