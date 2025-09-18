using System;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using SayMore.UI.ProjectWindow;
using System.Threading;

namespace SayMoreTests.UI.ProjectWindow
{
	[TestFixture]
	public class ReleaseNotesCommandTests
	{
		[Test, Apartment(ApartmentState.STA)]
		[NonParallelizable]
		public void Execute_LaunchAndClose_DoesNotCrash()
		{
			if (!Environment.UserInteractive)
				Assert.Ignore("Ignored in CI");
			
			var tester = new ModalFormTester();
			var buttonTester = new ButtonTester("_okButton");
			tester.ExpectModal("ShowReleaseNotesDialog", () => buttonTester.FireEvent("Click"));
			var cmd = new ReleaseNotesCommand();
			cmd.Execute();
			tester.Verify();
		}

	}
}
