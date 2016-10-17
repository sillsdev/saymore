using SIL.Windows.Forms.Widgets;

namespace SayMore.UI.LowLevelControls
{
	partial class ProgressControl
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
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._labelStatus = new SIL.Windows.Forms.Widgets.BetterLabel();
			this.SuspendLayout();
			// 
			// _progressBar
			// 
			this._progressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this._progressBar.Location = new System.Drawing.Point(0, 0);
			this._progressBar.Margin = new System.Windows.Forms.Padding(0);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(307, 17);
			this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this._progressBar.TabIndex = 2;
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._labelStatus.Location = new System.Drawing.Point(0, 20);
			this._labelStatus.Multiline = true;
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.ReadOnly = true;
			this._labelStatus.Size = new System.Drawing.Size(303, 58);
			this._labelStatus.TabIndex = 4;
			this._labelStatus.TabStop = false;
			this._labelStatus.Text = "Status";
			// 
			// ProgressControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._progressBar);
			this.Controls.Add(this._labelStatus);
			this.Name = "ProgressControl";
			this.Size = new System.Drawing.Size(307, 83);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar _progressBar;
		private BetterLabel _labelStatus;
	}
}
