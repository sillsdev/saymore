using SIL.Windows.Forms;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SayMore.Utilities
{
	static class RobustFontHelper
	{
		private static readonly Regex s_regexFontDescription = 
			new Regex(@"^\s*(?<fontFamily>[^,]+),\s*(?<emSize>-?[.0-9]*)(,\s*(?<style>[^,]+))?\s*$");

		/// <summary>
		/// Uses the FontHelper to create the requested font. Will attempt to fall back to a
		/// similar font in a standard size if the size specification is missing or 0. If unable
		/// to create the requested font, the user will see a non-fatal message and this will
		/// return <c>null</c>. (Caller is responsible for handling a null return value.)
		/// </summary>
		/// <param name="settingValue">Font settings string</param>
		/// <param name="handleError">Callback to handle an error. If this method returns
		/// <c>null</c>, the exception parameter will also be set to a non-null value. If this
		/// handler is called with a <c>null</c> exception parameter, the fallback font was
		/// successfully created, but the caller may still wish to warn the user about the
		/// problem or take other remedial action.</param>
		/// <returns>The specified font, a reasonable fallback, or <c>null</c></returns>
		public static Font MakeFont(string settingValue, Action<Exception> handleError)
		{
			try
			{
				return FontHelper.MakeFont(settingValue);
			}
			catch (Exception origException)
			{
				var e = origException;
				while (!(e is ArgumentException) && e.InnerException != null)
					e = e.InnerException;
				if (e is ArgumentException)
				{
					var match = s_regexFontDescription.Match(settingValue);
					if (match.Success)
					{
						try
						{
							var fallback = match.Result("${fontFamily}, " + SystemFonts.MessageBoxFont.Size);
							if (match.Groups["style"].Length > 0)
								fallback += ", " + match.Groups["style"].Value;
							var font = FontHelper.MakeFont(fallback);
							if (font != null)
							{
								handleError?.Invoke(null);
								return font;
							}
						}
						catch (Exception)
						{
							// Report outer exception instead.
						}
					}
				}
				handleError?.Invoke(e as ArgumentException ?? origException);

				return null;
			}
		}
	}
}
