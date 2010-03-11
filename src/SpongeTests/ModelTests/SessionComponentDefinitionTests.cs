using System.Linq;
using NUnit.Framework;
using SIL.Sponge.Model;

[TestFixture]
public sealed class SessionComponentDefinitionTests
{
	[Test]
	public void GetFileIsElligible_PathMatches_True()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.IsTrue(def.GetFileIsElligible("fub.wav"));
		Assert.IsTrue(def.GetFileIsElligible("fub.mp3"));
		Assert.IsTrue(def.GetFileIsElligible("fub.ogg"));
		Assert.IsTrue(def.GetFileIsElligible("fub.avi"));
		Assert.IsTrue(def.GetFileIsElligible("fub.mov"));
	}

	[Test]
	public void GetFileIsElligible_PathDoesNotMatch_False()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.IsFalse(def.GetFileIsElligible("fub.xmp3"));
		Assert.IsFalse(def.GetFileIsElligible("fub.mp3x"));
	}

	private SessionComponentDefinition GetDefinitionForOriginalRecording()
	{
		return SessionComponentDefinition.CreateHardCodedDefinitions().First();
	}

	[Test]
	public void GetCanonicalName_NoDirectoryInPath_ChangesName()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.AreEqual("mySession_Original.wav", def.GetCanoncialName("mySession", "fub.wav"));
	}

	[Test]
	public void GetCanonicalName_HasDirectoryInPath_ChangesName()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.AreEqual(@"c:\foo\mySession_Original.wav", def.GetCanoncialName("mySession", @"c:\foo\fub.wav"));
	}

	[Test]
	public void SessionHasThisComponent_HaveOneMatching_True()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.IsTrue(def.SessionHasThisComponent("mySession", new string[]{"x.txt", @"c:\foo\mySession_Original.wav", "z.doc"}));
	}

	[Test]
	public void SessionHasThisComponent_NonMatchingSession_False()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.IsFalse(def.SessionHasThisComponent("mySession", new string[] { @"c:\foo\XSession_Original.wav" }));
	}

   [Test]
	public void SessionHasThisComponent_NonMatchingExtension_False()
	{
		SessionComponentDefinition def = GetDefinitionForOriginalRecording();
		Assert.IsFalse(def.SessionHasThisComponent("mySession", new string[] { @"c:\foo\mySession_Original.txt" }));
	}
   [Test]
   public void SessionHasThisComponent_NonMatchingTemplate_False()
   {
	   SessionComponentDefinition def = GetDefinitionForOriginalRecording();
	   Assert.IsFalse(def.SessionHasThisComponent("mySession", new string[] { @"c:\foo\mySession_BLAH.wav" }));
   }
}
