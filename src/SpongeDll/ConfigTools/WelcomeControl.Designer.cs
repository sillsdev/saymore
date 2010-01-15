namespace SIL.Sponge.ConfigTools
{
    partial class WelcomeControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeControl));
			this._imageList = new System.Windows.Forms.ImageList(this.components);
			this._debounceListIndexChangedEvent = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lblTemplate = new System.Windows.Forms.Label();
			this.flwPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.btnTemplate = new System.Windows.Forms.Button();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblVersionInfo = new System.Windows.Forms.Label();
			this.lblProdInfo1 = new System.Windows.Forms.Label();
			this.lnkSIL = new System.Windows.Forms.LinkLabel();
			this.lblProdInfo2 = new System.Windows.Forms.Label();
			this.lnkSpongeWebSite = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// _imageList
			// 
			this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
			this._imageList.TransparentColor = System.Drawing.Color.Magenta;
			this._imageList.Images.SetKeyName(0, "browse");
			this._imageList.Images.SetKeyName(1, "getFromUsb");
			this._imageList.Images.SetKeyName(2, "wesayProject");
			this._imageList.Images.SetKeyName(3, "getFromInternet");
			this._imageList.Images.SetKeyName(4, "newProject");
			this._imageList.Images.SetKeyName(5, "flex");
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 300;
			// 
			// lblTemplate
			// 
			this.lblTemplate.AutoSize = true;
			this.lblTemplate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTemplate.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.lblTemplate.Location = new System.Drawing.Point(445, 9);
			this.lblTemplate.Name = "lblTemplate";
			this.lblTemplate.Size = new System.Drawing.Size(116, 20);
			this.lblTemplate.TabIndex = 7;
			this.lblTemplate.Text = "Template Label";
			this.lblTemplate.Visible = false;
			// 
			// flwPanel
			// 
			this.flwPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flwPanel.Location = new System.Drawing.Point(27, 101);
			this.flwPanel.Name = "flwPanel";
			this.flwPanel.Size = new System.Drawing.Size(557, 198);
			this.flwPanel.TabIndex = 8;
			// 
			// btnTemplate
			// 
			this.btnTemplate.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnTemplate.FlatAppearance.BorderSize = 0;
			this.btnTemplate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
			this.btnTemplate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
			this.btnTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTemplate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnTemplate.ImageKey = "wesayProject";
			this.btnTemplate.ImageList = this._imageList;
			this.btnTemplate.Location = new System.Drawing.Point(3, 9);
			this.btnTemplate.Name = "btnTemplate";
			this.btnTemplate.Size = new System.Drawing.Size(351, 26);
			this.btnTemplate.TabIndex = 6;
			this.btnTemplate.Text = "   templateButton";
			this.btnTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnTemplate.UseVisualStyleBackColor = true;
			this.btnTemplate.Visible = false;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.AutoSize = true;
			this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSubTitle.Location = new System.Drawing.Point(113, 66);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(300, 17);
			this.lblSubTitle.TabIndex = 9;
			this.lblSubTitle.Text = "Language Documentation Project Management";
			// 
			// lblVersionInfo
			// 
			this.lblVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVersionInfo.AutoSize = true;
			this.lblVersionInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersionInfo.Location = new System.Drawing.Point(27, 304);
			this.lblVersionInfo.Name = "lblVersionInfo";
			this.lblVersionInfo.Size = new System.Drawing.Size(14, 13);
			this.lblVersionInfo.TabIndex = 10;
			this.lblVersionInfo.Text = "#";
			// 
			// lblProdInfo1
			// 
			this.lblProdInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProdInfo1.AutoSize = true;
			this.lblProdInfo1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProdInfo1.Location = new System.Drawing.Point(27, 320);
			this.lblProdInfo1.Name = "lblProdInfo1";
			this.lblProdInfo1.Size = new System.Drawing.Size(128, 13);
			this.lblProdInfo1.TabIndex = 11;
			this.lblProdInfo1.Text = "Sponge is a product of ";
			// 
			// lnkSIL
			// 
			this.lnkSIL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkSIL.AutoSize = true;
			this.lnkSIL.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkSIL.Location = new System.Drawing.Point(149, 320);
			this.lnkSIL.Name = "lnkSIL";
			this.lnkSIL.Size = new System.Drawing.Size(94, 13);
			this.lnkSIL.TabIndex = 12;
			this.lnkSIL.TabStop = true;
			this.lnkSIL.Text = "SIL International.";
			// 
			// lblProdInfo2
			// 
			this.lblProdInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProdInfo2.AutoSize = true;
			this.lblProdInfo2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProdInfo2.Location = new System.Drawing.Point(247, 320);
			this.lblProdInfo2.Name = "lblProdInfo2";
			this.lblProdInfo2.Size = new System.Drawing.Size(49, 13);
			this.lblProdInfo2.TabIndex = 13;
			this.lblProdInfo2.Text = "Visit the";
			// 
			// lnkSpongeWebSite
			// 
			this.lnkSpongeWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkSpongeWebSite.AutoSize = true;
			this.lnkSpongeWebSite.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkSpongeWebSite.Location = new System.Drawing.Point(292, 320);
			this.lnkSpongeWebSite.Name = "lnkSpongeWebSite";
			this.lnkSpongeWebSite.Size = new System.Drawing.Size(96, 13);
			this.lnkSpongeWebSite.TabIndex = 14;
			this.lnkSpongeWebSite.TabStop = true;
			this.lnkSpongeWebSite.Text = "Sponge web site.";
			// 
			// WelcomeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.lnkSpongeWebSite);
			this.Controls.Add(this.lnkSIL);
			this.Controls.Add(this.lblProdInfo1);
			this.Controls.Add(this.lblVersionInfo);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.flwPanel);
			this.Controls.Add(this.lblTemplate);
			this.Controls.Add(this.btnTemplate);
			this.Controls.Add(this.lblProdInfo2);
			this.Name = "WelcomeControl";
			this.Size = new System.Drawing.Size(587, 338);
			this.Load += new System.EventHandler(this.WelcomeControl_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Timer _debounceListIndexChangedEvent;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Label lblTemplate;
		private System.Windows.Forms.FlowLayoutPanel flwPanel;
		private System.Windows.Forms.Label lblSubTitle;
		private System.Windows.Forms.Label lblVersionInfo;
		private System.Windows.Forms.Label lblProdInfo1;
		private System.Windows.Forms.LinkLabel lnkSIL;
		private System.Windows.Forms.Label lblProdInfo2;
		private System.Windows.Forms.LinkLabel lnkSpongeWebSite;

    }
}
