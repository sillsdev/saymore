
using System.Reflection;

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
			int test;
			var val = birthYear.Trim();

			// year must be a 4 digit integer
			return string.IsNullOrEmpty(val) || (val.Length == 4 && int.TryParse(val, out test));
		}
	}
}
