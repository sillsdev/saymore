using System;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Person and Event are different from other files. This subclass allows us to
	/// account for those differences
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectElementComponentFile : ComponentFile
	{
		public delegate ProjectElementComponentFile Factory(ProjectElement parentElement,
			FileType fileType, FileSerializer fileSerializer, string rootElementName);

		//protected readonly ProjectElement _parentElement;

		/// ------------------------------------------------------------------------------------
		public ProjectElementComponentFile(ProjectElement parentElement,
			FileType fileType, FileSerializer fileSerializer, string rootElementName, FieldUpdater fieldUpdater)
			: base(parentElement, parentElement.SettingsFilePath, fileType, rootElementName, fileSerializer, fieldUpdater)
		{
			PathToAnnotatedFile = parentElement.SettingsFilePath;//same thing, there isn't a pair of files for event/person
			InitializeFileInfo();
		}

		[Obsolete("For Mocking Only")]
		public ProjectElementComponentFile(){}

		/// ------------------------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			if (key == "status")
			{
				var value = base.GetStringValue(key, ParentElement.DefaultStatusValue);
				return value.Replace('_', ' ');
			}

			return (key != "id" ? base.GetStringValue(key, defaultValue) : ParentElement.Id);
		}

		/// ------------------------------------------------------------------------------------
		public override string SetStringValue(string key, string newValue, out string failureMessage)
		{
			if (key == "status")
				newValue = newValue.Replace(' ', '_');

			return base.SetStringValue(key, newValue, out failureMessage);
		}

		/// ------------------------------------------------------------------------------------
		public override string TryChangeChangeId(string newId, out string failureMessage)
		{
			failureMessage = null;

			if (ParentElement.Id != newId)
			{
				var oldId = ParentElement.Id;
				if (ParentElement.TryChangeIdAndSave(newId, out failureMessage))
				{
					LoadFileSizeAndDateModified();
					OnIdChanged("id", oldId, newId);
				}
			}

			// Send back whatever the id is now, whether or not renaming succeeded.
			return ParentElement.Id;
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeRenamedForRole
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeCustomRenamed
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetCanHaveAnnotationFile()
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
			base.Save(path);
			PathToAnnotatedFile = ParentElement.SettingsFilePath;
		}

		/// ------------------------------------------------------------------------------------
		public override void HandleDoubleClick()
		{
			//don't do anything
		}
	}
}
