// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: GridPanel.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.ComponentModel;
using System.Windows.Forms;
using SilUtils;
using SilUtils.Controls;

namespace Sponge2.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GridPanel : SilPanel
	{
		protected SilGrid _grid = new SilGrid();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GridPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridPanel()
		{
			_grid = new SilGrid();
			_grid.DefaultCellStyle = new DataGridViewCellStyle();
			_grid.BorderStyle = BorderStyle.None;
			_grid.Dock = DockStyle.Fill;
			Controls.Add(_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the panel's grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SilGrid Grid
		{
			get { return _grid; }
		}
	}
}
