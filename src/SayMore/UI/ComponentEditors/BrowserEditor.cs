using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BrowserEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public BrowserEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Browser";
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (_browser != null)
				_browser.Navigate(file.PathToAnnotatedFile);
		}
	}
}
