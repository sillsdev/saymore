
using System.Reflection;
using System.Windows.Forms;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
using DatePicker = SayMore.UI.LowLevelControls.DatePicker;

namespace SayMore.Utilities
{
	internal static class ExtensionMethods
	{
		public static MethodInfo HasMethod(this object objectToCheck, string methodName)
		{
			var type = objectToCheck.GetType();
			return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		public static bool IsValidBirthYear(this string birthYear)
		{
			var val = birthYear.Trim();

			// year must be a 4 digit integer
			return string.IsNullOrEmpty(val) || (val.Length == 4 && int.TryParse(val, out _));
		}

		/// <summary>
		/// When the user presses Enter, should we treat it as a Tab press?
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static bool ShouldTreatEnterAsTab(this Control ctrl)
		{
			if (ctrl is ComboBox)
				return true;

			if (ctrl is DatePicker)
				return true;

			// Enter is valid input for a multi-line text box
			if (ctrl is TextBox txt)
				return !txt.Multiline;

			return false;
		}

		/// <summary>
		/// We only want to jump to controls which we can type into
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static bool ShouldTabToMe(this Control ctrl)
		{
			if (ctrl is ComboBox)
				return true;

			if (ctrl is DatePicker)
				return true;

			if (ctrl is TextBox)
				return true;

			if (ctrl is DataGridView)
				return true;

			return false;
		}
	}
}
