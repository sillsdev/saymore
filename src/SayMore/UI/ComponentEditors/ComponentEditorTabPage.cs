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
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.Model.Files;
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
		protected override void OnResize(System.EventArgs eventargs)
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
				rc.Inflate(1,1);
				rc.Width--;
				rc.Height--;

				using (var pen = new Pen(AppColors.DataEntryPanelBorder))
					e.Graphics.DrawRectangle(pen, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentChanged(System.EventArgs e)
		{
			base.OnParentChanged(e);
			ImageKey = EditorProvider.ImageKey;
		}

		/// ------------------------------------------------------------------------------------
		public void SetProvider(IEditorProvider provider)
		{
			EditorProvider = provider;
			Text = provider.TabText;
			IsEditorControlLoaded = false;
			Controls.Clear();
		}

		/// ------------------------------------------------------------------------------------
		public bool LoadEditorControl(ComponentFile file)
		{
			if (IsEditorControlLoaded)
				return false;

			var control = EditorProvider.Control;
			control.Dock = DockStyle.Fill;
			Controls.Add(control);
			ImageKey = EditorProvider.ImageKey;
			IsEditorControlLoaded = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disposes of the EditorProvider contained on the tab page if it's for an audio
		/// or video player. The Windows media player tends to hold onto files and folders
		/// otherwise. (cf. SP-60).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DestroyAudioVideoProvider()
		{
			if (Controls.Count > 0 && Controls[0] is AudioVideoPlayer)
				Controls[0].Dispose();
		}
	}
}
