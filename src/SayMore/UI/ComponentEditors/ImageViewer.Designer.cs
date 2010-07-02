namespace SayMore.UI.ComponentEditors
{
	partial class ImageViewer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// ------------------------------------------------------------------------------------
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
			this._zoomTrackBar = new System.Windows.Forms.TrackBar();
			this._tableLayoutZoom = new System.Windows.Forms.TableLayoutPanel();
			this._labelZoom = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._zoomTrackBar)).BeginInit();
			this._tableLayoutZoom.SuspendLayout();
			this.SuspendLayout();
			// 
			// _zoomTrackBar
			// 
			this._zoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this._zoomTrackBar.LargeChange = 25;
			this._zoomTrackBar.Location = new System.Drawing.Point(1, 16);
			this._zoomTrackBar.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._zoomTrackBar.Maximum = 1000;
			this._zoomTrackBar.Minimum = 10;
			this._zoomTrackBar.Name = "_zoomTrackBar";
			this._zoomTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
			this._zoomTrackBar.Size = new System.Drawing.Size(45, 246);
			this._zoomTrackBar.SmallChange = 5;
			this._zoomTrackBar.TabIndex = 0;
			this._zoomTrackBar.TickFrequency = 100;
			this._zoomTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			this._zoomTrackBar.Value = 10;
			this._zoomTrackBar.ValueChanged += new System.EventHandler(this.HandleZoomTrackBarValueChanged);
			// 
			// _tableLayoutZoom
			// 
			this._tableLayoutZoom.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutZoom.ColumnCount = 1;
			this._tableLayoutZoom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutZoom.Controls.Add(this._labelZoom, 0, 0);
			this._tableLayoutZoom.Controls.Add(this._zoomTrackBar, 0, 1);
			this._tableLayoutZoom.Dock = System.Windows.Forms.DockStyle.Right;
			this._tableLayoutZoom.Location = new System.Drawing.Point(394, 7);
			this._tableLayoutZoom.Name = "_tableLayoutZoom";
			this._tableLayoutZoom.RowCount = 2;
			this._tableLayoutZoom.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutZoom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutZoom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutZoom.Size = new System.Drawing.Size(48, 265);
			this._tableLayoutZoom.TabIndex = 1;
			// 
			// _labelZoom
			// 
			this._labelZoom.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelZoom.AutoSize = true;
			this._labelZoom.Location = new System.Drawing.Point(7, 0);
			this._labelZoom.Name = "_labelZoom";
			this._labelZoom.Size = new System.Drawing.Size(34, 13);
			this._labelZoom.TabIndex = 0;
			this._labelZoom.Text = "Zoom";
			this._labelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ImageViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutZoom);
			this.Name = "ImageViewer";
			this.Size = new System.Drawing.Size(449, 279);
			((System.ComponentModel.ISupportInitialize)(this._zoomTrackBar)).EndInit();
			this._tableLayoutZoom.ResumeLayout(false);
			this._tableLayoutZoom.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TrackBar _zoomTrackBar;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutZoom;
		private System.Windows.Forms.Label _labelZoom;
	}
}
