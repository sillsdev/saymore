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
// File: TagTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using NUnit.Framework;
using SIL.Sponge.Model;

namespace SpongeTests.ModelTests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class TagTests
	{
		private const string kXml =
			"<fileTags>" +
			"<tag fileExtensions=\"wma,mp3,wav\">" +
			"<name>Recording</name>" +
			"<description>Audio</description>" +
			"</tag>" +
			"<tag fileExtensions=\"pdf,doc\">" +
			"<name>Transcription</name>" +
			"<description>Document</description>" +
			"</tag>" +
			"<tag fileExtensions=\"eaf\">" +
			"<name>Annotation</name>" +
			"<description>ElanAnn</description>" +
			"</tag>" +
			"</fileTags>";

		private TagList m_list;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_list = TagList.Load(kXml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the TagList.Load() method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadTagList()
		{
			Assert.AreEqual(3, m_list.Count);

			Assert.AreEqual("Recording", m_list[0].Name);
			Assert.AreEqual("Audio", m_list[0].Description);
			Assert.AreEqual("Transcription", m_list[1].Name);
			Assert.AreEqual("Document", m_list[1].Description);
			Assert.AreEqual("Annotation", m_list[2].Name);
			Assert.AreEqual("ElanAnn", m_list[2].Description);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the InitializeTagExtensions method in the TagList class.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void InitializeTagExtensions()
		//{
		//    TagList list = new TagList();
		//    var tagsByExt =
		//        ReflectionHelper.GetField(list, "m_tagsByExtension") as Dictionary<string, Tag>;

		//    Assert.IsNotNull(tagsByExt);

		//    ReflectionHelper.CallMethod(list, "InitializeTagExtensions", null);
		//    Assert.AreEqual(0, tagsByExt.Count);

		//    Tag tag = new Tag();
		//    tag.Extensions = "mp3";
		//    ReflectionHelper.CallMethod(list, "InitializeTagExtensions", tag);
		//    Assert.AreEqual(1, tagsByExt.Count);
		//    Assert.AreEqual(tag, tagsByExt["mp3"]);

		//    tag.Extensions = ".mp3, wav,ogg";
		//    ReflectionHelper.CallMethod(list, "InitializeTagExtensions", tag);
		//    Assert.AreEqual(3, tagsByExt.Count);
		//    Assert.AreEqual(tag, tagsByExt["mp3"]);
		//    Assert.AreEqual(tag, tagsByExt["wav"]);
		//    Assert.AreEqual(tag, tagsByExt["ogg"]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that the GetTagForExtension method returns the appropriate tag for an
		///// extension.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTagForExtension()
		//{
		//    Assert.AreEqual(3, m_list.Count);

		//    Assert.AreEqual(m_list[0], m_list.GetTagForExtension("wma"));
		//    Assert.AreEqual(m_list[0], m_list.GetTagForExtension("mp3"));
		//    Assert.AreEqual(m_list[0], m_list.GetTagForExtension("wav"));
		//    Assert.AreEqual(m_list[1], m_list.GetTagForExtension("pdf"));
		//    Assert.AreEqual(m_list[1], m_list.GetTagForExtension("doc"));
		//    Assert.AreEqual(m_list[2], m_list.GetTagForExtension("eaf"));
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that the GetExtensionsForTag method in TagList returns the proper extensions.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetExtensionsForTag()
		//{
		//    Assert.AreEqual(3, m_list.Count);

		//    var extensions = m_list.GetExtensionsForTag(m_list[0]);
		//    Assert.AreEqual(3, extensions.Count);
		//    Assert.IsTrue(extensions.Contains("wma"));
		//    Assert.IsTrue(extensions.Contains("wav"));
		//    Assert.IsTrue(extensions.Contains("mp3"));

		//    extensions = m_list.GetExtensionsForTag(m_list[1]);
		//    Assert.AreEqual(2, extensions.Count);
		//    Assert.IsTrue(extensions.Contains("pdf"));
		//    Assert.IsTrue(extensions.Contains("doc"));

		//    extensions = m_list.GetExtensionsForTag(m_list[2]);
		//    Assert.AreEqual(1, extensions.Count);
		//    Assert.IsTrue(extensions.Contains("eaf"));
		//}
	}
}