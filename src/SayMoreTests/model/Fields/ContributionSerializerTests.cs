using System;
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Core.ClearShare;
using SayMore.Model.Fields;

namespace SayMoreTests.Model.Fields
{
	[TestFixture]
	public class ContributionSerializerTests
	{
		private ContributionSerializer _serializer;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_serializer = new ContributionSerializer();
		}

		/// ------------------------------------------------------------------------------------
		public string XmlBlob =>
			@"
					<contributions>
						<contributor>
							<name>Red</name>
							<role>developer</role>
							<date>1993/07/04</date>
							<notes>Show host</notes>
						</contributor>
						<contributor>
							<name>Green</name>
							<role>singer</role>
							<date>2004/10/18</date>
							<notes>Show dork</notes>
						</contributor>
					</contributions>";

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Deserialize_ReturnsCollection()
		{
			VerifyCollection(_serializer.Deserialize(XmlBlob) as ContributionCollection);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Serialize_CanRoundTrip()
		{
			var collection = _serializer.Deserialize(XmlBlob) as ContributionCollection;
			var blob = _serializer.Serialize(collection).ToString();
			VerifyCollection(_serializer.Deserialize(blob) as ContributionCollection);
		}

		/// ------------------------------------------------------------------------------------
		private static void VerifyCollection(IList<Contribution> collection)
		{
			Assert.AreEqual(2, collection.Count);
			Assert.AreEqual("Red", collection[0].ContributorName);
			Assert.AreEqual("Green", collection[1].ContributorName);
			Assert.AreEqual("Developer", collection[0].Role.Name);
			Assert.AreEqual("Singer", collection[1].Role.Name);
			Assert.AreEqual(new DateTime(1993, 7, 4), collection[0].Date);
			Assert.AreEqual(new DateTime(2004, 10, 18), collection[1].Date);
			Assert.AreEqual("Show host", collection[0].Comments);
			Assert.AreEqual("Show dork", collection[1].Comments);
		}
	}
}
