using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldsValuesGridViewModel
	{
		private readonly FieldGatherer _fieldGatherer;
		private ComponentFile _file;

		public Action ComponentFileChanged;
		public List<KeyValuePair<FieldInstance, FieldDefinition>> RowData { get; private set; }

		private Dictionary<string, IEnumerable<string>> _autoCompleteLists = new Dictionary<string,IEnumerable<string>>();
		private readonly IMultiListDataProvider _autoCompleteProvider;

		public Func<string, bool> FieldFilterFunction { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldsValuesGridViewModel(ComponentFile file, IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			_fieldGatherer = fieldGatherer;

			if (autoCompleteProvider != null)
			{
				_autoCompleteProvider = autoCompleteProvider;
				_autoCompleteProvider.NewDataAvailable += HandleNewAutoCompleteDataAvailable;
				_autoCompleteLists = _autoCompleteProvider.GetValueLists();
			}

			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public void SetComponentFile(ComponentFile file)
		{
			_file = file;
			RowData = new List<KeyValuePair<FieldInstance, FieldDefinition>>();
			LoadFields();

			if (ComponentFileChanged != null)
				ComponentFileChanged();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the field values into the model's data cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadFields()
		{
			var factoryFields = _file.FileType.FactoryFields;
			var fields = factoryFields.Union(_fieldGatherer.GetAllFieldsForFileType(_file.FileType).Where(f => !factoryFields.Any(e => e.Key == f.Key)));
			foreach (var field in fields)
			{
				if (FieldFilterFunction!=null && !FieldFilterFunction(field.Key))
					continue;

				if (field.Key == "notes")
					continue;

				var fieldValue = new FieldInstance(field.Key, _file.GetStringValue(field.Key, string.Empty));

				//TODO: make use of field.ReadOnly

				// Each row in the cache is a key/value pair. The key is the FieldValue object
				// and the value is a boolean indicating whether or not the field is custom.
				RowData.Add(new KeyValuePair<FieldInstance, FieldDefinition>(fieldValue.CreateCopy(), field));
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GridSettingsName
		{
			get { return _file.FileType.FieldsGridSettingsName; }
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewAutoCompleteDataAvailable(object sender, EventArgs e)
		{
			_autoCompleteLists = _autoCompleteProvider.GetValueLists();
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteStringCollection GetAutoCompleteListForIndex(int index)
		{
			var fieldId = GetIdForIndex(index);
			var autoCompleteValues = new AutoCompleteStringCollection();

			if (!string.IsNullOrEmpty(fieldId))
			{
				IEnumerable<string> values;
				if (_autoCompleteLists.TryGetValue(fieldId, out values))
					autoCompleteValues.AddRange(values.ToArray());
			}

			return autoCompleteValues;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsIndexForCustomField(int index)
		{
			if (index < RowData.Count && RowData[index].Value != null) //review caused by the "empty" thing (new field?)
			{
				return RowData[index].Value.IsCustom;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsIndexForReadOnlyField(int index)
		{
			return (index < RowData.Count ? RowData[index].Value.ReadOnly : false);
		}

		/// ------------------------------------------------------------------------------------
		public bool CanDeleteRow(int index, out int indexToDelete)
		{
			indexToDelete = (index < RowData.Count ? index : -1);
			return IsIndexForCustomField(index);
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance AddEmptyField()
		{

			var fieldValue = new FieldInstance(string.Empty, string.Empty);
			//Review do (jh): I think this is just for new fields, so this null might not be right
			RowData.Add(new KeyValuePair<FieldInstance, FieldDefinition>(fieldValue, null));
			return fieldValue;
		}

		/// ------------------------------------------------------------------------------------
		public string GetIdForIndex(int index)
		{
			return (index < RowData.Count ? RowData[index].Key.FieldId : null);
		}

		/// ------------------------------------------------------------------------------------
		public string GetValueForIndex(int index)
		{
			return (index < RowData.Count ? RowData[index].Key.Value : null);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIdForIndex(string id, int index)
		{
			var fieldValue = (index == RowData.Count ? AddEmptyField() : RowData[index].Key);
			fieldValue.FieldId = (id != null ? id.Trim() : string.Empty).Replace(' ', '_');
		}

		/// ------------------------------------------------------------------------------------
		public void SaveValueForIndex(string value, int index)
		{
			var fieldValue = (index == RowData.Count ? AddEmptyField() : RowData[index].Key);
			fieldValue.Value = (value != null ? value.Trim() : string.Empty);
			_file.Save();
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveFieldForIndex(int index)
		{
			var field = RowData[index].Key;

			var origField = _file.MetaDataFieldValues.Find(x => x.FieldId == field.FieldId);
			if (origField != null)
				_file.MetaDataFieldValues.Remove(origField);

			_file.Save();
			RowData.RemoveAt(index);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveFieldForIndex(int index)
		{
			var newField = RowData[index].Key;
			var oldField = _file.MetaDataFieldValues.Find(x => x.FieldId == newField.FieldId);

			// Don't bother doing anything if the old value is the same as the new value.
			if (oldField == newField)
				return;

			// TODO: handle case where new name is different.

			string failureMessage;
			_file.SetValue(newField, out failureMessage);

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			_file.Save();
		}
	}
}
