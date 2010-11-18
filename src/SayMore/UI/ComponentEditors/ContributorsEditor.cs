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

			//I don't know if _binder is going to make sense for this or not:  _binder.SetComponentFile(file);
		}
	}
}
