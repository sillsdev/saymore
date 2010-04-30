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
// File: ComponentEditorTabPage.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Windows.Forms;
using Sponge2.Model;

namespace Sponge2.UI
{
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorTabPage : TabPage
	{
		public EditorProvider EditorProvider { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ComponentEditorTabPage(EditorProvider provider)
		{
			EditorProvider = provider;
			Text = provider.TabName;
		}
	}
}
