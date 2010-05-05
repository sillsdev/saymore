// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: DiscourseType.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilUtils;

namespace Sponge2.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("type")]
	public class DiscourseType
	{
		private static List<DiscourseType> s_allTypes;

		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("comments")]
		public string Comments { get; set; }

		[XmlElement("definition")]
		public string Definition { get; set; }

		[XmlArray("examples"), XmlArrayItem("example")]
		public List<string> Examples { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the discourse types.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<DiscourseType> AllTypes
		{
			get
			{
				if (s_allTypes == null)
				{
					var path = Application.ExecutablePath;
					path = Path.Combine(Path.GetDirectoryName(path), "DiscourseTypes.xml");
					s_allTypes = Load(path);
				}

				return s_allTypes;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the application's discourse types from the file specified by path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<DiscourseType> Load(string path)
		{
			return XmlSerializationHelper.DeserializeFromFile<List<DiscourseType>>(path, "discourseTypes", true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tooltip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Tooltip
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
