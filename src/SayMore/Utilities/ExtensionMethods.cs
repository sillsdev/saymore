
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
	}
}
