using System.Linq;
using NUnit.Framework;
using SayMore;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMoreTests
{
	[TestFixture]
	public class ApplicationContainerTests
	{
		[Test]
		public void ComponentRoles_ContainsOralTranslationRole_HasCorrectRenamingSuffix()
		{
			// get list of all available roles
			var roles = ApplicationContainer.ComponentRoles.ToList();

			// get the oral translation role
			var oralTranslationRole = roles.FirstOrDefault(r => r.Id == ComponentRole.kOralTranslationComponentRoleId);
			Assert.NotNull(oralTranslationRole);

			// OralAnnotationTranslationSegmentFileSuffix must start with the oral translation role RenamingTemplateSuffix
			var renamingSuffix = ComponentRole.kFileSuffixSeparator + oralTranslationRole.GetRenamingTemplateSuffix();
			Assert.IsTrue(Settings.Default.OralAnnotationTranslationSegmentFileSuffix.StartsWith(renamingSuffix));
		}
	}
}
