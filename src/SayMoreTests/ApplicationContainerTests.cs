using System.Linq;
using L10NSharp;
using NUnit.Framework;
using SayMore;
using SayMore.Model.Files;

namespace SayMoreTests
{
	[TestFixture]
	public class ApplicationContainerTests
	{
		[SetUp]
		public void SetUp()
		{
			LocalizationManager.StrictInitializationMode = false;
		}
		
		[TestCase(ComponentRole.kOralTranslationComponentRoleId, "_Translation.wav")]
		[TestCase(ComponentRole.kCarefulSpeechComponentRoleId, "_Careful.wav")]
		public void GetRenamingTemplateSuffix_SegmentFileRoleId_SegmentFileSuffixStartsWithRenamingTemplateSuffix(string roleId, string segmentFileSuffix)
		{
			// get the oral translation role
			var role = ApplicationContainer.ComponentRoles.Single(r => r.Id == roleId);

			// segment file suffix must start with the role RenamingTemplateSuffix
			var renamingSuffix = ComponentRole.kFileSuffixSeparator + role.GetRenamingTemplateSuffix();

			Assert.IsTrue(segmentFileSuffix.StartsWith(renamingSuffix));
		}
	}
}
