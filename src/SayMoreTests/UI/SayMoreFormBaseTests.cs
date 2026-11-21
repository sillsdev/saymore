using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI;
using SayMore.Utilities;

namespace SayMoreTests.UI
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class SayMoreFormBaseTests
	{
		private class TestForm : SayMoreFormBase
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Mirrors the SegmenterDlgBase/OralAnnotationRecorderDlgBase bug shape: a subclass's
		/// own OnLoad override calls base.OnLoad(e) (triggering the Load-based scaling pass)
		/// and then sets a raw, unscaled Font on a child control afterward.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private class FormThatResetsAFontAfterBaseOnLoad : SayMoreFormBase
		{
			public float UnscaledBaselineSize { get; } = 10f;
			public Label LabelSetAfterBaseOnLoad { get; }

			public FormThatResetsAFontAfterBaseOnLoad()
			{
				LabelSetAfterBaseOnLoad = new Label
					{ Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, UnscaledBaselineSize) };
				Controls.Add(LabelSetAfterBaseOnLoad);
			}

			protected override void OnLoad(EventArgs e)
			{
				base.OnLoad(e);
				LabelSetAfterBaseOnLoad.Font = new Font(FontFamily.GenericSansSerif, UnscaledBaselineSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Load_ScaleNotAt100Percent_ScalesChildControlFont()
		{
			Settings.Default.UITextScale = 200;

			using (var form = new TestForm())
			{
				var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				form.Controls.Add(label);
				var origSize = label.Font.Size;

				form.Show();

				Assert.Greater(label.Font.Size, origSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleChanged_FormAlreadyLoaded_RescalesChildControlFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (var form = new TestForm())
			{
				var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				form.Controls.Add(label);
				form.Show();
				var sizeAt100Percent = label.Font.Size;

				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();

				Assert.Greater(label.Font.Size, sizeAt100Percent);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Shown_SubclassSetsFontAfterBaseOnLoad_StillEndsUpScaled()
		{
			Settings.Default.UITextScale = 200;

			using (var form = new FormThatResetsAFontAfterBaseOnLoad())
			{
				form.Show();
				// Form.Shown is posted via BeginInvoke, not raised synchronously from Show() --
				// a headless test has no message loop to deliver it without pumping once.
				Application.DoEvents();
				Assert.Greater(form.LabelSetAfterBaseOnLoad.Font.Size, form.UnscaledBaselineSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_FormDisposed_NoLongerRespondsToScaleChanged()
		{
			Settings.Default.UITextScale = 100;

			var form = new TestForm();
			var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
			form.Controls.Add(label);
			form.Show();

			form.Dispose();

			Assert.DoesNotThrow(() =>
			{
				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();
			});
		}
	}
}
