using System;
using L10NSharp;
using SIL.Archiving.IMDI.Lists;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;

namespace SayMore.UI.ComponentEditors
{
	class AdditionalFieldsValuesGridViewModel : FieldsValuesGridViewModel
	{
		/// ------------------------------------------------------------------------------------
		public AdditionalFieldsValuesGridViewModel(ComponentFile file,
			IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer) :
			base(file, autoCompleteProvider, fieldGatherer, IncludeField)
		{
		}

		/// ------------------------------------------------------------------------------------
		private static bool IncludeField(string key)
		{
			if (!key.StartsWith(XmlFileSerializer.kAdditionalFieldIdPrefix))
				return false;

			foreach (string field in Settings.Default.AdditionalFieldsToHide.Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries))
			{
				if (key == XmlFileSerializer.kAdditionalFieldIdPrefix + field)
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public string GetListType(int index)
		{
			var id = GetIdForIndex(index);
			if (id != null)
			{
				switch (id)
				{
					case SessionFileType.kInteractivityFieldName:
						return ListType.ContentInteractivity;
					case SessionFileType.kInvolvementFieldName:
						return ListType.ContentInvolvement;
					case SessionFileType.kCountryFieldName:
						return ListType.Countries;
					case SessionFileType.kContinentFieldName:
						return ListType.Continents;
					case SessionFileType.kPlanningTypeFieldName:
						return ListType.ContentPlanningType;
					case SessionFileType.kSocialContextFieldName:
						return ListType.ContentSocialContext;
				}
			}
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public override string GetDisplayableFieldName(int index)
		{
			var id = GetIdForIndex(index);
			if (id != null)
			{
				if (id == SessionFileType.kInvolvementFieldName)
					return LocalizationManager.GetString("SessionsView.MetadataEditor.AdditionalFields.Involvement", "Researcher Involvement");
				var name = id.Substring(XmlFileSerializer.kAdditionalFieldIdPrefix.Length).Replace('_', ' ');
				return LocalizationManager.GetDynamicString("SayMore", "SessionsView.MetadataEditor.AdditionalFields." + name, name);
			}
			return id;
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFieldNameToSerialize(string id)
		{
			return XmlFileSerializer.kAdditionalFieldIdPrefix + base.GetFieldNameToSerialize(id);
		}
	}
}
