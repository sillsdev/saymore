using System;
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
		public void TrySelectEditorOfType(Type componentEditorType)
		{
			if (componentEditorType == null)
				return;

			foreach (TabPage page in TabPages)
			{
				if (page.Controls.Count > 0 && page.Controls[0].GetType() == componentEditorType)
				{
					SelectedTab = page;
					break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void MakeAppropriateEditorsVisible()
		{
			try
			{
				var visibleEditors = EditorProviders.Where(ep => ep.IsOKToShow).ToList();

				if (visibleEditors.Count == TabPages.Count &&
					GetAreAppropriateEditorsAlreadyVisible(visibleEditors.Select(e => e.GetType()).ToList()))
				{
					return;
				}

				Utils.SetWindowRedraw(this, false);
				TabPages.Clear();

				foreach (var editor in EditorProviders.Where(ep => ep.IsOKToShow))
				{
					TabPages.Add(new ComponentEditorTabPage(editor, _componentEditorBorderColor));
					editor.Control.BackColor = _componentEditorBackColor;
					editor.Control.UseWaitCursor = false;
				}

				Utils.SetWindowRedraw(this, true);
			}
			catch (ObjectDisposedException)
			{
				// This can happen when shutting down.
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool GetAreAppropriateEditorsAlreadyVisible(ICollection<Type> desiredEditorTypes)
		{
			return TabPages.Cast<TabPage>().All(tp =>
				(from object ctrl in tp.Controls select desiredEditorTypes.Contains(ctrl)).FirstOrDefault());
		}
	}
}
