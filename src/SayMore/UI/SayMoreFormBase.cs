using System;
using System.Windows.Forms;
using SayMore.Utilities;

namespace SayMore.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Base class for SayMore's top-level dialogs. Applies the user's UI text scale setting
	/// when the form loads and whenever the setting changes while the form is open.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SayMoreFormBase : Form
	{
		protected SayMoreFormBase()
		{
			Load += HandleLoad;
			Shown += HandleShown;
			UITextScaler.ScaleChanged += HandleScaleChanged;
			Disposed += HandleDisposed;
		}

		private void HandleLoad(object sender, EventArgs e)
		{
			this.ScaleUIFont();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Safety net: Load fires mid-way through a subclass's own OnLoad override chain (at
		/// whatever point it calls base.OnLoad), so anything a subclass sets after that call
		/// wins over the Load-triggered scaling pass above. Shown is guaranteed to fire only
		/// after the entire OnLoad chain, base-to-most-derived, has returned, so it catches
		/// whatever Load couldn't -- at the cost of running after the form is already visible,
		/// which is why ScaleUIFont suppresses repaint while it corrects anything.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleShown(object sender, EventArgs e)
		{
			this.ScaleUIFont();
		}

		private void HandleScaleChanged(object sender, EventArgs e)
		{
			this.ScaleUIFont();
		}

		private void HandleDisposed(object sender, EventArgs e)
		{
			UITextScaler.ScaleChanged -= HandleScaleChanged;
		}
	}
}
