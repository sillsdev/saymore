namespace Sponge2.UI.ComponentEditors
{
	partial class AudioComponentEditor
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
			this._labelRecordist = new System.Windows.Forms.Label();
			this._recordist = new Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._notes = new Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox();
			this._labelNotes = new System.Windows.Forms.Label();
			this._microphone = new Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox();
			this._labelMicrophone = new System.Windows.Forms.Label();
			this._device = new Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox();
			this._labelDevice = new System.Windows.Forms.Label();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _labelRecordist
			// 
			this._labelRecordist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordist.AutoSize = true;
			this._labelRecordist.BackColor = System.Drawing.Color.Transparent;
			this._labelRecordist.Location = new System.Drawing.Point(16, 6);
			this._labelRecordist.Margin = new System.Windows.Forms.Padding(5, 6, 3, 0);
			this._labelRecordist.Name = "_labelRecordist";
			this._labelRecordist.Size = new System.Drawing.Size(55, 13);
			this._labelRecordist.TabIndex = 1;
			this._labelRecordist.Text = "Recordist:";
			// 
			// _recordist
			// 
			this._recordist.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._recordist.Dock = System.Windows.Forms.DockStyle.Top;
			this._recordist.FocusedBackColor = System.Drawing.SystemColors.Window;
			this._recordist.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._recordist.InnerTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._recordist.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._recordist.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._recordist.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._recordist.InnerTextBox.Name = "_txtBox";
			this._recordist.InnerTextBox.Size = new System.Drawing.Size(413, 13);
			this._recordist.InnerTextBox.TabIndex = 0;
			this._recordist.Location = new System.Drawing.Point(74, 5);
			this._recordist.Margin = new System.Windows.Forms.Padding(0, 5, 5, 1);
			this._recordist.Multiline = false;
			this._recordist.Name = "_recordist";
			this._recordist.Padding = new System.Windows.Forms.Padding(1);
			this._recordist.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this._recordist.Size = new System.Drawing.Size(415, 15);
			this._recordist.TabIndex = 0;
			this._recordist.UnfocusedBackColor = System.Drawing.SystemColors.Window;
			this._recordist.UnfocusedBorderColor = System.Drawing.Color.Transparent;
			// 
			// _tableLayout
			// 
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._notes, 1, 3);
			this._tableLayout.Controls.Add(this._labelNotes, 0, 3);
			this._tableLayout.Controls.Add(this._microphone, 1, 2);
			this._tableLayout.Controls.Add(this._labelMicrophone, 0, 2);
			this._tableLayout.Controls.Add(this._device, 1, 1);
			this._tableLayout.Controls.Add(this._labelDevice, 0, 1);
			this._tableLayout.Controls.Add(this._recordist, 1, 0);
			this._tableLayout.Controls.Add(this._labelRecordist, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 4;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.Size = new System.Drawing.Size(494, 334);
			this._tableLayout.TabIndex = 1;
			// 
			// _notes
			// 
			this._notes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._notes.Dock = System.Windows.Forms.DockStyle.Top;
			this._notes.FocusedBackColor = System.Drawing.SystemColors.Window;
			this._notes.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._notes.InnerTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._notes.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._notes.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._notes.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._notes.InnerTextBox.Multiline = true;
			this._notes.InnerTextBox.Name = "_txtBox";
			this._notes.InnerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._notes.InnerTextBox.Size = new System.Drawing.Size(413, 56);
			this._notes.InnerTextBox.TabIndex = 0;
			this._notes.Location = new System.Drawing.Point(74, 53);
			this._notes.Margin = new System.Windows.Forms.Padding(0, 0, 5, 1);
			this._notes.Multiline = true;
			this._notes.Name = "_notes";
			this._notes.Padding = new System.Windows.Forms.Padding(1);
			this._notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._notes.Size = new System.Drawing.Size(415, 58);
			this._notes.TabIndex = 4;
			this._notes.UnfocusedBackColor = System.Drawing.SystemColors.Window;
			this._notes.UnfocusedBorderColor = System.Drawing.Color.Transparent;
			// 
			// _labelNotes
			// 
			this._labelNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelNotes.AutoSize = true;
			this._labelNotes.BackColor = System.Drawing.Color.Transparent;
			this._labelNotes.Location = new System.Drawing.Point(33, 54);
			this._labelNotes.Margin = new System.Windows.Forms.Padding(5, 1, 3, 0);
			this._labelNotes.Name = "_labelNotes";
			this._labelNotes.Size = new System.Drawing.Size(38, 13);
			this._labelNotes.TabIndex = 4;
			this._labelNotes.Text = "Notes:";
			// 
			// _microphone
			// 
			this._microphone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._microphone.Dock = System.Windows.Forms.DockStyle.Top;
			this._microphone.FocusedBackColor = System.Drawing.SystemColors.Window;
			this._microphone.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._microphone.InnerTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._microphone.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._microphone.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._microphone.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._microphone.InnerTextBox.Name = "_txtBox";
			this._microphone.InnerTextBox.Size = new System.Drawing.Size(413, 13);
			this._microphone.InnerTextBox.TabIndex = 0;
			this._microphone.Location = new System.Drawing.Point(74, 37);
			this._microphone.Margin = new System.Windows.Forms.Padding(0, 0, 5, 1);
			this._microphone.Multiline = false;
			this._microphone.Name = "_microphone";
			this._microphone.Padding = new System.Windows.Forms.Padding(1);
			this._microphone.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this._microphone.Size = new System.Drawing.Size(415, 15);
			this._microphone.TabIndex = 3;
			this._microphone.UnfocusedBackColor = System.Drawing.SystemColors.Window;
			this._microphone.UnfocusedBorderColor = System.Drawing.Color.Transparent;
			// 
			// _labelMicrophone
			// 
			this._labelMicrophone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelMicrophone.AutoSize = true;
			this._labelMicrophone.BackColor = System.Drawing.Color.Transparent;
			this._labelMicrophone.Location = new System.Drawing.Point(5, 38);
			this._labelMicrophone.Margin = new System.Windows.Forms.Padding(5, 1, 3, 0);
			this._labelMicrophone.Name = "_labelMicrophone";
			this._labelMicrophone.Size = new System.Drawing.Size(66, 13);
			this._labelMicrophone.TabIndex = 3;
			this._labelMicrophone.Text = "Microphone:";
			// 
			// _device
			// 
			this._device.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._device.Dock = System.Windows.Forms.DockStyle.Top;
			this._device.FocusedBackColor = System.Drawing.SystemColors.Window;
			this._device.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._device.InnerTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._device.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._device.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._device.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._device.InnerTextBox.Name = "_txtBox";
			this._device.InnerTextBox.Size = new System.Drawing.Size(413, 13);
			this._device.InnerTextBox.TabIndex = 0;
			this._device.Location = new System.Drawing.Point(74, 21);
			this._device.Margin = new System.Windows.Forms.Padding(0, 0, 5, 1);
			this._device.Multiline = false;
			this._device.Name = "_device";
			this._device.Padding = new System.Windows.Forms.Padding(1);
			this._device.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this._device.Size = new System.Drawing.Size(415, 15);
			this._device.TabIndex = 2;
			this._device.UnfocusedBackColor = System.Drawing.SystemColors.Window;
			this._device.UnfocusedBorderColor = System.Drawing.Color.Transparent;
			// 
			// _labelDevice
			// 
			this._labelDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelDevice.AutoSize = true;
			this._labelDevice.BackColor = System.Drawing.Color.Transparent;
			this._labelDevice.Location = new System.Drawing.Point(27, 22);
			this._labelDevice.Margin = new System.Windows.Forms.Padding(5, 1, 3, 0);
			this._labelDevice.Name = "_labelDevice";
			this._labelDevice.Size = new System.Drawing.Size(44, 13);
			this._labelDevice.TabIndex = 2;
			this._labelDevice.Text = "Device:";
			// 
			// AudioComponentEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Name = "AudioComponentEditor";
			this.Size = new System.Drawing.Size(508, 348);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox _recordist;
		private System.Windows.Forms.Label _labelRecordist;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox _device;
		private System.Windows.Forms.Label _labelDevice;
		private Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox _notes;
		private System.Windows.Forms.Label _labelNotes;
		private Sponge2.UI.LowLevelControls.VisuallyDynamicTextBox _microphone;
		private System.Windows.Forms.Label _labelMicrophone;
	}
}
