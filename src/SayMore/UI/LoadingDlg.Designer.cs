namespace SayMore.UI
{
	partial class LoadingDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._labelLoading = new System.Windows.Forms.Label();
			this._pictureLoading = new System.Windows.Forms.PictureBox();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelLoading
			// 
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLoading, null);
			this.locExtender.SetLocalizationComment(this._labelLoading, null);
			this.locExtender.SetLocalizingId(this._labelLoading, "UI.LoadingDlg._labelLoading");
			this._labelLoading.Location = new System.Drawing.Point(94, 31);
			this._labelLoading.Name = "_labelLoading";
			this._labelLoading.Size = new System.Drawing.Size(72, 16);
			this._labelLoading.TabIndex = 2;
			this._labelLoading.Text = "Loading...";
			// 
			// _pictureLoading
			// 
			this._pictureLoading.BackColor = System.Drawing.Color.Transparent;
			this._pictureLoading.Image = global::SayMore.Properties.Resources.BusyWheelLarge;
			this.locExtender.SetLocalizableToolTip(this._pictureLoading, null);
			this.locExtender.SetLocalizationComment(this._pictureLoading, null);
			this.locExtender.SetLocalizationPriority(this._pictureLoading, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureLoading, "UI.LoadingDlg._pictureLoading");
			this._pictureLoading.Location = new System.Drawing.Point(28, 15);
			this._pictureLoading.Name = "_pictureLoading";
			this._pictureLoading.Size = new System.Drawing.Size(48, 48);
			this._pictureLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureLoading.TabIndex = 1;
			this._pictureLoading.TabStop = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// LoadingDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(245, 78);
			this.Controls.Add(this._labelLoading);
			this.Controls.Add(this._pictureLoading);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "UI.LoadingDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadingDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.PictureBox _pictureLoading;
		private Localization.UI.LocalizationExtender locExtender;
	}
}