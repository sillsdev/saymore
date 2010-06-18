// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: EditorBase.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.UI.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class EditorBase : UserControl
	{
		/// ------------------------------------------------------------------------------------
		public EditorBase()
		{
			DoubleBuffered = true;
			BackColor = AppColors.DataEntryPanelBegin;
			Padding = new Padding(7);
			AutoScroll = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			SetLabelFonts(this, new Font(SystemFonts.IconTitleFont, FontStyle.Bold));
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		private static void SetLabelFonts(Control parent, Font fnt)
		{
			foreach (Control ctrl in parent.Controls)
			{
				if (ctrl.Name.StartsWith("_label"))
					ctrl.Font = fnt;
				else
					SetLabelFonts(ctrl, fnt);
			}
		}
	}
}
