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
			System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
			this._labelLoading = new System.Windows.Forms.Label();
			this._pictureLoading = new System.Windows.Forms.PictureBox();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._linkCancel = new System.Windows.Forms.LinkLabel();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _labelLoading
			// 
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Dock = System.Windows.Forms.DockStyle.Left;
			this._labelLoading.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLoading, null);
			this.locExtender.SetLocalizationComment(this._labelLoading, null);
			this.locExtender.SetLocalizingId(this._labelLoading, "DialogBoxes.LoadingDlg._labelLoading");
			this._labelLoading.Location = new System.Drawing.Point(3, 0);
			this._labelLoading.MaximumSize = new System.Drawing.Size(220, 0);
			this._labelLoading.MinimumSize = new System.Drawing.Size(72, 0);
			this._labelLoading.Name = "_labelLoading";
			this._labelLoading.Size = new System.Drawing.Size(72, 36);
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
			this.locExtender.SetLocalizingId(this._pictureLoading, "DialogBoxes.LoadingDlg._pictureLoading");
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
			// _linkCancel
			// 
			this._linkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._linkCancel.AutoSize = true;
			this._linkCancel.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._linkCancel, null);
			this.locExtender.SetLocalizationComment(this._linkCancel, null);
			this.locExtender.SetLocalizingId(this._linkCancel, "DialogBoxes.LoadingDlg.CancelLink");
			this._linkCancel.Location = new System.Drawing.Point(102, 43);
			this._linkCancel.Name = "_linkCancel";
			this._linkCancel.Size = new System.Drawing.Size(40, 13);
			this._linkCancel.TabIndex = 3;
			this._linkCancel.TabStop = true;
			this._linkCancel.Text = "Cancel";
			this._linkCancel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this._linkCancel.Visible = false;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(this._labelLoading, 0, 0);
			tableLayoutPanel1.Controls.Add(this._linkCancel, 0, 1);
			tableLayoutPanel1.Location = new System.Drawing.Point(92, 15);
			tableLayoutPanel1.MinimumSize = new System.Drawing.Size(145, 56);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			tableLayoutPanel1.Size = new System.Drawing.Size(145, 56);
			tableLayoutPanel1.TabIndex = 4;
			// 
			// LoadingDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(245, 78);
			this.Controls.Add(tableLayoutPanel1);
			this.Controls.Add(this._pictureLoading);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.LoadingDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadingDlg";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 7);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.PictureBox _pictureLoading;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkCancel;
	}
}