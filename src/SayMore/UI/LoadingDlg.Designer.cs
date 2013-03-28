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
			this._linkCancel = new System.Windows.Forms.LinkLabel();
			this._pictureLoading = new System.Windows.Forms.PictureBox();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _labelLoading
			// 
			this._labelLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLoading, null);
			this.locExtender.SetLocalizationComment(this._labelLoading, null);
			this.locExtender.SetLocalizingId(this._labelLoading, "DialogBoxes.LoadingDlg._labelLoading");
			this._labelLoading.Location = new System.Drawing.Point(71, 3);
			this._labelLoading.Margin = new System.Windows.Forms.Padding(3);
			this._labelLoading.Name = "_labelLoading";
			this._labelLoading.Size = new System.Drawing.Size(169, 54);
			this._labelLoading.TabIndex = 2;
			this._labelLoading.Text = "Loading...";
			this._labelLoading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _linkCancel
			// 
			this._linkCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._linkCancel.AutoSize = true;
			this._linkCancel.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._linkCancel, null);
			this.locExtender.SetLocalizationComment(this._linkCancel, null);
			this.locExtender.SetLocalizingId(this._linkCancel, "DialogBoxes.LoadingDlg.CancelLink");
			this._linkCancel.Location = new System.Drawing.Point(200, 63);
			this._linkCancel.Margin = new System.Windows.Forms.Padding(3);
			this._linkCancel.Name = "_linkCancel";
			this._linkCancel.Size = new System.Drawing.Size(40, 13);
			this._linkCancel.TabIndex = 3;
			this._linkCancel.TabStop = true;
			this._linkCancel.Text = "Cancel";
			this._linkCancel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this._linkCancel.Visible = false;
			// 
			// _pictureLoading
			// 
			this._pictureLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._pictureLoading.BackColor = System.Drawing.Color.Transparent;
			this._pictureLoading.Image = global::SayMore.Properties.Resources.BusyWheelLarge;
			this.locExtender.SetLocalizableToolTip(this._pictureLoading, null);
			this.locExtender.SetLocalizationComment(this._pictureLoading, null);
			this.locExtender.SetLocalizationPriority(this._pictureLoading, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureLoading, "DialogBoxes.LoadingDlg._pictureLoading");
			this._pictureLoading.Location = new System.Drawing.Point(10, 15);
			this._pictureLoading.Margin = new System.Windows.Forms.Padding(10);
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
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.AutoSize = true;
			this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._pictureLoading, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._linkCancel, 1, 1);
			this._tableLayoutPanel.Controls.Add(this._labelLoading, 1, 0);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(1, 1);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 2;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(243, 79);
			this._tableLayoutPanel.TabIndex = 4;
			// 
			// LoadingDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(245, 81);
			this.Controls.Add(this._tableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.LoadingDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadingDlg";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.Resize += new System.EventHandler(this.LoadingDlg_Resize);
			((System.ComponentModel.ISupportInitialize)(this._pictureLoading)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.PictureBox _pictureLoading;
		private L10NSharp.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkCancel;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
	}
}