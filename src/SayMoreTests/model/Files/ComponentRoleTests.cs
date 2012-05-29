using System.Drawing;
using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Utilities;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public sealed class ComponentRoleTests
	{
		[Test]
		public void IsPotential_PathMatches_True()
		{
			ComponentRole role = GetRoleForSourceRecording();
			Assert.IsTrue(role.IsPotential("fub.wav"));
			Assert.IsTrue(role.IsPotential("fub.mp3"));
			Assert.IsTrue(role.IsPotential("fub.ogg"));
			Assert.IsTrue(role.IsPotential("fub.avi"));
			Assert.IsTrue(role.IsPotential("fub.mov"));
		}

		[Test]
		public void IsPotential_PathDoesNotMatch_False()
		{
			ComponentRole role = GetRoleForSourceRecording();
			Assert.IsFalse(role.IsPotential("fub.xmp3"));
			Assert.IsFalse(role.IsPotential("fub.mp3x"));
		}

		[Test]
		public void IsMatch_IsNotCorrectFileType_ReturnsFalse()
		{
			var role = GetRoleForSourceRecording();
			Assert.IsFalse(role.IsMatch("fub.ogg"));
			Assert.IsFalse(role.IsMatch("fub.txt"));
			Assert.IsFalse(role.IsMatch("fub.pdf"));
			Assert.IsFalse(role.IsMatch("fub.doc"));
			Assert.IsFalse(role.IsMatch("fub_Original.doc"));
			Assert.IsFalse(role.IsMatch("fub_Source.doc"));
		}

		[Test]
		public void IsMatch_HasCorrectFileTypeWithoutCorrectSuffix_ReturnsFalse()
		{
			var role = GetRoleForSourceRecording();
			Assert.IsFalse(role.IsMatch("fub.wav"));
			Assert.IsFalse(role.IsMatch("fub.mpg"));
			Assert.IsFalse(role.IsMatch("fub.mp3"));
			Assert.IsFalse(role.IsMatch("fub.wmv"));
		}

		[Test]
		public void IsMatch_HasCorrectFileTypeAndSuffix_ReturnsTrue()
		{
			var role = GetRoleForSourceRecording();
			Assert.IsTrue(role.IsMatch("fub_Source.wav"));
			Assert.IsTrue(role.IsMatch("fub_Source.mpg"));
			Assert.IsTrue(role.IsMatch("fub_Source.mp4"));
		}

		[Test]
		public void IsMatch_HasCorrectFileTypeAndOldSourceSuffix_ReturnsTrue()
		{
			var role = GetRoleForSourceRecording();
			Assert.IsTrue(role.IsMatch("fub_Original.wav"));
			Assert.IsTrue(role.IsMatch("fub_Original.mpg"));
			Assert.IsTrue(role.IsMatch("fub_Original.mp4"));
		}

		private static ComponentRole GetRoleForSourceRecording()
		{
			return new ComponentRole(typeof(Session), "source", "Source Recording",
				ComponentRole.MeasurementTypes.Time, FileSystemUtils.GetIsAudioVideo,
				"$ElementId$_Source", Color.Magenta, Color.Black);
		}

		[Test]
		public void GetCanonicalName_NoDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForSourceRecording();
			Assert.AreEqual("mySession_Source.wav", role.GetCanoncialName("mySession", "fub.wav"));
		}

		[Test]
		public void GetCanonicalName_HasDirectoryInPath_ChangesName()
		{
			ComponentRole role = GetRoleForSourceRecording();
			Assert.AreEqual(@"c:\foo\mySession_Source.wav", role.GetCanoncialName("mySession", @"c:\foo\fub.wav"));
		}

		//[Test]
		//public void AtLeastOneFileHasThisRole_HaveOneMatching_True()
		//{
		//    ComponentRole role = GetRoleForSourceRecording();
		//    Assert.IsTrue(role.AtLeastOneFileHasThisRole("myEvent", new[] { "x.txt", @"c:\foo\myEvent_Source.wav", "z.doc" }));
		//}

		//[Test]
		//public void AtLeastOneFileHasThisRole_NonMatchingEvent_False()
		//{
		//    ComponentRole role = GetRoleForSourceRecording();
		//    Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new[] { @"c:\foo\XEvent_Source.wav" }));
		//}

		//[Test]
		//public void AtLeastOneFileHasThisRole_NonMatchingExtension_False()
		//{
		//    ComponentRole role = GetRoleForSourceRecording();
		//    Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new[] { @"c:\foo\myEvent_Source.txt" }));
		//}

		//[Test]
		//public void AtLeastOneFileHasThisRole_NonMatchingTemplate_False()
		//{
		//    ComponentRole role = GetRoleForSourceRecording();
		//    Assert.IsFalse(role.AtLeastOneFileHasThisRole("myEvent", new[] { @"c:\foo\myEvent_BLAH.wav" }));
		//}
	}
}