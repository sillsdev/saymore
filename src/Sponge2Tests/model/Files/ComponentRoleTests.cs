using System.Linq;
using NUnit.Framework;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2Tests.model.Files
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
			return new ComponentRole(typeof(Session),"original", "Original Recording", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$ElementId$_Original");
		}

		[Test]
		public void GetCanonicalName_NoDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.AreEqual("mySession_Original.wav", role.GetCanoncialName("mySession", "fub.wav"));
		}

		[Test]
		public void GetCanonicalName_HasDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.AreEqual(@"c:\foo\mySession_Original.wav", role.GetCanoncialName("mySession", @"c:\foo\fub.wav"));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_HaveOneMatching_True()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsTrue(role.AtLeastOneFileHasThisRole("mySession", new string[] { "x.txt", @"c:\foo\mySession_Original.wav", "z.doc" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingSession_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("mySession", new string[] { @"c:\foo\XSession_Original.wav" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingExtension_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("mySession", new string[] { @"c:\foo\mySession_Original.txt" }));
		}

		[Test]
		public void AtLeastOneFileHasThisRole_NonMatchingTemplate_False()
		{
			ComponentRole role = GetRoleForOriginalRecording();
			Assert.IsFalse(role.AtLeastOneFileHasThisRole("mySession", new string[] { @"c:\foo\mySession_BLAH.wav" }));
		}
	}
}