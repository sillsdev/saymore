using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SilTools;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorsTabControl : TabControl
	{
		public string ProviderKey { get; private set; }
		public IEnumerable<IEditorProvider> EditorProviders { get; private set; }

		private readonly Color _componentEditorBackColor;
		private readonly Color _componentEditorBorderColor;

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
			EditorProviders = editorProviders;

			_componentEditorBackColor = componentEditorBackColor;
			_componentEditorBorderColor = componentEditorBorderColor;

			MakeAppropriateEditorsVisible();
		}

		/// ------------------------------------------------------------------------------------
		public IEditorProvider CurrentEditor
		{
			get
			{
				var page = SelectedTab as ComponentEditorTabPage;
				return (page != null ? page.EditorProvider : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void MakeAppropriateEditorsVisible()
		{
			var visibleEditors = EditorProviders.Where(ep => ep.IsOKSToShow).ToList();

			if (visibleEditors.Count == TabPages.Count)
				return;

			Utils.SetWindowRedraw(this, false);

			TabPages.Clear();

			foreach (var editor in EditorProviders.Where(ep => ep.IsOKSToShow))
			{
				TabPages.Add(new ComponentEditorTabPage(editor, _componentEditorBorderColor));
				editor.Control.BackColor = _componentEditorBackColor;
			}

			Utils.SetWindowRedraw(this, true);
		}
	}
}
