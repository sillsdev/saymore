namespace Sponge2.UI
{
	partial class SessionsControl
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
			this._newSessionButton = new System.Windows.Forms.Button();
			this._sessionListView = new System.Windows.Forms.ListView();
			this._componentsListView = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _newSessionButton
			// 
			this._newSessionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._newSessionButton.Location = new System.Drawing.Point(19, 320);
			this._newSessionButton.Name = "_newSessionButton";
			this._newSessionButton.Size = new System.Drawing.Size(99, 23);
			this._newSessionButton.TabIndex = 2;
			this._newSessionButton.Text = "New Session";
			this._newSessionButton.UseVisualStyleBackColor = true;
			this._newSessionButton.Click += new System.EventHandler(this.OnNewSessionButtonClick);
			// 
			// _sessionListView
			// 
			this._sessionListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this._sessionListView.HideSelection = false;
			this._sessionListView.Location = new System.Drawing.Point(19, 29);
			this._sessionListView.MultiSelect = false;
			this._sessionListView.Name = "_sessionListView";
			this._sessionListView.Size = new System.Drawing.Size(121, 274);
			this._sessionListView.TabIndex = 3;
			this._sessionListView.UseCompatibleStateImageBehavior = false;
			this._sessionListView.View = System.Windows.Forms.View.List;
			this._sessionListView.SelectedIndexChanged += new System.EventHandler(this.OnSessionListView_SelectedIndexChanged);
			// 
			// _componentsListView
			// 
			this._componentsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._componentsListView.HideSelection = false;
			this._componentsListView.Location = new System.Drawing.Point(189, 39);
			this._componentsListView.MultiSelect = false;
			this._componentsListView.Name = "_componentsListView";
			this._componentsListView.Size = new System.Drawing.Size(307, 98);
			this._componentsListView.TabIndex = 4;
			this._componentsListView.UseCompatibleStateImageBehavior = false;
			this._componentsListView.View = System.Windows.Forms.View.List;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Sessions";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(186, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Session Components";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(189, 160);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(311, 187);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(303, 161);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(303, 161);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// SessionsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._componentsListView);
			this.Controls.Add(this._sessionListView);
			this.Controls.Add(this._newSessionButton);
			this.Name = "SessionsControl";
			this.Size = new System.Drawing.Size(503, 350);
			this.Load += new System.EventHandler(this.SessionsControl_Load);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _newSessionButton;
		private System.Windows.Forms.ListView _sessionListView;
		private System.Windows.Forms.ListView _componentsListView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
	}
}
