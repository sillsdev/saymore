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
			key => key.StartsWith(FileSerializer.kCustomFieldIdPrefix))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override string GetDisplayableFieldName(int index)
		{
			var id = GetIdForIndex(index);
			if (id != null)
				id = id.Substring(FileSerializer.kCustomFieldIdPrefix.Length).Replace('_', ' ');
			return id;
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFieldNameToSerialize(string id)
		{
			return FileSerializer.kCustomFieldIdPrefix + base.GetFieldNameToSerialize(id);
		}
	}
}
