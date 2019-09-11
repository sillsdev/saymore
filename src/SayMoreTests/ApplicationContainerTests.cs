using System.Linq;
using NUnit.Framework;
using SayMore;
using SayMore.Model.Files;

namespace SayMoreTests
{
	[TestFixture]
	public class ApplicationContainerTests
	{
		[TestCase(ComponentRole.kOralTranslationComponentRoleId, "_Translation.wav", ExpectedResult = true)]
		[TestCase(ComponentRole.kCarefulSpeechComponentRoleId, "_Careful.wav", ExpectedResult = true)]
		public bool GetRenamingTemplateSuffix_RolesWithSegmentFileSuffix_SegmentFileSuffixStartsWithRenamingTemplateSuffix(string roleId, string segmentFileSuffix)
		{
			// get the oral translation role
			var role = ApplicationContainer.ComponentRoles.Single(r => r.Id == roleId);
			Assert.NotNull(role);

			// segment file suffix must start with the role RenamingTemplateSuffix
			var renamingSuffix = ComponentRole.kFileSuffixSeparator + role.GetRenamingTemplateSuffix();

			return segmentFileSuffix.StartsWith(renamingSuffix);
		}
	}
}
