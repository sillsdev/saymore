using SIL.IO;
using SIL.Windows.Forms.ReleaseNotes;

namespace SayMore.UI.ProjectWindow
{
	/// <summary>
	/// Show the release notes which come with this version
	/// </summary>
	/// <remarks>Like all commands, this class is "wired up" automatically by autofac,
	/// by nature of it implementing ICommand. Then on the menu side, the menu item
	/// which invokes it has a tag containing the id of this command ("releaseNotes").
	/// Obviously, this doesn't buy us much for such a simple command, which at the
	/// moment has no external dependencies, cannot be undone, etc. This does make it quite easy
	/// to unit test, however.
	/// </remarks>
	public class ReleaseNotesCommand :Command
	{
		public ReleaseNotesCommand() : base("releaseNotes")
		{
		}

		public override void Execute()
		{
			var path = FileLocator.GetFileDistributedWithApplication("ReleaseNotes.md");
			using (var dlg = new ShowReleaseNotesDialog(System.Windows.Forms.Application.OpenForms.Count > 0 ? System.Windows.Forms.Application.OpenForms[0].Icon : null, path))
			{
				dlg.ShowDialog();
			}
		}
	}
}
