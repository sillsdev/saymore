using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class EventTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("eventTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		private Event CreateEvent(string id)
		{
			return new Event(_parentFolder.Path, id, new EventFileType(() => null),
				ComponentFile.CreateMinimalComponentFileForTests, new FileSerializer(), null);
		}

		/*
		 * THIS IS MOSTLY EMPTY BECAUSE MOST OF THE BEHAVIOR THUS FAR IS IN THE BASE CLASS,
		 * AND TESTED THERE, INSTEAD
		 */
	}
}
