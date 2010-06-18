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
			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			return (key != "id" ? base.GetStringValue(key, defaultValue) : _parentElement.Id);
		}

		/// ------------------------------------------------------------------------------------
		public override string ChangeId(string newId, out string failureMessage)
		{
			failureMessage = null;

			if (_parentElement.Id != newId)
			{
				var oldId = _parentElement.Id;
				if (_parentElement.TryChangeIdAndSave(newId, out failureMessage))
				{
					LoadFileSizeAndDateModified();
					InvokeIdChanged(oldId, newId);
				}
			}

			// Send back whatever the id is now, whether or not renaming failed.
			return _parentElement.Id;
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
			base.Save(path);
			PathToAnnotatedFile = _parentElement.SettingsFilePath;
		}

		/// ------------------------------------------------------------------------------------
		public override void HandleDoubleClick()
		{
			//don't do anything
		}
	}
}
