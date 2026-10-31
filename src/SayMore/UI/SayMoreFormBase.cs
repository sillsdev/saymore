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
			UITextScaler.ScaleChanged += HandleScaleChanged;
			Disposed += HandleDisposed;
		}

		private void HandleLoad(object sender, EventArgs e)
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
