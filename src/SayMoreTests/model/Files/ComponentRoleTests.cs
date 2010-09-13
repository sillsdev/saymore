using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public sealed class ComponentRoleTests
	{
		[Test]
		public void IsPotential_PathMatches_True()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsTrue(role.IsPotential("fub.wav"));
			Assert.IsTrue(role.IsPotential("fub.mp3"));
			Assert.IsTrue(role.IsPotential("fub.ogg"));
			Assert.IsTrue(role.IsPotential("fub.avi"));
			Assert.IsTrue(role.IsPotential("fub.mov"));
		}

		[Test]
		public void IsPotential_PathDoesNotMatch_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.IsPotential("fub.xmp3"));
			Assert.IsFalse(role.IsPotential("fub.mp3x"));
		}

		private static ComponentRole GetRoleForOriginalRecording()
		{
			return new ComponentRole(typeof(Event), "original", "Original Recording",
				ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$ElementId$_Original");
		}

		[Test]
		public void GetCanonicalName_NoDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.AreEqual("myEvent_Original.wav", role.GetCanoncialName("myEvent", "fub.wav"));
		}

		[Test]
		public void GetCanonicalName_HasDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.AreEqual(@"c:\foo\myEvent_Original.wav", role.GetCanoncialName("myEvent", @"c:\foo\fub.wav"));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_HaveOneMatching_True()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsTrue(role.AtLeastOneFileHasThisRole("myEvent", new string[] { "x.txt", @"c:\foo\myEvent_Original.wav", "z.doc" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingEvent_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new string[] { @"c:\foo\XEvent_Original.wav" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingExtension_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new string[] { @"c:\foo\myEvent_Original.txt" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingTemplate_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new string[] { @"c:\foo\myEvent_BLAH.wav" }));
		}
	}
}