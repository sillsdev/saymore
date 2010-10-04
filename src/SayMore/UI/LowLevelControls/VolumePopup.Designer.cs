namespace SayMore.UI.LowLevelControls
{
	partial class VolumePopup
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this._sliderVolume = new Slider();
			this.SuspendLayout();
			// 
			// _sliderVolume
			// 
			this._sliderVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this._sliderVolume.BackColor = System.Drawing.Color.Transparent;
			this._sliderVolume.Cursor = System.Windows.Forms.Cursors.Hand;
			this._sliderVolume.Location = new System.Drawing.Point(10, 8);
			this._sliderVolume.Name = "_sliderVolume";
			this._sliderVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this._sliderVolume.Size = new System.Drawing.Size(13, 144);
			this._sliderVolume.TabIndex = 0;
			this._sliderVolume.TooltipFormat = "{0:F0}%";
			this._sliderVolume.ValueChanged += new System.EventHandler(this.HandleSliderVolumeValueChanged);
			this._sliderVolume.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleSliderVolumeMouseUp);
			// 
			// VolumePopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._sliderVolume);
			this.Name = "VolumePopup";
			this.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
			this.Size = new System.Drawing.Size(33, 160);
			this.ResumeLayout(false);

		}

		#endregion

		private Slider _sliderVolume;
	}
}
