using SayMore.UI.LowLevelControls;

namespace SayMore.Transcription.UI
{
	partial class TinyMediaPlayer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._buttonStop = new SayMore.UI.LowLevelControls.IconicButton();
			this._buttonPlay = new SayMore.UI.LowLevelControls.IconicButton();
			this.SuspendLayout();
			// 
			// _buttonStop
			// 
			this._buttonStop.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonStop.BackColor = System.Drawing.Color.Transparent;
			this._buttonStop.BackgroundImage = global::SayMore.Properties.Resources.StopSegment;
			this._buttonStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this._buttonStop.ImageHot = global::SayMore.Properties.Resources.StopSegment_Hot;
			this._buttonStop.ImageNormal = global::SayMore.Properties.Resources.StopSegment;
			this._buttonStop.Location = new System.Drawing.Point(108, 11);
			this._buttonStop.Name = "_buttonStop";
			this._buttonStop.Size = new System.Drawing.Size(19, 20);
			this._buttonStop.TabIndex = 1;
			// 
			// _buttonPlay
			// 
			this._buttonPlay.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonPlay.BackColor = System.Drawing.Color.Transparent;
			this._buttonPlay.BackgroundImage = global::SayMore.Properties.Resources.PlaySegment;
			this._buttonPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this._buttonPlay.ImageHot = global::SayMore.Properties.Resources.PlaySegment_Hot;
			this._buttonPlay.ImageNormal = global::SayMore.Properties.Resources.PlaySegment;
			this._buttonPlay.Location = new System.Drawing.Point(136, 11);
			this._buttonPlay.Name = "_buttonPlay";
			this._buttonPlay.Size = new System.Drawing.Size(19, 20);
			this._buttonPlay.TabIndex = 0;
			// 
			// TinyMediaPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.PaleGoldenrod;
			this.Controls.Add(this._buttonStop);
			this.Controls.Add(this._buttonPlay);
			this.Name = "TinyMediaPlayer";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this.Size = new System.Drawing.Size(159, 42);
			this.ResumeLayout(false);

		}

		#endregion

		private IconicButton _buttonPlay;
		private IconicButton _buttonStop;
	}
}
