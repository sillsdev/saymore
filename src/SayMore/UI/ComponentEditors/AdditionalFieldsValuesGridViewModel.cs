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
				id = id.Substring(XmlFileSerializer.kAdditionalFieldIdPrefix.Length).Replace('_', ' ');
			return id;
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFieldNameToSerialize(string id)
		{
			return XmlFileSerializer.kAdditionalFieldIdPrefix + base.GetFieldNameToSerialize(id);
		}
	}
}
