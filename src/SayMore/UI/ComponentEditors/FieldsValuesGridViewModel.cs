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
	public class FieldsValuesGridViewModel
	{
		private readonly FieldGatherer _fieldGatherer;
		private ComponentFile _file;

		public Action ComponentFileChanged;
		public List<FieldInstance> RowData { get; private set; }

		private Dictionary<string, IEnumerable<string>> _autoCompleteLists = new Dictionary<string,IEnumerable<string>>();
		private readonly IMultiListDataProvider _autoCompleteProvider;
		private readonly Func<string, bool> _includedFieldFilterFunction;
		private IEnumerable<FieldDefinition> _fieldDefsForFile;

		/// ------------------------------------------------------------------------------------
		public FieldsValuesGridViewModel(ComponentFile file, IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer, Func<string, bool> includedFieldFilterFunction)
		{
			_fieldGatherer = fieldGatherer;
			_includedFieldFilterFunction = includedFieldFilterFunction;

			if (autoCompleteProvider != null)
			{
				_autoCompleteProvider = autoCompleteProvider;
				_autoCompleteProvider.NewDataAvailable += HandleNewAutoCompleteDataAvailable;
				_autoCompleteLists = _autoCompleteProvider.GetValueLists(true);
			}

			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public void SetComponentFile(ComponentFile file)
		{
			_file = file;
			_file.Load();
			RowData = new List<FieldInstance>();
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
			var factoryFields = _file.FileType.FactoryFields.ToArray();

			_fieldDefsForFile = factoryFields
				.Union(_fieldGatherer.GetAllFieldsForFileType(_file.FileType)
				.Where(f => !factoryFields.Any(e => e.Key == f.Key)));

			foreach (var field in _fieldDefsForFile)
			{
				if (_includedFieldFilterFunction != null && !_includedFieldFilterFunction(field.Key))
					continue;

				//TODO: make use of field.ReadOnly

				var fieldValue = new FieldInstance(field.Key, _file.GetStringValue(field.Key, string.Empty));
				RowData.Add(fieldValue);
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
			_autoCompleteLists = _autoCompleteProvider.GetValueLists(true);
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
		private FieldDefinition GetFieldDefinitionForIndex(int index)
		{
			return (index >= RowData.Count ? null :
				_fieldDefsForFile.FirstOrDefault(def => def.FieldName == RowData[index].FieldId));
		}

		/// ------------------------------------------------------------------------------------
		public bool IsIndexForCustomField(int index)
		{
			var fieldDef = GetFieldDefinitionForIndex(index);
			return (fieldDef == null || fieldDef.IsCustom);
		}

		/// ------------------------------------------------------------------------------------
		public bool IsIndexForReadOnlyField(int index)
		{
			var fieldDef = GetFieldDefinitionForIndex(index);
			return (fieldDef != null && fieldDef.ReadOnly);
		}

		/// ------------------------------------------------------------------------------------
		public bool CanDeleteRow(int index, out int indexToDelete)
		{
			indexToDelete = (index < RowData.Count ? index : -1);
			return IsIndexForCustomField(index);
		}

		/// ------------------------------------------------------------------------------------
		public string GetIdForIndex(int index)
		{
			return (index < RowData.Count ? RowData[index].FieldId : null);
		}

		/// ------------------------------------------------------------------------------------
		public string GetDisplayableFieldName(int index)
		{
			var id = GetIdForIndex(index);

			switch (id)
			{
				case "Device": return Program.GetString("FieldsAndValuesGrid.Device", "Device");
				case "Microphone": return Program.GetString("FieldsAndValuesGrid.Microphone", "Microphone");
				case "Duration": return Program.GetString("FieldsAndValuesGrid.Duration", "Duration");
				case "Audio_Bit_Rate": return Program.GetString("FieldsAndValuesGrid.AudioBitRate", "Audio Bit Rate");
				case "Video_Bit_Rate": return Program.GetString("FieldsAndValuesGrid.VideoBitRate", "Video Bit Rate");
				case "Sample_Rate": return Program.GetString("FieldsAndValuesGrid.SampleRate", "Sample Rate");
				case "Bit_Depth": return Program.GetString("FieldsAndValuesGrid.BitDepth", "Bit Depth");
				case "Channels": return Program.GetString("FieldsAndValuesGrid.Channels", "Channels");
				case "Resolution": return Program.GetString("FieldsAndValuesGrid.Resolution", "Resolution");
				case "Frame_Rate": return Program.GetString("FieldsAndValuesGrid.FrameRate", "Frame Rate");
				default: return id;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GetValueForIndex(int index)
		{
			return (index < RowData.Count ? RowData[index].ValueAsString : null);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetShouldAskToRemoveFieldEverywhere(int index, string newId, out string oldId)
		{
			oldId = (index < 0 || index >= RowData.Count ? null : RowData[index].FieldId);
			return ((newId == null || newId.Trim() == string.Empty) && oldId != null);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveIdForIndex(int index, string newId)
		{
			if (newId == null || newId.Trim() == string.Empty)
				return;

			newId = newId.Trim().Replace(' ', '_');

			if (index == RowData.Count)
				RowData.Add(new FieldInstance(newId));
			else if (RowData[index].FieldId != newId)
			{
				_fieldGatherer.SuspendProcessing();
				_file.RenameId(RowData[index].FieldId, newId);
				RowData[index].FieldId = newId;
				_file.Save();
				_fieldGatherer.GatherFieldsForFileNow(_file.PathToAnnotatedFile);
				_fieldGatherer.ResumeProcessing(true);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SaveValueForIndex(int index, string value)
		{
			value = (value != null ? value.Trim() : string.Empty);

			if (value == RowData[index].ValueAsString)
				return;

			string failureMessage;
			value = _file.SetStringValue(RowData[index].FieldId, value, out failureMessage);
			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
			else
				RowData[index].Value = value;

			_fieldGatherer.SuspendProcessing();
			_file.Save();
			_fieldGatherer.GatherFieldsForFileNow(_file.PathToAnnotatedFile);
			_fieldGatherer.ResumeProcessing(false);
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveFieldFromEntireProject(int index)
		{
			_fieldGatherer.SuspendProcessing();
			_file.RemoveField(RowData[index].FieldId);
			RowData.RemoveAt(index);
			_file.Save();
			_fieldGatherer.GatherFieldsForFileNow(_file.PathToAnnotatedFile);
			_fieldGatherer.ResumeProcessing(true);
		}
	}
}
