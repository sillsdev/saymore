using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class BrowserEditor : EditorBase
	{
		/// ------------------------------------------------------------------------------------
		public BrowserEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Browser";
			_browser.Navigate(file.PathToAnnotatedFile);
		}
	}
}
