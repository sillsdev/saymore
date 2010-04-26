using System;
using System.Collections.Generic;

namespace Sponge2.Model
{
	/// <summary>
	/// Both sessions and people are made up of a number of files: an xml file we help them edit,
	/// plus any number of other files (videos, texts, images, etc.).  Each of these is represented
	/// by an object of this class.
	/// </summary>
	public class ComponentFile
	{
		public string Path { get; set; }

		public ComponentFile(string path)
		{
			Path = path;
		}

#if notyet

		public FileType GetFileType()
		{
			return null;
		}


		/// <summary>
		/// What part(s) does this file play in the workflow of the session/person?
		/// </summary>
		public IEnumerable<ComponentRole> GetRoles()
		{
			return new ComponentRole[] {};
		}

		/// <summary>
		/// The roles various people have played in creating/editing this file.
		/// </summary>
		public List<Contribution> Contributions
		{
			get; private set;
		}

		/// <summary>
		/// The metadata do we have associated with this file.
		/// </summary>
		public List<FieldValue> MetaDataValues
		{
			get; private set;
		}
#endif
	}
}