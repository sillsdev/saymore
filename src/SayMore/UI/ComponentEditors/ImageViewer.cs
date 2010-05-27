using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;
using SilUtils;
using SilUtils.Controls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ImageViewer : EditorBase
	{
		private readonly int[] _clickZoomPercentages;
		private readonly Bitmap _image;
		private readonly SilPanel _panelImage;
		private bool _firstTimePaint = true;

		/// ------------------------------------------------------------------------------------
		public ImageViewer(ComponentFile file)
		{
			InitializeComponent();
			Name = "ImageViewer";

			_labelZoomPercent.Font = SystemFonts.IconTitleFont;
			_labelZoom.Font = SystemFonts.IconTitleFont;

			_image = new Bitmap(file.PathToAnnotatedFile);

			_panelImage = new SilPanel();
			_panelImage.Dock = DockStyle.Fill;
			_panelImage.AutoScroll = true;
			_panelImage.Cursor = Cursors.Hand;
			Controls.Add(_panelImage);
			_panelImage.BringToFront();
			_panelImage.Paint += HandleImagePanelPaint;
			_panelImage.Scroll += HandleImagePanelScroll;
			_panelImage.MouseClick += HandleImagePanelMouseClick;
			_panelImage.MouseDoubleClick += HandleImagePanelMouseClick;

			_clickZoomPercentages = PortableSettingsProvider.GetIntArrayFromString(
				Settings.Default.ImageViewerClickImageZoomPercentages);
		}

		/// ------------------------------------------------------------------------------------
		public void FitImageInAvailableSpace()
		{
			for (int pct = _zoomTrackBar.Maximum; pct >= _zoomTrackBar.Minimum; pct -= _zoomTrackBar.SmallChange)
			{
				var sz = GetScaledSize(pct);
				if (sz.Width <= _panelImage.ClientSize.Width && sz.Height <= _panelImage.ClientSize.Height)
				{
					_zoomTrackBar.Value = pct;
					return;
				}
			}

			_zoomTrackBar.Value = _zoomTrackBar.Minimum;
		}

		/// ------------------------------------------------------------------------------------
		void HandleImagePanelMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			var pct = _zoomTrackBar.Value;

			if (pct >= _clickZoomPercentages.Max())
				pct = _clickZoomPercentages.Min();
			else
				pct = _clickZoomPercentages.First(x => x > pct);

			_zoomTrackBar.Value = pct;

			if (!_zoomTrackBar.Focused)
				_zoomTrackBar.Focus();
		}

		/// ------------------------------------------------------------------------------------
		void HandleImagePanelScroll(object sender, ScrollEventArgs e)
		{
			_panelImage.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		void HandleImagePanelPaint(object sender, PaintEventArgs e)
		{
			if (_firstTimePaint)
			{
				_firstTimePaint = false;
				FitImageInAvailableSpace();
				return;
			}

			var sz = _panelImage.AutoScrollMinSize;

			var dx = (sz.Width > _panelImage.ClientSize.Width ?
				_panelImage.AutoScrollPosition.X : (_panelImage.ClientSize.Width - sz.Width) / 2);

			var dy = (sz.Height > _panelImage.ClientSize.Height ?
				_panelImage.AutoScrollPosition.Y : (_panelImage.ClientSize.Height - sz.Height) / 2);

			var rc = new Rectangle(new Point(dx, dy), sz);

			e.Graphics.DrawImage(_image, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomTrackBarValueChanged(object sender, EventArgs e)
		{
			var fmt = LocalizationManager.LocalizeString("ImageViewer.ZoomValueFormat", "{0}%");
			_labelZoomPercent.Text = string.Format(fmt, _zoomTrackBar.Value);
			_panelImage.AutoScrollMinSize = GetScaledSize(_zoomTrackBar.Value);
			_panelImage.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private Size GetScaledSize(int percent)
		{
			var scaleFactor = percent / 100f;
			return new Size((int)(_image.Width * scaleFactor), (int)(_image.Height * scaleFactor));
		}
	}
}
