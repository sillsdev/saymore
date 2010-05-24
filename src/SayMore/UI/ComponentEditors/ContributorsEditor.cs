using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Contributors";
			_binder.SetComponentFile(file);
		}
	}
}
