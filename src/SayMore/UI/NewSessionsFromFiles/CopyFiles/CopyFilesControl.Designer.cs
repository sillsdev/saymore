using Palaso.UI.WindowsForms.Widgets;

namespace SayMore.UI.NewSessionsFromFiles
{
	partial class CopyFilesControl
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
			this.components = new System.ComponentModel.Container();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this._statusLabel = new Palaso.UI.WindowsForms.Widgets.BetterLabel();
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
			// _timer
			// 
			this._timer.Enabled = true;
			this._timer.Interval = 300;
			this._timer.Tick += new System.EventHandler(this._timer_Tick);
			// 
			// _statusLabel
			// 
			this._statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._statusLabel.Location = new System.Drawing.Point(3, 20);
			this._statusLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this._statusLabel.Multiline = true;
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.ReadOnly = true;
			this._statusLabel.Size = new System.Drawing.Size(298, 58);
			this._statusLabel.TabIndex = 4;
			this._statusLabel.TabStop = false;
			this._statusLabel.Text = "Status";
			// 
			// CopyFilesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._progressBar);
			this.Controls.Add(this._statusLabel);
			this.Name = "CopyFilesControl";
			this.Size = new System.Drawing.Size(307, 83);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Timer _timer;
		private BetterLabel _statusLabel;
	}
}
