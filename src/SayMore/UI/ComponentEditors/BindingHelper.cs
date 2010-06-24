using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("IsBound", typeof(IComponent))]
	[ProvideProperty("IsComponentFileId", typeof(IComponent))]
	public class BindingHelper : Component, IExtenderProvider
	{
		private readonly Func<string, string> MakeKeyFromText = (x => x.TrimStart('_'));
		private readonly Func<Control, string> MakeKeyFromControl = (x => x.Name.TrimStart('_'));

		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		public event GetBoundControlValueHandler GetBoundControlValue;

		private Container components;

		private readonly Dictionary<Control, bool> _extendedControls =
			new Dictionary<Control, bool>();

		private readonly Dictionary<FieldsValuesGrid, IEnumerable<FieldValue>> _boundGrids =
			new Dictionary<FieldsValuesGrid, IEnumerable<FieldValue>>();

		private List<Control> _boundControls;
		private ComponentFile _file;
		private Control _componentFileIdControl;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		public BindingHelper()
		{
			// Required for Windows.Forms Class Composition Designer support
			components = new Container();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for instance that supports Class Composition designer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BindingHelper(IContainer container) : this()
		{
			container.Add(this);
		}

		#endregion

		#region IExtenderProvider Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Extend only certain controls. Add new ones as they are needed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanExtend(object extendee)
		{
			var ctrl = extendee as Control;
			if (ctrl == null)
				return false;

			var extend = (ctrl is TextBox || ctrl is DateTimePicker || ctrl is ComboBox);

			if (extend && !_extendedControls.ContainsKey(ctrl))
				_extendedControls[ctrl] = true;

			return extend;
		}

		#endregion

		#region Properties provided by this extender
		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("BindingHelper Properties")]
		public bool GetIsBound(object obj)
		{
			if (obj is FieldsValuesGrid)
				return _boundGrids.ContainsKey((FieldsValuesGrid)obj);

			bool isBound;
			return (_extendedControls.TryGetValue(obj as Control, out isBound) ? isBound : false);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsBound(object obj, bool bind)
		{
			if (obj is FieldsValuesGrid)
			{
				if (bind)
					BindFieldsValuesGrid((FieldsValuesGrid)obj, null);
				else if (_boundGrids.ContainsKey((FieldsValuesGrid)obj))
					UnbindFieldsValuesGrid((FieldsValuesGrid)obj);
			}
			else
			{
				var ctrl = obj as Control;
				_extendedControls[ctrl] = bind;

				// Do this just in case this is being called from outside the initialize
				// components method and after the component file has been set.
				if (!bind)
					UnBindControl(ctrl);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("BindingHelper Properties")]
		public bool GetIsComponentFileId(object obj)
		{
			return (_componentFileIdControl == obj);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsComponentFileId(object obj, bool isFileId)
		{
			if (obj is Control && isFileId)
				_componentFileIdControl = (Control)obj;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetComponentFile(ComponentFile file)
		{
			if (DesignMode)
				return;

			_file = file;

			// First, collect only the extended controls that are bound.
			_boundControls = _extendedControls.Where(x => x.Value).Select(x => x.Key).ToList();

			foreach (var ctrl in _boundControls)
			{
				ctrl.Font = SystemFonts.IconTitleFont;
				BindControl(ctrl);
			}

			foreach (var grid in _boundGrids.Keys)
				UpdateFieldsValuesGridFromFile(grid);
		}

		/// ------------------------------------------------------------------------------------
		private void BindControl(Control ctrl)
		{
			if (!_boundControls.Contains(ctrl))
				_boundControls.Add(ctrl);

			UpdateControlValueFromField(ctrl);
			ctrl.Validating += HandleValidatingControl;
			ctrl.Disposed += HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void UnBindControl(Control ctrl)
		{
			if (ctrl is FieldsValuesGrid)
			{
				UnbindFieldsValuesGrid((FieldsValuesGrid)ctrl);
				return;
			}

			ctrl.Disposed -= HandleDisposed;
			ctrl.Validating -= HandleValidatingControl;

			if (_boundControls != null && _boundControls.Contains(ctrl))
				_boundControls.Remove(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Binds a field/value grid. If factoryFieldsToShowInGrid is null, it is assumed that
		/// the grid is only for custom fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void BindFieldsValuesGrid(FieldsValuesGrid grid,
			IEnumerable<FieldValue> factoryFieldsToShowInGrid)
		{
			_boundGrids[grid] = (factoryFieldsToShowInGrid ?? new List<FieldValue>(0));

			grid.RowValidated += HandleFieldsValuesGridRowValidated;
			grid.Disposed += HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		public void UnbindFieldsValuesGrid(FieldsValuesGrid grid)
		{
			if (_boundGrids.ContainsKey(grid))
				_boundGrids.Remove(grid);

			grid.RowValidated -= HandleFieldsValuesGridRowValidated;
			grid.Disposed -= HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDisposed(object sender, EventArgs e)
		{
			UnBindControl(sender as Control);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when something happens (like chosing a preset) which modifies the values
		/// of the file directly, and we need to update the UI
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFieldsFromFile()
		{
			foreach (var ctrl in _boundControls)
				UpdateControlValueFromField(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateFieldsValuesGridFromFile(FieldsValuesGrid grid)
		{
			var fieldsToShowInGrid = GetFactoryFieldsToShowInGrid(grid);
			fieldsToShowInGrid.AddRange(GetCustomFieldsToShowInGrid());

			grid.RowValidated -= HandleFieldsValuesGridRowValidated;
			grid.SetFieldsAndValues(fieldsToShowInGrid);
			grid.RowValidated += HandleFieldsValuesGridRowValidated;
		}

		/// ------------------------------------------------------------------------------------
		private List<FieldValue> GetFactoryFieldsToShowInGrid(FieldsValuesGrid grid)
		{
			IEnumerable<FieldValue> defaultFieldList;
			if (!_boundGrids.TryGetValue(grid, out defaultFieldList))
				return 	new List<FieldValue>();

			var returnFields = new List<FieldValue>();

			foreach (var defaultfld in defaultFieldList)
			{
				var field = _file.MetaDataFieldValues.FirstOrDefault(x => x.FieldKey == defaultfld.FieldKey);
				if (field == null)
					returnFields.Add(defaultfld);
				else
				{
					field.DisplayName = defaultfld.DisplayName;
					returnFields.Add(field);
				}
			}

			return returnFields;
		}

		/// ------------------------------------------------------------------------------------
		private List<FieldValue> GetCustomFieldsToShowInGrid()
		{
			return (from x in _file.MetaDataFieldValues
					where x.IsCustomField
					select x).ToList();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateControlValueFromField(Control ctrl)
		{
			var key = MakeKeyFromControl(ctrl);
			ctrl.Text = _file.GetStringValue(key, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public string GetValue(string key)
		{
			return _file.GetStringValue(key, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public string SetValue(string key, string value)
		{
			string failureMessage;
			var modifiedValue = _file.SetValue(key, value, out failureMessage);

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			_file.Save();

			return modifiedValue;
		}

		///// ------------------------------------------------------------------------------------
		//public void ResetBoundControlsToDefaultValues()
		//{
		//    foreach (var ctrl in _boundControls)
		//    {
		//        if (ctrl is TextBox)
		//            ctrl.Text = string.Empty;
		//        else if (ctrl is DateTimePicker)
		//        {
		//            ((DateTimePicker)ctrl).Value = _file != null && File.Exists(_file.PathToAnnotatedFile) ?
		//                File.GetLastWriteTime(_file.PathToAnnotatedFile) : DateTime.Now;
		//        }
		//        else if (ctrl is ComboBox)
		//            ((ComboBox)ctrl).SelectedIndex = (((ComboBox)ctrl).Items.Count > 0 ? 0 : -1);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		private void HandleValidatingControl(object sender, CancelEventArgs e)
		{
			var ctrl = (Control)sender;
			var key = MakeKeyFromControl(ctrl);

			string newValue = null;
			var gotNewValueFromDelegate = (GetBoundControlValue != null &&
				!GetBoundControlValue(this, ctrl, out newValue));

			// Don't bother doing anything if the old value is the same as the new value.
			var oldValue = _file.GetStringValue(key, null);
			if (oldValue != null && oldValue == ctrl.Text.Trim())
				return;

			string failureMessage;

			newValue = (_componentFileIdControl == ctrl ?
				_file.TryChangeChangeId(ctrl.Text.Trim(), out failureMessage) :
				_file.SetValue(key, (newValue ?? ctrl.Text.Trim()), out failureMessage));

			if (!gotNewValueFromDelegate)
				ctrl.Text = newValue;

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			if (_componentFileIdControl != ctrl)
				_file.Save();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFieldsValuesGridRowValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (((FieldsValuesGrid)sender).NewRowIndex == e.RowIndex)
				return;

			FieldValue oldFieldValue;
			FieldValue newFieldValue;
			((FieldsValuesGrid)sender).GetFieldValueForIndex(e.RowIndex,
				out oldFieldValue, out newFieldValue);

			// Don't bother doing anything if the old value is the same as the new value.
			if (oldFieldValue == newFieldValue)
				return;

			// TODO: handle case where new display name is different.

			string failureMessage;
			_file.SetValue(newFieldValue, out failureMessage);

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			_file.Save();
		}
	}
}