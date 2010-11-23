using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.ClearShare;

namespace SayMoreTests.ClearShare
{
	/// <summary>
	/// High-level examples of using ClearShare
	/// </summary>
	public class ClearShareUsage
	{
		[Test, Ignore("not yet")]
		public void TypicalEmbedInMyXmlDocument()
		{
			var system = new OlacSystem();

			Work work = new Work();
			work.Licenses.Add(License.CreativeCommons_Attribution_ShareAlike);
			work.Contributions.Add(new Contribution("Charlie Brown", system.GetRoleOrThrow("author")));
			work.Contributions.Add(new Contribution("Linus", system.GetRoleOrThrow("editor")));

			string metaData = system.GetXmlForWork(work);

			//Embed that data in our own file
			using (var f = new TempFile(@"<doc>
				<metadata>" + metaData + @"</metadata>
				<ourDocumentContents>blah blah<ourDocumentContents/></doc>"))
			{
				//Then when it comes time to read the file, we can extract out the work again
				var dom = new XmlDocument();
				dom.Load(f.Path);

				var node = dom.SelectSingleNode("//metadata");
				var work2 = system.LoadWorkFromXml(node.InnerXml);

				Assert.AreEqual(2,work2.Contributions.Count());
			}

		}

	}

	public class ClearShareSystem
	{
		public ClearShareSystem(IArchivingMetaDataSystem metaDataSystem)
		{

		}
	}
}
