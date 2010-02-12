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
			this.flwFileInfo = new System.Windows.Forms.FlowLayoutPanel();
			this.lblNotes = new System.Windows.Forms.Label();
			this.lblFile = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnMoreAction = new SilUtils.Controls.XButton();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.hctNotes = new SIL.Sponge.Controls.HoverCueTextBox();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// flwFileInfo
			// 
			this.flwFileInfo.BackColor = System.Drawing.SystemColors.Control;
			this.flwFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flwFileInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flwFileInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.flwFileInfo.Location = new System.Drawing.Point(0, 0);
			this.flwFileInfo.Name = "flwFileInfo";
			this.flwFileInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.flwFileInfo.Size = new System.Drawing.Size(251, 88);
			this.flwFileInfo.TabIndex = 0;
			// 
			// lblNotes
			// 
			this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNotes.AutoSize = true;
			this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNotes.Location = new System.Drawing.Point(243, 0);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(41, 15);
			this.lblNotes.TabIndex = 2;
			this.lblNotes.Text = "Notes:";
			// 
			// lblFile
			// 
			this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblFile.AutoEllipsis = true;
			this.lblFile.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFile.Location = new System.Drawing.Point(30, 0);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(180, 15);
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
			this.btnMoreAction.Location = new System.Drawing.Point(8, 0);
			this.btnMoreAction.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.btnMoreAction.Name = "btnMoreAction";
			this.btnMoreAction.Size = new System.Drawing.Size(16, 16);
			this.btnMoreAction.TabIndex = 0;
			this.btnMoreAction.Click += new System.EventHandler(this.btnMoreAction_Click);
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(38, 18);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.flwFileInfo);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.hctNotes);
			this.splitContainer.Size = new System.Drawing.Size(443, 88);
			this.splitContainer.SplitterDistance = 251;
			this.splitContainer.TabIndex = 13;
			this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			// 
			// hctNotes
			// 
			this.hctNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.hctNotes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.hctNotes.DynamicBorder = false;
			this.hctNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hctNotes.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this.hctNotes.InnerTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.hctNotes.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.hctNotes.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hctNotes.InnerTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hctNotes.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this.hctNotes.InnerTextBox.Multiline = true;
			this.hctNotes.InnerTextBox.Name = "m_txtBox";
			this.hctNotes.InnerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.hctNotes.InnerTextBox.Size = new System.Drawing.Size(186, 169);
			this.hctNotes.InnerTextBox.TabIndex = 0;
			this.hctNotes.Location = new System.Drawing.Point(0, 0);
			this.hctNotes.Name = "hctNotes";
			this.hctNotes.Padding = new System.Windows.Forms.Padding(1);
			this.hctNotes.Size = new System.Drawing.Size(188, 88);
			this.hctNotes.TabIndex = 0;
			// 
			// InfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.btnMoreAction);
			this.Controls.Add(this.lblFile);
			this.Name = "InfoPanel";
			this.Size = new System.Drawing.Size(481, 106);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flwFileInfo;
		private HoverCueTextBox hctNotes;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.PictureBox picIcon;
		private SilUtils.Controls.XButton btnMoreAction;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.SplitContainer splitContainer;
	}
}
