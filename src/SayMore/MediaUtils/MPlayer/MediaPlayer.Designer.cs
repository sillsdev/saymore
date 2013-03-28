
using SayMore.UI.LowLevelControls;

namespace SayMore.Media.MPlayer
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
			this.components = new System.ComponentModel.Container();
			this._toolbarButtons = new System.Windows.Forms.ToolStrip();
			this._buttonPlay = new System.Windows.Forms.ToolStripButton();
			this._buttonPause = new System.Windows.Forms.ToolStripButton();
			this._buttonStop = new System.Windows.Forms.ToolStripButton();
			this._buttonVolume = new System.Windows.Forms.ToolStripSplitButton();
			this._labelTime = new System.Windows.Forms.ToolStripLabel();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._videoPanel = new SayMore.Media.MPlayer.VideoPanel();
			this._volumePopup = new SayMore.UI.LowLevelControls.VolumePopup();
			this._sliderTime = new SayMore.UI.LowLevelControls.Slider();
			this._toolbarButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._videoPanel.SuspendLayout();
			this.SuspendLayout();
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
			this.locExtender.SetLocalizableToolTip(this._toolbarButtons, null);
			this.locExtender.SetLocalizationComment(this._toolbarButtons, null);
			this.locExtender.SetLocalizationPriority(this._toolbarButtons, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolbarButtons, "UI.MediaPlayer._toolbarButtons");
			this._toolbarButtons.Location = new System.Drawing.Point(0, 217);
			this._toolbarButtons.Name = "_toolbarButtons";
			this._toolbarButtons.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolbarButtons.Size = new System.Drawing.Size(318, 34);
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
			this.locExtender.SetLocalizableToolTip(this._buttonPlay, "Play");
			this.locExtender.SetLocalizationComment(this._buttonPlay, null);
			this.locExtender.SetLocalizingId(this._buttonPlay, "CommonToMultipleViews.MediaPlayer.PlayButton");
			this._buttonPlay.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonPlay.Name = "_buttonPlay";
			this._buttonPlay.Size = new System.Drawing.Size(33, 33);
			this._buttonPlay.Text = "Play";
			this._buttonPlay.ToolTipText = "Play";
			this._buttonPlay.Click += new System.EventHandler(this.HandleButtonPlayClick);
			// 
			// _buttonPause
			// 
			this._buttonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonPause.Image = global::SayMore.Properties.Resources.Pause;
			this._buttonPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonPause, "Pause");
			this.locExtender.SetLocalizationComment(this._buttonPause, null);
			this.locExtender.SetLocalizingId(this._buttonPause, "CommonToMultipleViews.MediaPlayer.PauseButton");
			this._buttonPause.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonPause.Name = "_buttonPause";
			this._buttonPause.Size = new System.Drawing.Size(33, 34);
			this._buttonPause.Text = "Pause";
			this._buttonPause.Click += new System.EventHandler(this.HandleButtonPauseClick);
			// 
			// _buttonStop
			// 
			this._buttonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonStop.Image = global::SayMore.Properties.Resources.Stop;
			this._buttonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonStop, "Stop");
			this.locExtender.SetLocalizationComment(this._buttonStop, null);
			this.locExtender.SetLocalizingId(this._buttonStop, "CommonToMultipleViews.MediaPlayer.StopButton");
			this._buttonStop.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._buttonStop.Name = "_buttonStop";
			this._buttonStop.Size = new System.Drawing.Size(28, 33);
			this._buttonStop.Text = "Stop";
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
			this.locExtender.SetLocalizableToolTip(this._buttonVolume, "Volume");
			this.locExtender.SetLocalizationComment(this._buttonVolume, null);
			this.locExtender.SetLocalizingId(this._buttonVolume, "CommonToMultipleViews.MediaPlayer.VolumeButton");
			this._buttonVolume.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
			this._buttonVolume.Name = "_buttonVolume";
			this._buttonVolume.Size = new System.Drawing.Size(42, 32);
			this._buttonVolume.Text = "Volume";
			this._buttonVolume.ToolTipText = "Volume";
			this._buttonVolume.ButtonClick += new System.EventHandler(this.HandleVolumeButtonClick);
			// 
			// _labelTime
			// 
			this._labelTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._labelTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._labelTime.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelTime, null);
			this.locExtender.SetLocalizationComment(this._labelTime, null);
			this.locExtender.SetLocalizationPriority(this._labelTime, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelTime, "UI.MediaPlayer._labelTime");
			this._labelTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelTime.Name = "_labelTime";
			this._labelTime.Size = new System.Drawing.Size(14, 29);
			this._labelTime.Text = "#";
			this._labelTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _videoPanel
			// 
			this._videoPanel.BackColor = System.Drawing.Color.Black;
			this._videoPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._videoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._videoPanel.ClipTextForChildControls = true;
			this._videoPanel.ControlReceivingFocusOnMnemonic = null;
			this._videoPanel.Controls.Add(this._volumePopup);
			this._videoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._videoPanel.DoubleBuffered = true;
			this._videoPanel.DrawOnlyBottomBorder = false;
			this._videoPanel.DrawOnlyTopBorder = false;
			this._videoPanel.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._videoPanel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._videoPanel, null);
			this.locExtender.SetLocalizationComment(this._videoPanel, null);
			this.locExtender.SetLocalizationPriority(this._videoPanel, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._videoPanel, "_videoPanel");
			this._videoPanel.Location = new System.Drawing.Point(0, 0);
			this._videoPanel.MnemonicGeneratesClick = false;
			this._videoPanel.Name = "_videoPanel";
			this._videoPanel.PaintExplorerBarBackground = false;
			this._videoPanel.Size = new System.Drawing.Size(318, 208);
			this._videoPanel.TabIndex = 10;
			// 
			// _volumePopup
			// 
			this.locExtender.SetLocalizableToolTip(this._volumePopup, null);
			this.locExtender.SetLocalizationComment(this._volumePopup, null);
			this.locExtender.SetLocalizationPriority(this._volumePopup, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._volumePopup, "UI.MediaPlayer.VolumePopup");
			this._volumePopup.Location = new System.Drawing.Point(260, 31);
			this._volumePopup.Name = "_volumePopup";
			this._volumePopup.OwningDropDown = null;
			this._volumePopup.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
			this._volumePopup.Size = new System.Drawing.Size(33, 160);
			this._volumePopup.TabIndex = 0;
			this._volumePopup.VolumeLevel = 0F;
			this._volumePopup.VolumeChanged += new System.EventHandler(this.HandleVolumePopupValueChanged);
			// 
			// _sliderTime
			// 
			this._sliderTime.BackColor = System.Drawing.Color.DimGray;
			this._sliderTime.Cursor = System.Windows.Forms.Cursors.Hand;
			this._sliderTime.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._sliderTime.Enabled = false;
			this.locExtender.SetLocalizableToolTip(this._sliderTime, null);
			this.locExtender.SetLocalizationComment(this._sliderTime, null);
			this.locExtender.SetLocalizationPriority(this._sliderTime, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._sliderTime, "UI.MediaPlayer._sliderTime");
			this._sliderTime.Location = new System.Drawing.Point(0, 208);
			this._sliderTime.Name = "_sliderTime";
			this._sliderTime.ShowTooltip = false;
			this._sliderTime.Size = new System.Drawing.Size(318, 9);
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
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SayMore");
			this.Name = "MediaPlayer";
			this.Size = new System.Drawing.Size(318, 251);
			this._toolbarButtons.ResumeLayout(false);
			this._toolbarButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._videoPanel.ResumeLayout(false);
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
		private L10NSharp.UI.LocalizationExtender locExtender;
	}
}
