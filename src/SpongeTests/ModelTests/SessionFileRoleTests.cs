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
		Assert.IsTrue(role.GetSomeFileForThisRoleExists(new string[]{"x.txt", @"c:\foo\y.wav", "z.doc"}));
	}
}
