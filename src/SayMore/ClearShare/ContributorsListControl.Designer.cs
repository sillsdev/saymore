namespace SayMore.ClearShare
{
    partial class ContributorsListControl
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
            this._deleteButton = new System.Windows.Forms.Button();
            this._addButton = new System.Windows.Forms.Button();
            this._grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _deleteButton
            // 
            this._deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteButton.Location = new System.Drawing.Point(265, 223);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(75, 23);
            this._deleteButton.TabIndex = 0;
            this._deleteButton.Text = "&Delete";
            this._deleteButton.UseVisualStyleBackColor = true;
            // 
            // _addButton
            // 
            this._addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addButton.Location = new System.Drawing.Point(346, 223);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(96, 23);
            this._addButton.TabIndex = 1;
            this._addButton.Text = "&New Contribution";
            this._addButton.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Location = new System.Drawing.Point(3, 3);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(439, 214);
            this._grid.TabIndex = 2;
            // 
            // ContributorsListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._addButton);
            this.Controls.Add(this._deleteButton);
            this.Name = "ContributorsListControl";
            this.Size = new System.Drawing.Size(453, 256);
            this.Load += new System.EventHandler(this.ContributorsListControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.DataGridView _grid;
    }
}
