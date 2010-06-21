using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class VideoComponentEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public VideoComponentEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Video File Information";
			_binder.SetComponentFile(file);
		}
	}
}
