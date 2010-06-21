using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";
			_binder.SetComponentFile(file);
		}
	}
}
