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
using Sponge2.Model.Files;

namespace Sponge2.UI
{
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorTabPage : TabPage
	{
		public EditorProvider EditorProvider { get; private set; }
		public bool IsEditorControlLoaded { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComponentEditorTabPage(EditorProvider provider)
		{
			SetProvider(provider);
		}

		/// ------------------------------------------------------------------------------------
		public void SetProvider(EditorProvider provider)
		{
			EditorProvider = provider;
			Text = provider.TabName;
			IsEditorControlLoaded = false;
			Controls.Clear();
		}

		/// ------------------------------------------------------------------------------------
		public bool LoadEditorControl(ComponentFile file)
		{
			if (IsEditorControlLoaded)
				return false;

			var control = EditorProvider.GetEditor(file);
			control.Dock = DockStyle.Fill;
			Controls.Add(control);
			IsEditorControlLoaded = true;
			return true;
		}
	}
}
