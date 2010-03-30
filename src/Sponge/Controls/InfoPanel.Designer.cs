namespace SIL.Sponge.Controls
{
	partial class InfoPanel
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
			this.lblFile = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnMoreAction = new SilUtils.Controls.XButton();
			this.label1 = new System.Windows.Forms.Label();
			this._presetMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._presetMenuButton = new SilUtils.Controls.XButton();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// lblFile
			// 
			this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblFile.AutoEllipsis = true;
			this.lblFile.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFile.Location = new System.Drawing.Point(30, 0);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(504, 15);
			this.lblFile.TabIndex = 1;
			this.lblFile.Text = "#";
			// 
			// picIcon
			// 
			this.picIcon.Location = new System.Drawing.Point(0, 18);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 7;
			this.picIcon.TabStop = false;
			// 
			// btnMoreAction
			// 
			this.btnMoreAction.BackColor = System.Drawing.Color.Transparent;
			this.btnMoreAction.CanBeChecked = false;
			this.btnMoreAction.Checked = false;
			this.btnMoreAction.DrawEmpty = false;
			this.btnMoreAction.DrawLeftArrowButton = false;
			this.btnMoreAction.DrawRightArrowButton = false;
			this.btnMoreAction.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnMoreAction.Image = global::SIL.Sponge.Properties.Resources.kimidMoreAction;
			this.btnMoreAction.Location = new System.Drawing.Point(7, 0);
			this.btnMoreAction.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.btnMoreAction.Name = "btnMoreAction";
			this.btnMoreAction.Size = new System.Drawing.Size(16, 16);
			this.btnMoreAction.TabIndex = 0;
			this.btnMoreAction.Click += new System.EventHandler(this.btnMoreAction_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(38, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Presets";
			// 
			// _presetMenu
			// 
			this._presetMenu.Name = "_presetMenu";
			this._presetMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// _presetMenuButton
			// 
			this._presetMenuButton.BackColor = System.Drawing.Color.Transparent;
			this._presetMenuButton.CanBeChecked = false;
			this._presetMenuButton.Checked = false;
			this._presetMenuButton.ContextMenuStrip = this._presetMenu;
			this._presetMenuButton.DrawEmpty = false;
			this._presetMenuButton.DrawLeftArrowButton = false;
			this._presetMenuButton.DrawRightArrowButton = false;
			this._presetMenuButton.Font = new System.Drawing.Font("Marlett", 9F);
			this._presetMenuButton.Image = global::SIL.Sponge.Properties.Resources.kimidMoreAction;
			this._presetMenuButton.Location = new System.Drawing.Point(86, 26);
			this._presetMenuButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._presetMenuButton.Name = "_presetMenuButton";
			this._presetMenuButton.Size = new System.Drawing.Size(16, 16);
			this._presetMenuButton.TabIndex = 10;
			this._presetMenuButton.Click += new System.EventHandler(this.OnPresetButtonClick);
			// 
			// InfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._presetMenuButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.btnMoreAction);
			this.Controls.Add(this.lblFile);
			this.Name = "InfoPanel";
			this.Size = new System.Drawing.Size(538, 106);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picIcon;
		private SilUtils.Controls.XButton btnMoreAction;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenuStrip _presetMenu;
		private SilUtils.Controls.XButton _presetMenuButton;
	}
}
