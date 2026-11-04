using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// EditorBase.SetComponentFile calls BindingHelper.SetComponentFile on every single element
	/// (row) selection, because the editor and its BindingHelper are long-lived, cached instances
	/// reused across selections, not recreated per row.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class BindingHelperTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void SetComponentFile_ScaleAboveDefault_BoundControlFontIsScaled()
		{
			Settings.Default.UITextScale = 200;

			using (var helper = new BindingHelper())
			using (var ctrl = new TextBox { Name = "_title", Font = new Font(FontFamily.GenericSansSerif, 9f) })
			{
				var originalUnscaledSize = ctrl.Font.Size;
				helper.SetIsBound(ctrl, true);
				var file = new ComponentFile(FileType.Create("Text", ".txt"));

				// Two calls simulate two element (row) selections reusing the same cached
				// editor/BindingHelper/control instances.
				helper.SetComponentFile(file);
				helper.SetComponentFile(file);

				Assert.Greater(ctrl.Font.Size, originalUnscaledSize);
			}
		}
	}
}
