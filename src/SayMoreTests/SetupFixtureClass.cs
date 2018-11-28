using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		[SetUp]
		public void SetupFixture()
		{
			Sldr.Initialize();
		}

		[TearDown]
		public void TearDownFixture()
		{
			Sldr.Cleanup();
		}
	}
}
