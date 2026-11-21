using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorTabPage : TabPage
	{
		private readonly Color _borderColor;
		public IEditorProvider EditorProvider { get; private set; }
		public bool IsEditorControlLoaded { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComponentEditorTabPage(IEditorProvider provider, Color borderColor)
		{
			DoubleBuffered = true;
			SetProvider(provider);
			Padding = new Padding(3, 5, 5, 4);
			_borderColor = borderColor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is to fix a .Net painting bug for tab controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			if (VisualStyleRenderer.IsSupported)
			{
				var renderer = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
				renderer.DrawBackground(e.Graphics, ClientRectangle);
			}

			if (Controls.Count > 0)
			{
				var rc = Controls[0].Bounds;
				rc.Inflate(1, 1);
				rc.Width--;
				rc.Height--;

				using (var pen = new Pen(_borderColor))
					e.Graphics.DrawRectangle(pen, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			ImageKey = EditorProvider.ImageKey;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// No separate live-rescale wiring needed here: TabPage derives from Panel, so once this
		/// page's TabControl ancestor is reached (see ComponentEditorsTabControl.ApplyScale),
		/// UITextScaler's generic recursive switch reaches this page -- and, through it, the
		/// hosted editor Control -- on its own via the ordinary Panel case.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetProvider(IEditorProvider provider)
		{
			if (EditorProvider != null)
				EditorProvider.TabTextChanged -= HandleEditorTextChanged;

			EditorProvider = provider;
			Text = provider.TabText;
			provider.TabTextChanged += HandleEditorTextChanged;

			var control = EditorProvider.Control;
			control.Dock = DockStyle.Fill;
			Controls.Add(control);
			ImageKey = EditorProvider.ImageKey;

			control.ScaleUIFont();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// EditorProvider.Control is a long-lived instance owned by FileType's own
				// editor cache, reused across element selections -- not owned by this TabPage.
				// Without removing it first, disposing this TabPage would cascade Dispose()
				// onto it too, and the next selection that needs to show it again would throw
				// ObjectDisposedException when re-adding it to a new TabPage.
				if (EditorProvider?.Control != null)
					Controls.Remove(EditorProvider.Control);
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleEditorTextChanged(string text)
		{
			Text = text;
		}
	}
}
