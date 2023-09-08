using System;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;

namespace SayMore.UI
{
	internal static class FontDialogHelper
	{
		public static Font SelectFont(Form owner, Font defaultFont)
		{
			using (var dlg = new FontDialog())
			{
				dlg.Font = defaultFont;

				try //strange, but twice we've found situations where ShowDialog crashes on windows
				{
					if (DialogResult.OK != dlg.ShowDialog(owner))
					{
						return null;
					}
				}
				catch (Exception)
				{
					// Note: this localization ID is not really 100% accurate since this is also used for the Free Translation font
					// and the project's working language font, but probably not worth changing.
					ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString("SessionsView.Transcription.FontDialogProblem",
						"There was some problem with choosing that font. If you just installed it, you might try restarting the program or even your computer."));
					return null;
				}

				return dlg.Font;
			}
		}
	}
}
