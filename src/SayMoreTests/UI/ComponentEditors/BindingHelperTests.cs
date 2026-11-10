using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.Utilities;

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Working-language text boxes are bound too, but their editor assigns them an
		/// already-scaled working-language font; selecting another element must leave that
		/// font exactly as assigned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void SetComponentFile_BoundControlExcludedFromUIFontScaling_KeepsAssignedFont()
		{
			Settings.Default.UITextScale = 130;

			using (var helper = new BindingHelper())
			using (var ctrl = new TextBox { Name = "_education" })
			{
				UITextScaler.ExcludeFromUIFontScaling(ctrl);
				helper.SetIsBound(ctrl, true);
				var file = new ComponentFile(FileType.Create("Text", ".txt"));
				helper.SetComponentFile(file);

				var assigned = new Font("Times New Roman", 14f * 1.3f);
				ctrl.Font = assigned;

				// Two calls simulate selecting another element and then reselecting this one.
				helper.SetComponentFile(file);
				helper.SetComponentFile(file);

				Assert.AreEqual(assigned, ctrl.Font);
			}
		}
	}
}
