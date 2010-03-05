using NUnit.Framework;
using SIL.Sponge.Model;

[TestFixture]
public sealed class SessionFileRoleTests
{
	[Test]
	public void GetFileIsElligible_PathMatches_True()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.IsTrue(role.GetFileIsElligible("fub.wav"));
		Assert.IsTrue(role.GetFileIsElligible("fub.mp3"));
		Assert.IsTrue(role.GetFileIsElligible("fub.ogg"));
		Assert.IsTrue(role.GetFileIsElligible("fub.avi"));
		Assert.IsTrue(role.GetFileIsElligible("fub.mov"));
	}

	[Test]
	public void GetFileIsElligible_PathDoesNotMatch_False()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.IsFalse(role.GetFileIsElligible("fub.xmp3"));
		Assert.IsFalse(role.GetFileIsElligible("fub.mp3x"));
	}

	private SessionFileRole GetOriginalFileRole()
	{
		return new SessionFileRole("original", "Original Recording", SessionFileRole.GetIsAudioVideo, "$SessionId$_Original");
	}

	[Test]
	public void GetCanonicalName_NoDirectoryInPath_ChangesName()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.AreEqual("mySession_Original.wav", role.GetCanoncialName("mySession", "fub.wav"));
	}

	[Test]
	public void GetCanonicalName_HasDirectoryInPath_ChangesName()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.AreEqual(@"c:\foo\mySession_Original.wav", role.GetCanoncialName("mySession", @"c:\foo\fub.wav"));
	}

	[Test, Ignore("not yet")]
	public void GetSomeFileForThisRoleExists_HaveOneMatching_True()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.IsTrue(role.GetSomeFileForThisRoleExists("mySession", new string[]{"x.txt", @"c:\foo\mySession_Original.wav", "z.doc"}));
	}

	[Test, Ignore("not yet")]
	public void GetSomeFileForThisRoleExists_NonMatchingSession_False()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.IsTrue(role.GetSomeFileForThisRoleExists("mySession", new string[] {@"c:\foo\XSession_Original.wav"}));
	}

   [Test, Ignore("not yet")]
	public void GetSomeFileForThisRoleExists_NonMatchingSessionExtension_False()
	{
		SessionFileRole role = GetOriginalFileRole();
		Assert.IsTrue(role.GetSomeFileForThisRoleExists("mySession", new string[] { @"c:\foo\mySession_Original.txt" }));
	}
   [Test, Ignore("not yet")]
   public void GetSomeFileForThisRoleExists_NonMatchingTemplat_False()
   {
	   SessionFileRole role = GetOriginalFileRole();
	   Assert.IsTrue(role.GetSomeFileForThisRoleExists("mySession", new string[] { @"c:\foo\mySession_BLAH.wav" }));
   }

}
