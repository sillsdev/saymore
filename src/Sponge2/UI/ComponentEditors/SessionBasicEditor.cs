using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	public partial class SessionBasicEditor : EditorBase
	{
		public SessionBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder.SetComponentFile(file);
		}
	}
}
