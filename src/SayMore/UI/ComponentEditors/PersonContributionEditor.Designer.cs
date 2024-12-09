using System.Windows.Forms;

namespace SayMore.UI.ComponentEditors
{
	partial class PersonContributionEditor
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
			if (disposing)
			{
				Program.PersonDataChanged -= Program_PersonDataChanged;
				_cancellationTokenSource?.Dispose();
				components?.Dispose();
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
			this._grid = new SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// colName
			// 
			this.colName.DataPropertyName = "FileName";
			this.colName.HeaderText = "_L10N_:PeopleView.ContributionEditor.NameColumnTitle!Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			this.colName.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colRole
			// 
			this.colRole.DataPropertyName = "FileName";
			this.colRole.HeaderText = "_L10N_:PeopleView.ContributionEditor.RoleColumnTitle!Role";
			this.colRole.Name = "colRole";
			this.colRole.ReadOnly = true;
			this.colRole.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colDate
			// 
			this.colDate.DataPropertyName = "FileName";
			this.colDate.HeaderText = "_L10N_:PeopleView.ContributionEditor.DateColumnTitle!Date";
			this.colDate.Name = "colDate";
			this.colDate.ReadOnly = true;
			this.colDate.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// colComments
			// 
			this.colComments.DataPropertyName = "FileName";
			this.colComments.HeaderText = "_L10N_:PeopleView.ContributionEditor.CommentColumnTitle!Comments";
			this.colComments.Name = "colComments";
			this.colComments.ReadOnly = true;
			this.colComments.SortMode = DataGridViewColumnSortMode.Automatic;
			// 
			// _grid
			// 
			this._grid.AllowUserToOrderColumns = false;
			this._grid.AllowUserToResizeRows = true;
			this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
				this.colName,
				this.colRole,
				this.colDate,
				this.colComments});
			this.locExtender.SetLocalizableToolTip(this._grid, null);
			this.locExtender.SetLocalizationComment(this._grid, null);
			this.locExtender.SetLocalizingId(this._grid, "ContributionsEditorGrid");
			this._grid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this._grid_ColumnWidthChanged);
			this._grid.Dock = System.Windows.Forms.DockStyle.Top;
			this._grid.Name = "PersonContributionGrid";
			this._grid.RowHeadersVisible = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// PersonContributionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(207)))), ((int)(((byte)(219)))));
			this.Controls.Add(this._grid);
			this.Name = "PersonRoles";
			this.Size = new System.Drawing.Size(410, 319);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid _grid;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn colComments;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
