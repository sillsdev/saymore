using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class CustomFieldsValuesGridViewModel : FieldsValuesGridViewModel
	{
		/// ------------------------------------------------------------------------------------
		public CustomFieldsValuesGridViewModel(ComponentFile file,
			IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer) :
			base(file, autoCompleteProvider, fieldGatherer,
			key => key.StartsWith(XmlFileSerializer.kCustomFieldIdPrefix))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override string GetDisplayableFieldName(int index)
		{
			var id = GetIdForIndex(index);
			if (id == null) return null;
			id = id.Substring(XmlFileSerializer.kCustomFieldIdPrefix.Length).Replace('_', ' ');
			return id.Trim();
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFieldNameToSerialize(string id)
		{
			return XmlFileSerializer.kCustomFieldIdPrefix + base.GetFieldNameToSerialize(id);
		}
	}
}
