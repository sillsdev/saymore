using NUnit.Framework;
using SIL.WritingSystems;

namespace SayMoreTests
{
	[SetUpFixture]
	public class SetupFixtureClass
	{
		/// <summary>
		/// Many unit tests require this initialization, and it must only be done once.
		/// </summary>
		[OneTimeSetUp]
		public void SetupFixture()
		{
			Sldr.Initialize();
		}

		[OneTimeTearDown]
		public void TearDownFixture()
		{
			Sldr.Cleanup();
		}
	}
}
