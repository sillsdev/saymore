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
// File: Tag.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Sponge.Model
{
	#region TagList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Represents a list of possible tags for session files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("fileTags")]
	public class TagList : List<Tag>
	{
		//private readonly Dictionary<string, Tag> m_tagsByExtension = new Dictionary<string, Tag>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a TagList from the information in the specified chunk of XML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static TagList Load(string xml)
		{
			Exception e;
			TagList list = Utils.DeserializeFromString<TagList>(xml, out e);
			if (e != null)
				throw e;

			//foreach (Tag tag in list)
			//    list.InitializeTagExtensions(tag);

			return list;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Adds to the list of tags by extension those extensions for the specified tag.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void InitializeTagExtensions(Tag tag)
		//{
		//    if (tag == null || string.IsNullOrEmpty(tag.Extensions))
		//        return;

		//    tag.Extensions = tag.Extensions.Replace(" ", string.Empty);
		//    string[] extensions = tag.Extensions.Split(',');
		//    foreach (string ext in extensions)
		//        m_tagsByExtension[ext.Replace(".", string.Empty)] = tag;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the <see cref="SIL.Sponge.Tag"/> for the specified file ext.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public Tag GetTagForExtension(string ext)
		//{
		//    Tag tag;
		//    return (m_tagsByExtension.TryGetValue(ext, out tag) ? tag : null);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the list of file extensions associated with the specified tag.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public List<string> GetExtensionsForTag(Tag tag)
		//{
		//    return (from kvp in m_tagsByExtension
		//            where kvp.Value == tag
		//            select kvp.Key).ToList();
		//}
	}

	#endregion

	#region Tag class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Represents a session file tag.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("tag")]
	public class Tag
	{
		//[XmlAttribute("fileExtensions")]
		//public string Extensions { get; set; }
		[XmlElement("name")]
		public string Name { get; set; }
		[XmlElement("description")]
		public string Description { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name + ": " + Description;
		}
	}

	#endregion
}
