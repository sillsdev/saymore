using NUnit.Extensions.Forms;
using NUnit.Framework;
using SayMore.UI.ProjectWindow;

namespace SayMoreTests.UI.ProjectWindow
{
	public class ReleaseNotesCommandTests
	{
		[Test]
		public void Execute_LaunchAndClose_DoesNotCrash()
		{
			var tester = new ModalFormTester();
			var buttonTester = new ButtonTester("_okButton");
			tester.ExpectModal("ShowReleaseNotesDialog", () => buttonTester.FireEvent("Click"));
			var cmd = new ReleaseNotesCommand();
			cmd.Execute();
			tester.Verify();
		}

	}
}
