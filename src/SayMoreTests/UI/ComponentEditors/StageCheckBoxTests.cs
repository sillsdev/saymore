using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.UI.ComponentEditors
{
	[TestFixture]
	[Apartment(System.Threading.ApartmentState.STA)]
	public class StageCheckBoxTests
	{
		[Test]
		public void UpdateText_WhenNameProviderChanges_TextUpdates()
		{
			var callCount = 0;
			var labels = new[] { "Transcription", "Transcripción" };
			var role = new ComponentRole(typeof(Session), ComponentRole.kTranscriptionComponentRoleId,
				() => labels[callCount], ComponentRole.MeasurementTypes.Words,
				_ => true, "$ElementId$_Transcription", Color.White, Color.Black);

			var checkBox = new StageCheckBox(role) { IsRoleCompleteProvider = _ => false };
			var stageValues = new Dictionary<string, StageCompleteType>
			{
				[role.Id] = StageCompleteType.NotComplete
			};
			checkBox.Update(false, stageValues);
			Assert.That(checkBox.Text, Is.EqualTo("Transcription"));

			callCount = 1;
			checkBox.UpdateText();
			Assert.That(checkBox.Text, Is.EqualTo("Transcripción"));
		}
	}
}
