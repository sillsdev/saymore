using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class TextAnnotationEditor
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._splitter = new System.Windows.Forms.SplitContainer();
			this._buttonExport = new System.Windows.Forms.Button();
			this._tableLayoutPlaybackSpeed = new System.Windows.Forms.TableLayoutPanel();
			this._comboPlaybackSpeed = new System.Windows.Forms.ComboBox();
			this._labelPlaybackSpeed = new System.Windows.Forms.Label();
			this._buttonHelp = new SilTools.Controls.ImageButton();
			this.button1 = new System.Windows.Forms.Button();
			this._tableLayout.SuspendLayout();
			this._splitter.SuspendLayout();
			this._tableLayoutPlaybackSpeed.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 3;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._splitter, 0, 1);
			this._tableLayout.Controls.Add(this._buttonExport, 1, 0);
			this._tableLayout.Controls.Add(this._tableLayoutPlaybackSpeed, 0, 0);
			this._tableLayout.Controls.Add(this._buttonHelp, 2, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(12, 12);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(425, 255);
			this._tableLayout.TabIndex = 0;
			// 
			// _splitter
			// 
			this._splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._splitter, 3);
			this._splitter.Location = new System.Drawing.Point(0, 34);
			this._splitter.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._splitter.Name = "_splitter";
			this._splitter.Size = new System.Drawing.Size(425, 221);
			this._splitter.SplitterDistance = 138;
			this._splitter.SplitterWidth = 8;
			this._splitter.TabIndex = 3;
			// 
			// _buttonExport
			// 
			this._buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonExport.AutoSize = true;
			this._buttonExport.Image = global::SayMore.Properties.Resources.InterlinearExport;
			this._buttonExport.Location = new System.Drawing.Point(310, 0);
			this._buttonExport.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this._buttonExport.Name = "_buttonExport";
			this._buttonExport.Size = new System.Drawing.Size(85, 26);
			this._buttonExport.TabIndex = 1;
			this._buttonExport.Text = "Export...";
			this._buttonExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonExport.UseVisualStyleBackColor = true;
			this._buttonExport.Click += new System.EventHandler(this.HandleExportButtonClick);
			// 
			// _tableLayoutPlaybackSpeed
			// 
			this._tableLayoutPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._tableLayoutPlaybackSpeed.AutoSize = true;
			this._tableLayoutPlaybackSpeed.ColumnCount = 2;
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.Controls.Add(this._comboPlaybackSpeed, 0, 0);
			this._tableLayoutPlaybackSpeed.Controls.Add(this._labelPlaybackSpeed, 0, 0);
			this._tableLayoutPlaybackSpeed.Location = new System.Drawing.Point(0, 2);
			this._tableLayoutPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPlaybackSpeed.Name = "_tableLayoutPlaybackSpeed";
			this._tableLayoutPlaybackSpeed.RowCount = 1;
			this._tableLayoutPlaybackSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPlaybackSpeed.Size = new System.Drawing.Size(191, 21);
			this._tableLayoutPlaybackSpeed.TabIndex = 0;
			// 
			// _comboPlaybackSpeed
			// 
			this._comboPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._comboPlaybackSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboPlaybackSpeed.FormattingEnabled = true;
			this._comboPlaybackSpeed.Location = new System.Drawing.Point(94, 0);
			this._comboPlaybackSpeed.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._comboPlaybackSpeed.Name = "_comboPlaybackSpeed";
			this._comboPlaybackSpeed.Size = new System.Drawing.Size(97, 21);
			this._comboPlaybackSpeed.TabIndex = 1;
			// 
			// _labelPlaybackSpeed
			// 
			this._labelPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelPlaybackSpeed.AutoSize = true;
			this._labelPlaybackSpeed.Location = new System.Drawing.Point(0, 4);
			this._labelPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelPlaybackSpeed.Name = "_labelPlaybackSpeed";
			this._labelPlaybackSpeed.Size = new System.Drawing.Size(88, 13);
			this._labelPlaybackSpeed.TabIndex = 0;
			this._labelPlaybackSpeed.Text = "Playback &Speed:";
			// 
			// _buttonHelp
			// 
			this._buttonHelp.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonHelp.AutoSize = true;
			this._buttonHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonHelp.FlatAppearance.BorderSize = 0;
			this._buttonHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonHelp.Image = null;
			this._buttonHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this._buttonHelp.Location = new System.Drawing.Point(405, 3);
			this._buttonHelp.Margin = new System.Windows.Forms.Padding(0);
			this._buttonHelp.Name = "_buttonHelp";
			this._buttonHelp.ShowFocusRectangle = true;
			this._buttonHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonHelp.TabIndex = 2;
			this._buttonHelp.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(321, 267);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 11);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TextAnnotationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.button1);
			this.Controls.Add(this._tableLayout);
			this.Name = "TextAnnotationEditor";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.Size = new System.Drawing.Size(449, 279);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._splitter.ResumeLayout(false);
			this._tableLayoutPlaybackSpeed.ResumeLayout(false);
			this._tableLayoutPlaybackSpeed.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.SplitContainer _splitter;
		private System.Windows.Forms.Button _buttonExport;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPlaybackSpeed;
        private System.Windows.Forms.ComboBox _comboPlaybackSpeed;
        private System.Windows.Forms.Label _labelPlaybackSpeed;
		private ImageButton _buttonHelp;
		private System.Windows.Forms.Button button1;


	}
}
