using SIL.Sponge.Controls;

namespace SIL.Sponge
{
	partial class PeopleVw
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lpPeople = new SIL.Sponge.Controls.ListPanel();
			this.tabPeople = new System.Windows.Forms.TabControl();
			this.tpgAbout = new System.Windows.Forms.TabPage();
			this.silGrid1 = new SilUtils.SilGrid();
			this.colField = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.textBox8 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox7 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tpgContributors = new System.Windows.Forms.TabPage();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.Panel1.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.tabPeople.SuspendLayout();
			this.tpgAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.silGrid1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.lpPeople);
			// 
			// splitRightSide
			// 
			// 
			// splitRightSide.Panel1
			// 
			this.splitRightSide.Panel1.Controls.Add(this.tabPeople);
			this.splitRightSide.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.splitRightSide.Panel2Collapsed = true;
			// 
			// lpPeople
			// 
			this.lpPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lpPeople.Location = new System.Drawing.Point(0, 0);
			this.lpPeople.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpPeople.Name = "lpPeople";
			this.lpPeople.Size = new System.Drawing.Size(171, 383);
			this.lpPeople.TabIndex = 0;
			this.lpPeople.Text = "People";
			// 
			// tabPeople
			// 
			this.tabPeople.Controls.Add(this.tpgAbout);
			this.tabPeople.Controls.Add(this.tpgContributors);
			this.tabPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPeople.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPeople.ItemSize = new System.Drawing.Size(65, 22);
			this.tabPeople.Location = new System.Drawing.Point(0, 3);
			this.tabPeople.Name = "tabPeople";
			this.tabPeople.SelectedIndex = 0;
			this.tabPeople.Size = new System.Drawing.Size(482, 380);
			this.tabPeople.TabIndex = 0;
			this.tabPeople.SizeChanged += new System.EventHandler(this.tabPeople_SizeChanged);
			// 
			// tpgAbout
			// 
			this.tpgAbout.Controls.Add(this.silGrid1);
			this.tpgAbout.Controls.Add(this.textBox8);
			this.tpgAbout.Controls.Add(this.label8);
			this.tpgAbout.Controls.Add(this.textBox7);
			this.tpgAbout.Controls.Add(this.label7);
			this.tpgAbout.Controls.Add(this.textBox6);
			this.tpgAbout.Controls.Add(this.label6);
			this.tpgAbout.Controls.Add(this.textBox5);
			this.tpgAbout.Controls.Add(this.label5);
			this.tpgAbout.Controls.Add(this.textBox3);
			this.tpgAbout.Controls.Add(this.label3);
			this.tpgAbout.Controls.Add(this.textBox4);
			this.tpgAbout.Controls.Add(this.label4);
			this.tpgAbout.Controls.Add(this.textBox2);
			this.tpgAbout.Controls.Add(this.label2);
			this.tpgAbout.Controls.Add(this.pictureBox1);
			this.tpgAbout.Controls.Add(this.textBox1);
			this.tpgAbout.Controls.Add(this.label1);
			this.tpgAbout.Location = new System.Drawing.Point(4, 26);
			this.tpgAbout.Name = "tpgAbout";
			this.tpgAbout.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAbout.Size = new System.Drawing.Size(474, 350);
			this.tpgAbout.TabIndex = 0;
			this.tpgAbout.Text = "About";
			this.tpgAbout.ToolTipText = "Description";
			this.tpgAbout.UseVisualStyleBackColor = true;
			// 
			// silGrid1
			// 
			this.silGrid1.AllowUserToAddRows = false;
			this.silGrid1.AllowUserToDeleteRows = false;
			this.silGrid1.AllowUserToOrderColumns = true;
			this.silGrid1.AllowUserToResizeRows = false;
			this.silGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.silGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.silGrid1.BackgroundColor = System.Drawing.SystemColors.Window;
			this.silGrid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.silGrid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this.silGrid1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.silGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.silGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.silGrid1.ColumnHeadersVisible = false;
			this.silGrid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colField,
            this.colData});
			this.silGrid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.silGrid1.GridColor = System.Drawing.Color.PaleGoldenrod;
			this.silGrid1.IsDirty = false;
			this.silGrid1.Location = new System.Drawing.Point(6, 6);
			this.silGrid1.MultiSelect = false;
			this.silGrid1.Name = "silGrid1";
			this.silGrid1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.silGrid1.RowHeadersVisible = false;
			this.silGrid1.RowHeadersWidth = 22;
			this.silGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.silGrid1.ShowWaterMarkWhenDirty = false;
			this.silGrid1.Size = new System.Drawing.Size(323, 336);
			this.silGrid1.TabIndex = 16;
			this.silGrid1.WaterMark = "!";
			// 
			// colField
			// 
			this.colField.Frozen = true;
			this.colField.HeaderText = "";
			this.colField.Name = "colField";
			this.colField.ReadOnly = true;
			// 
			// colData
			// 
			this.colData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colData.FillWeight = 65F;
			this.colData.HeaderText = "";
			this.colData.Name = "colData";
			// 
			// textBox8
			// 
			this.textBox8.Location = new System.Drawing.Point(420, 261);
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new System.Drawing.Size(33, 23);
			this.textBox8.TabIndex = 11;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(296, 264);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(116, 15);
			this.label8.TabIndex = 10;
			this.label8.Text = "Place of L1 Learning:";
			// 
			// textBox7
			// 
			this.textBox7.Location = new System.Drawing.Point(420, 232);
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new System.Drawing.Size(33, 23);
			this.textBox7.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(296, 235);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(63, 15);
			this.label7.TabIndex = 8;
			this.label7.Text = "Education:";
			// 
			// textBox6
			// 
			this.textBox6.Location = new System.Drawing.Point(420, 203);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(33, 23);
			this.textBox6.TabIndex = 7;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(296, 206);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 15);
			this.label6.TabIndex = 6;
			this.label6.Text = "Gender:";
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(420, 174);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(33, 23);
			this.textBox5.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(296, 177);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(31, 15);
			this.label5.TabIndex = 4;
			this.label5.Text = "Age:";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(420, 319);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(33, 23);
			this.textBox3.TabIndex = 15;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(296, 322);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 15);
			this.label3.TabIndex = 14;
			this.label3.Text = "Mother\'s Language:";
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(420, 290);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(33, 23);
			this.textBox4.TabIndex = 13;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(296, 293);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(106, 15);
			this.label4.TabIndex = 12;
			this.label4.Text = "Father\'s Language:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(420, 145);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(33, 23);
			this.textBox2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(296, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Full Name:";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Image = global::SIL.Sponge.Properties.Resources.kimidNoPhoto;
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(340, 6);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(128, 128);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(420, 116);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(33, 23);
			this.textBox1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(296, 119);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Short Name:";
			// 
			// tpgContributors
			// 
			this.tpgContributors.Location = new System.Drawing.Point(4, 26);
			this.tpgContributors.Name = "tpgContributors";
			this.tpgContributors.Padding = new System.Windows.Forms.Padding(3);
			this.tpgContributors.Size = new System.Drawing.Size(474, 350);
			this.tpgContributors.TabIndex = 1;
			this.tpgContributors.Text = "Contributors && Permissions";
			this.tpgContributors.ToolTipText = "Contributors & Permissions";
			this.tpgContributors.UseVisualStyleBackColor = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.Frozen = true;
			this.dataGridViewTextBoxColumn1.HeaderText = "";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.FillWeight = 65F;
			this.dataGridViewTextBoxColumn2.HeaderText = "";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// PeopleVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "PeopleVw";
			this.ShowRightBottomPanel = false;
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.Panel1.ResumeLayout(false);
			this.splitRightSide.ResumeLayout(false);
			this.tabPeople.ResumeLayout(false);
			this.tpgAbout.ResumeLayout(false);
			this.tpgAbout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.silGrid1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private ListPanel lpPeople;
		private System.Windows.Forms.TabControl tabPeople;
		private System.Windows.Forms.TabPage tpgAbout;
		private System.Windows.Forms.TabPage tpgContributors;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox8;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textBox7;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.Label label6;
		private SilUtils.SilGrid silGrid1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colField;
		private System.Windows.Forms.DataGridViewTextBoxColumn colData;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
	}
}
