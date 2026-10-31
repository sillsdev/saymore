using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// ApplyFontRecursively only recurses into a hardcoded allowlist of container types
	/// (Panel, UserControl, ComboBox, DataGridView, ToolStrip). SplitContainer isn't a Panel
	/// subclass, so content inside a SplitContainer's panels (e.g. a left-hand sidebar) was
	/// silently skipped, even though the SplitContainer itself was reached.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UITextScalerSplitContainerTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_LabelInsideSplitContainerPanel1_ScalesLabelFont()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var split = new SplitContainer())
			{
				var label = new Label { Text = "Sidebar", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				split.Panel1.Controls.Add(label);
				host.Controls.Add(split);
				var origSize = label.Font.Size;

				host.ScaleUIFont();

				Assert.Greater(label.Font.Size, origSize);
			}
		}
	}
}
