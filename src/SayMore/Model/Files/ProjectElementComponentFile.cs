using System;
using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Fields;

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
		public delegate ProjectElementComponentFile Factory(ProjectElement parentElement,
			FileType fileType, XmlFileSerializer xmlFileSerializer, string rootElementName);

		//protected readonly ProjectElement _parentElement;

		/// ------------------------------------------------------------------------------------
		public ProjectElementComponentFile(ProjectElement parentElement,
			FileType fileType, XmlFileSerializer xmlFileSerializer, string rootElementName, FieldUpdater fieldUpdater)
			: base(parentElement, parentElement.SettingsFilePath, fileType, rootElementName, xmlFileSerializer, fieldUpdater)
		{
			InitializeFileInfo();
		}

		[Obsolete("For Mocking Only")]
		public ProjectElementComponentFile(){}

		/// ------------------------------------------------------------------------------------
		public string GetStringValue(string key, string defaultValue, bool localized)
		{
			if (key == SessionFileType.kStatusFieldName)
			{
				var value = base.GetStringValue(key, ParentElement.DefaultStatusValue);
				return localized ? Session.GetLocalizedStatus(value) : value;
			}
			if (key == SessionFileType.kGenreFieldName)
			{
				var value = base.GetStringValue(key, defaultValue);
				return localized ? GenreDefinition.TranslateIdToName(value) : value;
			}

			return (key != "id" ? base.GetStringValue(key, defaultValue) : ParentElement.Id);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			return GetStringValue(key, defaultValue, true);
		}

		/// ------------------------------------------------------------------------------------
		public override string SetStringValue(string key, string newValue, out string failureMessage)
		{
			if (key == SessionFileType.kStatusFieldName)
				newValue = Session.GetStatusAsEnumParsableString(newValue);
			else if (key == SessionFileType.kGenreFieldName)
				newValue = GenreDefinition.TranslateNameToId(newValue);

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

		/// ------------------------------------------------------------------------------------
		public IEnumerable<FieldInstance> GetCustomFields()
		{
			return MetaDataFieldValues.Where(
				val => val.FieldId.StartsWith(XmlFileSerializer.kCustomFieldIdPrefix));
		}
	}
}
