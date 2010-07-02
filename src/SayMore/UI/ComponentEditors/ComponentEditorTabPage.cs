using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.UI.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class ComponentEditorTabPage : TabPage
	{
		public IEditorProvider EditorProvider { get; private set; }
		public bool IsEditorControlLoaded { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComponentEditorTabPage(IEditorProvider provider)
		{
			DoubleBuffered = true;
			SetProvider(provider);
			Padding = new Padding(3, 5, 5, 4);
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

				using (var pen = new Pen(AppColors.DataEntryPanelBorder))
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
		public void SetProvider(IEditorProvider provider)
		{
			EditorProvider = provider;
			Text = provider.TabText;

			var control = EditorProvider.Control;
			control.Dock = DockStyle.Fill;
			Controls.Add(control);
			ImageKey = EditorProvider.ImageKey;
		}
	}
}
