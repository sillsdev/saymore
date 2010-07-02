using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorsTabControl : TabControl
	{
		public string ProviderKey { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ComponentEditorsTabControl(string providerKey, ImageList imgList,
			IEnumerable<IEditorProvider> editorProviders, Color componentEditorBackColor,
			Color componentEditorBorderColor)
		{
			ImageList = imgList;
			Font = SystemFonts.IconTitleFont;
			Dock = DockStyle.Fill;
			Visible = false;
			ProviderKey = providerKey;

			foreach (var editor in editorProviders)
			{
				TabPages.Add(new ComponentEditorTabPage(editor, componentEditorBorderColor));
				editor.Control.BackColor = componentEditorBackColor;
			}
		}
	}
}
