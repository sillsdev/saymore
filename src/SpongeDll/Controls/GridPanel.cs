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
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SilUtils;
using SilUtils.Controls;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GridPanel : SilPanel
	{
		protected SilGrid m_grid = new SilGrid();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GridPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridPanel()
		{
			m_grid = new SilGrid();
			m_grid.DefaultCellStyle = new DataGridViewCellStyle();
			m_grid.BorderStyle = BorderStyle.None;
			m_grid.Dock = DockStyle.Fill;
			Controls.Add(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the panel's grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridView Grid
		{
			get { return m_grid; }
		}
	}
}
