using L10NSharp;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	class AdditionalFieldsValuesGridViewModel : FieldsValuesGridViewModel
	{
		/// ------------------------------------------------------------------------------------
		public AdditionalFieldsValuesGridViewModel(ComponentFile file,
			IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer) :
			base(file, autoCompleteProvider, fieldGatherer,
			key => key.StartsWith(XmlFileSerializer.kAdditionalFieldIdPrefix))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override string GetDisplayableFieldName(int index)
		{
			var id = GetIdForIndex(index);
			if (id != null)
			{
				var name = id.Substring(XmlFileSerializer.kAdditionalFieldIdPrefix.Length).Replace('_', ' ');
				if (name == "Involvement")
					return LocalizationManager.GetString("SessionsView.MetadataEditor.AdditionalFields.Involvement", "Researcher Involvement");
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
