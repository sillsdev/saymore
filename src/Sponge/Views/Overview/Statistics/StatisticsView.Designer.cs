namespace SIL.Sponge.Views
{
    partial class StatisticsView
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
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._refreshButton = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Location = new System.Drawing.Point(17, 27);
            this._table.Name = "_table";
            this._table.RowCount = 1;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(326, 122);
            this._table.TabIndex = 0;
            // 
            // _refreshButton
            // 
            this._refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._refreshButton.AutoSize = true;
            this._refreshButton.Location = new System.Drawing.Point(299, 11);
            this._refreshButton.Name = "_refreshButton";
            this._refreshButton.Size = new System.Drawing.Size(44, 13);
            this._refreshButton.TabIndex = 1;
            this._refreshButton.TabStop = true;
            this._refreshButton.Text = "Refresh";
            this._refreshButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnRefreshButtonClicked);
            // 
            // StatisticsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._refreshButton);
            this.Controls.Add(this._table);
            this.Name = "StatisticsView";
            this.Size = new System.Drawing.Size(363, 171);
            this.Load += new System.EventHandler(this.StatisticsView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.LinkLabel _refreshButton;
    }
}
