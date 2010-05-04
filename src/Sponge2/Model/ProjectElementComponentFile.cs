using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sponge2.Model.Files;

namespace Sponge2.Model
{
	/// <summary>
	/// Person and Session are different from other files. This subclass allows us to
	/// account for those differences
	/// </summary>
	public class ProjectElementComponentFile :ComponentFile
	{
		private readonly ProjectElement _parentElement;

		public ProjectElementComponentFile(ProjectElement parentElement, FileType fileType, FileSerializer fileSerializer, string rootElementName)
			: base(parentElement.SettingsFilePath, fileType, fileSerializer, rootElementName)
		{
			_parentElement = parentElement;
		}

		//--------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			if (key != "id")
			{
				return base.GetStringValue(key, defaultValue);
			}
			return _parentElement.Id;
		}


		public override string SetValue(string key, string value, out string failureMessage)
		{
			if (key != "id")
			{
				return base.SetValue(key, value, out failureMessage);
			}
			if(_parentElement.TryChangeIdAndSave(value, out failureMessage))
			{
				InvokeUiShouldRefresh();
			}


			//send back whatever is now, changed or not, if the renaming failed
			return _parentElement.Id;
		}

		public override string FileName
		{
			get { return Path.GetFileName(_parentElement.SettingsFilePath); }
		}
	}
}
