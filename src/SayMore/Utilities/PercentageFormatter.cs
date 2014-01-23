using System;
using System.Globalization;
using L10NSharp;

namespace SayMore.Utilities
{
	/// <summary>
	/// Simple utility class for formatting numbers as percentages for display in the UI.
	/// Ideally, the code would simply use the NumberFormat of the UI culture
	/// and format the percentage using "P0", but Windows doesn't give the user any way
	/// to set the value of the PercentPositivePattern, and the default (for "en" is 0,
	/// which means that a space is inserted between the number and percent sign (e.g.,
	/// "40 %"). So I don't think it's safe to assume that MicroSoft necessarily got it
	/// right for any cluture. Still, this is better than adding a separate localizable
	/// string for every percentage we want to be able to display in the UI.
	/// </summary>
	public class PercentageFormatter
	{
		private readonly string _pctFmt;
		private readonly IFormatProvider _formatInfo;

		public PercentageFormatter()
		{
			_pctFmt = LocalizationManager.GetString("CommonToMultipleViews.PercentFormat", "{0}%");
			try
			{
				_formatInfo = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			}
			catch
			{
				_formatInfo = CultureInfo.CurrentCulture;
			}
		}

		public PercentageFormatter(string pctFmt, IFormatProvider formatInfo)
		{
			_pctFmt = pctFmt;
			_formatInfo = formatInfo;
		}

		public string Format(int pct)
		{
			return string.Format(_pctFmt, pct.ToString("F0", _formatInfo));
		}

		public string Format(double decimalVal)
		{
			return string.Format(_pctFmt, (decimalVal * 100).ToString("N0", _formatInfo));
		}

		public string Format(string formattedPct, out double value)
		{
			var text = formattedPct.Replace("%", string.Empty).Trim();
			if (double.TryParse(text, out value))
			{
				return Format(value / 100.0);
			}
			return null;
		}
	}
}
