using System.IO;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Person and Session are different from other files. This subclass allows us to
	/// account for those differences
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectElementComponentFile : ComponentFile
	{
		private readonly ProjectElement _parentElement;

		/// ------------------------------------------------------------------------------------
		public ProjectElementComponentFile(ProjectElement parentElement,
			FileType fileType, FileSerializer fileSerializer, string rootElementName)
			: base(parentElement.SettingsFilePath, fileType, fileSerializer, rootElementName)
		{
			_parentElement = parentElement;
			PathToAnnotatedFile = parentElement.SettingsFilePath;//same thing, there isn't a pair of files for session/person
		}

		/// ------------------------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			return (key != "id" ? base.GetStringValue(key, defaultValue) : _parentElement.Id);
		}

		/// ------------------------------------------------------------------------------------
		public override string SetValue(string key, string value, out string failureMessage)
		{
			if (key != "id")
			{
				return base.SetValue(key, value, out failureMessage);
			}

			if (_parentElement.TryChangeIdAndSave(value, out failureMessage))
			{
				InvokeUiShouldRefresh();
			}

			//send back whatever is now, changed or not, if the renaming failed
			return _parentElement.Id;
		}

		/// ------------------------------------------------------------------------------------
		public override string FileName
		{
			get { return Path.GetFileName(_parentElement.SettingsFilePath); }
		}

		/// ------------------------------------------------------------------------------------
		public override void HandleDoubleClick()
		{
			//don't do anything
		}
	}
}
