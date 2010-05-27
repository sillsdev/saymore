using System.Windows.Forms;
using SayMore.Model.Files;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class EditorProvider
	{
		public Control Control { get; private set; }
		public string TabText { get; private set; }
		public string ImageKey { get; private set; }

		/// ------------------------------------------------------------------------------------
		public EditorProvider(Control control, string tabText)
		{
			Control = control;
			TabText = tabText;
			ImageKey = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		public EditorProvider(Control control, string tabText, string imageKey) : this(control, tabText)
		{
			ImageKey = imageKey;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Note: the caller doesn't own what this returns: don't dispose of it, ever
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control GetEditor(ComponentFile file)
		{
			//NB: in the future, we can do more complicated things like reusing controls,
			//constructing controls using the DI container, etc.
			//This will be invisible to the client.
			return Control;
		}
	}
}