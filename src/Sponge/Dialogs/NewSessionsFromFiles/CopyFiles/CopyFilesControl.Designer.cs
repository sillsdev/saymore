namespace SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles
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
			this.betterLabel1 = new Palaso.UI.WindowsForms.Widgets.BetterLabel();
			this._statusLabel = new Palaso.UI.WindowsForms.Widgets.BetterLabel();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// betterLabel1
			// 
			this.betterLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.betterLabel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.betterLabel1.Location = new System.Drawing.Point(0, 0);
			this.betterLabel1.Multiline = true;
			this.betterLabel1.Name = "betterLabel1";
			this.betterLabel1.ReadOnly = true;
			this.betterLabel1.Size = new System.Drawing.Size(100, 20);
			this.betterLabel1.TabIndex = 0;
			this.betterLabel1.TabStop = false;
			// 
			// _statusLabel
			// 
			this._statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._statusLabel.Location = new System.Drawing.Point(17, 41);
			this._statusLabel.Multiline = true;
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.ReadOnly = true;
			this._statusLabel.Size = new System.Drawing.Size(454, 61);
			this._statusLabel.TabIndex = 1;
			this._statusLabel.TabStop = false;
			this._statusLabel.Text = "Status";
			// 
			// _progressBar
			// 
			this._progressBar.Location = new System.Drawing.Point(17, 12);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(432, 23);
			this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this._progressBar.TabIndex = 2;
			this._progressBar.Click += new System.EventHandler(this._progressBar_Click);
			// 
			// _timer
			// 
			this._timer.Enabled = true;
			this._timer.Interval = 300;
			this._timer.Tick += new System.EventHandler(this._timer_Tick);
			// 
			// CopyFilesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._progressBar);
			this.Controls.Add(this._statusLabel);
			this.Controls.Add(this.betterLabel1);
			this.Name = "CopyFilesControl";
			this.Size = new System.Drawing.Size(501, 117);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Palaso.UI.WindowsForms.Widgets.BetterLabel betterLabel1;
		private Palaso.UI.WindowsForms.Widgets.BetterLabel _statusLabel;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Timer _timer;
	}
}
