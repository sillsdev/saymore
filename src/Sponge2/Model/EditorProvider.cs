using System.Windows.Forms;
using Sponge2.Model.Files;

namespace Sponge2.Model
{
	public class EditorProvider
	{
		private readonly Control _control;

		public EditorProvider(Control control, string tabName)
		{
			_control = control;
			TabName = tabName;
		}

		public string TabName { get; private set; }

		/// <summary>
		/// Note: the caller doesn't own what this returns: don't dispose of it, ever
		/// </summary>
		public Control GetEditor(ComponentFile file)
		{
			//NB: in the future, we can do more complicated things like reusing controls,
			//constructing controls using the DI container, etc.
			//This will be invisible to the client.
			return _control;
		}
	}
}