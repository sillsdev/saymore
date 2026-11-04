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
	public class ProjectElementComponentFile : ComponentFile, IStringValueCollection
	{
		public delegate ProjectElementComponentFile Factory(ProjectElement parentElement,
			FileType fileType, XmlFileSerializer xmlFileSerializer, string rootElementName);

		private string _oldUiId = null;

		public string Id => ParentElement.Id;

		/// ------------------------------------------------------------------------------------
		public ProjectElementComponentFile(ProjectElement parentElement,
			FileType fileType, XmlFileSerializer xmlFileSerializer, string rootElementName, FieldUpdater fieldUpdater)
			: base(parentElement, parentElement.SettingsFilePath, fileType, rootElementName, xmlFileSerializer, fieldUpdater)
		{
			InitializeFileInfo();
		}

		[Obsolete("For Mocking Only")]
		public ProjectElementComponentFile():base(null) {}

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

			return (key != "id" ? base.GetStringValue(key, defaultValue) : Id);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetStringValue(string key, string defaultValue)
		{
			return GetStringValue(key, defaultValue, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Sets the given string value and returns whether it was set exactly as
		/// requested.</summary>
		/// <returns><c>false</c> if the value is changed or the set operation fails</returns>
		/// ------------------------------------------------------------------------------------
		public virtual bool TrySetStringValue(string key, string newValue)
		{
			return (SetStringValue(key, newValue) == newValue);
		}

		/// ------------------------------------------------------------------------------------
		public override string SetStringValue(string key, string newValue)
		{
			if (key == SessionFileType.kStatusFieldName)
				newValue = Session.GetStatusAsEnumParsableString(newValue);
			else if (key == SessionFileType.kGenreFieldName)
				newValue = GenreDefinition.TranslateNameToId(newValue);
			else if (key == PersonFileType.kCode)
				_oldUiId = ParentElement.UiId;

			return base.SetStringValue(key, newValue);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnAfterSave(object sender)
		{
			base.OnAfterSave(sender);
			if (_oldUiId != null)
			{
				ParentElement.NotifyOtherElementsOfUiIdChange(_oldUiId);
				_oldUiId = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string TryChangeChangeId(string newId, out string failureMessage)
		{
			failureMessage = null;

			if (ParentElement.Id != newId)
			{
				Program.SuspendBackgroundProcesses();
				try
				{
					var oldId = ParentElement.Id;
					if (ParentElement.TryChangeIdAndSave(newId, out failureMessage))
					{
						LoadFileSizeAndDateModified();
						OnIdChanged("id", oldId, newId);
					}
				}
				finally
				{
					Program.ResumeBackgroundProcesses(false);
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
		public virtual IEnumerable<FieldInstance> GetCustomFields()
		{
			return MetaDataFieldValues.Where(
				val => val.FieldId.StartsWith(XmlFileSerializer.kCustomFieldIdPrefix));
		}

		// The metadata fields (besides any custom fields) that are worth searching for a
		// session's own metadata file.
		private static readonly HashSet<string> sessionFileSearchableTags = new HashSet<string>
		{
			SessionFileType.kGenreFieldName,
			SessionFileType.kTitleFieldName,
			SessionFileType.kSettingFieldName,
			SessionFileType.kParticipantsFieldName,
			SessionFileType.kSituationFieldName,
			SessionFileType.kSynopsisFieldName,
			SessionFileType.kLocationFieldName,
			SessionFileType.kAccessFieldName,
			FileType.kNotesFieldName,
			SessionFileType.kCountryFieldName,
			SessionFileType.kContinentFieldName,
			SessionFileType.kRegionFieldName,
			SessionFileType.kAddressFieldName,
			SessionFileType.kSubGenreFieldName,
			SessionFileType.kInteractivityFieldName,
			SessionFileType.kPlanningTypeFieldName,
			SessionFileType.kInvolvementFieldName,
			SessionFileType.kSocialContextFieldName,
			SessionFileType.kTaskFieldName
		};

		/// ------------------------------------------------------------------------------------
		protected override HashSet<string> GetCustomFieldIds()
		{
			return new HashSet<string>(GetCustomFields()
				.Select(f => f.FieldId)
				.Where(id => !string.IsNullOrEmpty(id)));
		}

		/// ------------------------------------------------------------------------------------
		protected override HashSet<string> GetSearchableFieldIds()
		{
			return FileType is SessionFileType
				? new HashSet<string>(sessionFileSearchableTags.Union(GetCustomFieldIds()))
				: base.GetSearchableFieldIds();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A session's own metadata file also matches if the query is found among its
		/// contributors' notes, which aren't represented by a searchable field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override bool IsMatch(string query)
		{
			return base.IsMatch(query) ||
				(FileType is SessionFileType && ContainsContributorNotes(query));
		}
	}
}
