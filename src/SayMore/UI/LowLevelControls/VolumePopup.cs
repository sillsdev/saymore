using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using L10NSharp;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	public partial class VolumePopup : UserControl
	{
		public event EventHandler VolumeChanged;

		/// ------------------------------------------------------------------------------------
		public VolumePopup()
		{
			InitializeComponent();
			var pctFmt = LocalizationManager.GetString("CommonToMultipleViews.PercentFormat", "{0}%");
			_sliderVolume.TooltipFormat = string.Format(pctFmt, "{0:F0}");
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public float VolumeLevel
		{
			get { return _sliderVolume.Value; }
			set { _sliderVolume.Value = value; }
		}

		/// ------------------------------------------------------------------------------------
		public void SetVolumeLevelWithoutEvents(float value)
		{
			_sliderVolume.SetValueWithoutEvent(value);
		}

		/// ------------------------------------------------------------------------------------
		public ToolStripDropDown OwningDropDown { get; set; }

		/// ------------------------------------------------------------------------------------
		private void HandleSliderVolumeValueChanged(object sender, EventArgs e)
		{
			if (VolumeChanged != null)
				VolumeChanged(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSliderVolumeMouseUp(object sender, MouseEventArgs e)
		{
			if (OwningDropDown != null)
				OwningDropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = ClientRectangle;
			var clr1 = Color.White;
			var clr2 = Color.Silver;
			using (var br = new LinearGradientBrush(rc, clr1, clr2, 0f))
				e.Graphics.FillRectangle(br, rc);

			rc.Width--;
			rc.Height--;

			e.Graphics.DrawRectangle(Pens.DarkGray, rc);
		}
	}
}
