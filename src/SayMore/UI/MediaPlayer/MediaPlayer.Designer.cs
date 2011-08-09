
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.MediaPlayer
{
	partial class MediaPlayer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._videoPanel = new SayMore.UI.MediaPlayer.VideoPanel();
			this._volumePopup = new SayMore.UI.LowLevelControls.VolumePopup();
			this._toolbarButtons = new System.Windows.Forms.ToolStrip();
			this._buttonPlay = new System.Windows.Forms.ToolStripButton();
			this._buttonPause = new System.Windows.Forms.ToolStripButton();
			this._buttonStop = new System.Windows.Forms.ToolStripButton();
			this._buttonVolume = new System.Windows.Forms.ToolStripSplitButton();
			this._labelTime = new System.Windows.Forms.ToolStripLabel();
			this._sliderTime = new SayMore.UI.LowLevelControls.Slider();
			this._videoPanel.SuspendLayout();
			this._toolbarButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// _panelContainer
			// 
			this._videoPanel.BackColor = System.Drawing.Color.Black;
			this._videoPanel.Controls.Add(this._volumePopup);
			this._videoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._videoPanel.Location = new System.Drawing.Point(0, 0);
			this._videoPanel.Name = "_panelContainer";
			this._videoPanel.Size = new System.Drawing.Size(305, 208);
			this._videoPanel.TabIndex = 10;
			// 
			// _volumePopup
			// 
			this._volumePopup.Location = new System.Drawing.Point(260, 31);
			this._volumePopup.Name = "_volumePopup";
			this._volumePopup.OwningDropDown = null;
			this._volumePopup.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
			this._volumePopup.Size = new System.Drawing.Size(33, 160);
			this._volumePopup.TabIndex = 0;
			this._volumePopup.VolumeLevel = 0F;
			this._volumePopup.VolumeChanged += new System.EventHandler(this.HandleVolumePopupValueChanged);
			// 
			// _toolbarButtons
			// 
			this._toolbarButtons.BackColor = System.Drawing.Color.Transparent;
			this._toolbarButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._toolbarButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolbarButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonPlay,
            this._buttonPause,
            this._buttonStop,
            this._buttonVolume,
            this._labelTime});
			this._toolbarButtons.Location = new System.Drawing.Point(0, 217);
			this._toolbarButtons.Name = "_toolbarButtons";
			this._toolbarButtons.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolbarButtons.Size = new System.Drawing.Size(305, 34);
			this._toolbarButtons.TabIndex = 12;
			this._toolbarButtons.Text = "toolStrip1";
			// 
			// _buttonPlay
			// 
			this._buttonPlay.AutoSize = false;
			this._buttonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonPlay.Image = global::SayMore.Properties.Resources.Play;
			this._buttonPlay.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this._buttonPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonPlay.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonPlay.Name = "_buttonPlay";
			this._buttonPlay.Size = new System.Drawing.Size(33, 33);
			this._buttonPlay.Text = "toolStripButton1";
			this._buttonPlay.ToolTipText = "Play";
			this._buttonPlay.Click += new System.EventHandler(this.HandleButtonPlayClick);
			// 
			// _buttonPause
			// 
			this._buttonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonPause.Image = global::SayMore.Properties.Resources.Pause;
			this._buttonPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonPause.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonPause.Name = "_buttonPause";
			this._buttonPause.Size = new System.Drawing.Size(33, 34);
			this._buttonPause.ToolTipText = "Pause";
			this._buttonPause.Click += new System.EventHandler(this.HandleButtonPauseClick);
			// 
			// _buttonStop
			// 
			this._buttonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonStop.Image = global::SayMore.Properties.Resources.Stop;
			this._buttonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonStop.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._buttonStop.Name = "_buttonStop";
			this._buttonStop.Size = new System.Drawing.Size(28, 33);
			this._buttonStop.ToolTipText = "Stop";
			this._buttonStop.Click += new System.EventHandler(this.HandleButtonStopClick);
			// 
			// _buttonVolume
			// 
			this._buttonVolume.AutoSize = false;
			this._buttonVolume.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonVolume.Image = global::SayMore.Properties.Resources.Volume;
			this._buttonVolume.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonVolume.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonVolume.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
			this._buttonVolume.Name = "_buttonVolume";
			this._buttonVolume.Size = new System.Drawing.Size(42, 32);
			this._buttonVolume.ToolTipText = "Volume";
			this._buttonVolume.ButtonClick += new System.EventHandler(this.HandleVolumeButtonClick);
			// 
			// _labelTime
			// 
			this._labelTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._labelTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._labelTime.ForeColor = System.Drawing.Color.White;
			this._labelTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelTime.Name = "_labelTime";
			this._labelTime.Size = new System.Drawing.Size(14, 29);
			this._labelTime.Text = "#";
			this._labelTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// _sliderTime
			// 
			this._sliderTime.BackColor = System.Drawing.Color.DimGray;
			this._sliderTime.Cursor = System.Windows.Forms.Cursors.Hand;
			this._sliderTime.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._sliderTime.Enabled = false;
			this._sliderTime.Location = new System.Drawing.Point(0, 208);
			this._sliderTime.Name = "_sliderTime";
			this._sliderTime.ShowTooltip = false;
			this._sliderTime.Size = new System.Drawing.Size(305, 9);
			this._sliderTime.TabIndex = 7;
			this._sliderTime.TrackThickness = 3;
			this._sliderTime.AfterUserMovingThumb += new System.EventHandler(this.HandleSliderTimeAfterUserMovingThumb);
			// 
			// MediaPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._videoPanel);
			this.Controls.Add(this._sliderTime);
			this.Controls.Add(this._toolbarButtons);
			this.Name = "MediaPlayer";
			this.Size = new System.Drawing.Size(305, 251);
			this._videoPanel.ResumeLayout(false);
			this._toolbarButtons.ResumeLayout(false);
			this._toolbarButtons.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private VideoPanel _videoPanel;
		private System.Windows.Forms.ToolStrip _toolbarButtons;
		private System.Windows.Forms.ToolStripButton _buttonPlay;
		private System.Windows.Forms.ToolStripButton _buttonPause;
		private System.Windows.Forms.ToolStripButton _buttonStop;
		private System.Windows.Forms.ToolStripLabel _labelTime;
		private Slider _sliderTime;
		private System.Windows.Forms.ToolStripSplitButton _buttonVolume;
		private VolumePopup _volumePopup;
	}
}
