using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using L10NSharp;
using SIL.Extensions;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Component for binding extensible key/value fields on a control to a ComponentFile.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("IsBound", typeof(Control))]
	[ProvideProperty("IsComponentFileId", typeof(Control))]
	public class BindingHelper : Component, IExtenderProvider
	{
		private readonly Func<Control, string> _makeIdFromControlName = (ctrl => ctrl.Name.TrimStart('_'));
		private readonly Func<string, string> _makeControlNameFromId = (id => "_" + id);

		public event EventHandler<TranslateBoundValueBeingSavedArgs> TranslateBoundValueBeingSaved;
		public event EventHandler<TranslateBoundValueBeingRetrievedArgs> TranslateBoundValueBeingRetrieved;

		public delegate void ValidationFailedHandler(BindingHelper sender, Control controlThatDidNotValidate);
		public event ValidationFailedHandler ValidationFailed;

		public delegate void DataSavedHandler();
		public event DataSavedHandler OnDataSaved;

		public ComponentFile ComponentFile { get; private set; }

// ReSharper disable once InconsistentNaming
// ReSharper disable once NotAccessedField.Local
		private Container components;
		private readonly Dictionary<Control, bool> _extendedControls = new Dictionary<Control, bool>();
		private List<Control> _boundControls;
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

			var extend = GetExtendedTypes().Contains(ctrl.GetType());

			if (extend && !_extendedControls.ContainsKey(ctrl))
				_extendedControls[ctrl] = true;

			return extend;
		}

		/// ------------------------------------------------------------------------------------
		private static IEnumerable<Type> GetExtendedTypes()
		{
			yield return typeof(TextBox);
			yield return typeof(DatePicker);
			yield return typeof(ComboBox);
			yield return typeof(MultiValueDropDownBox);
			yield return typeof (CheckBox);
		}

		#endregion

		#region Properties provided by this extender
		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("BindingHelper Properties")]
		public bool GetIsBound(Control ctrl)
		{
			return ctrl != null && _extendedControls.TryGetValue(ctrl, out var isBound) && isBound;
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsBound(Control ctrl, bool bind)
		{
			if (ctrl == null)
				return;

			_extendedControls[ctrl] = bind;

			// Do this just in case this is being called from outside the initialize
			// components method and after the component file has been set.
			if (!bind)
				UnBindControl(ctrl, true);
		}

		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("BindingHelper Properties")]
		public bool GetIsComponentFileId(Control ctrl)
		{
			return (ctrl != null && _componentFileIdControl == ctrl);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsComponentFileId(Control ctrl, bool isFileId)
		{
			if (isFileId)
				_componentFileIdControl = ctrl;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that invalid file name characters cannot be entered into id fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void HandleIdFieldKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != (char)Keys.Back && Path.GetInvalidFileNameChars().Contains(e.KeyChar))
			{
				System.Media.SystemSounds.Beep.Play();
				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetComponentFile(ComponentFile file)
		{
			if (DesignMode)
				return;

			if (ComponentFile != null)
				ComponentFile.MetadataValueChanged -= HandleValueChangedOutsideBinder;

			ComponentFile = file;
			ComponentFile.MetadataValueChanged += HandleValueChangedOutsideBinder;

			// First, collect only the extended controls that are bound.
			_boundControls = _extendedControls.Where(x => x.Value).Select(x => x.Key).ToList();

			foreach (var ctrl in _boundControls)
			{
				ctrl.Font = Program.DialogFont;
				BindControl(ctrl);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void BindControl(Control ctrl)
		{
			UnBindControl(ctrl, false);

			if (!_boundControls.Contains(ctrl))
				_boundControls.Add(ctrl);

			UpdateControlValueFromField(ctrl);
			ctrl.Disposed += HandleDisposed;

			switch (ctrl)
			{
				case ComboBox comboBox when comboBox.DropDownStyle == ComboBoxStyle.DropDownList:
					comboBox.SelectedValueChanged += HandleBoundComboValueChanged;
					break;
				case DatePicker datePicker:
					datePicker.ValueChanged += HandleDateValueChanged;
					break;
				default:
				{
					ctrl.Validating += HandleValidatingControl;
					ValidateOnHide(ctrl);
					if (ctrl == _componentFileIdControl)
						ctrl.KeyPress += HandleIdFieldKeyPress;
					break;
				}
			}
		}

		private readonly HashSet<Control> _validateOnHideControls = new HashSet<Control>();

		// Note: this variable name seems to be misleading, as it often remains true even when
		// validation is not actively occurring.
		private bool _runningValidations;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the control or any of its parents gets hidden, e.g., by switching to another tab,
		/// perform validation, even if the new tab does not assign focus to something, which
		/// would normally trigger it. This ensures some important side effects of validation
		/// don't get skipped (e.g., updating a Person's name in Contributors).
		/// </summary>
		/// <param name="start"></param>
		/// ------------------------------------------------------------------------------------
		void ValidateOnHide(Control start)
		{
			var ctrl = start;
			// We don't need to start doing this until the main window gets activated.
			// This prevents any unexpected side effects or loops during startup.
			var form = ctrl.FindForm();
			if (form != null)
			{
				form.Activated -= StartValidating; // avoid duplicates
				form.Activated += StartValidating;
			}
			while (ctrl != null && !_validateOnHideControls.Contains(ctrl))
			{
				_validateOnHideControls.Add(ctrl);
				var localCopy = ctrl; // ctrl will have changed value by the time the event is raised
				ctrl.VisibleChanged += (sender, args) =>
				{
					if (_runningValidations && !localCopy.Visible)
					{
						// Somehow the validation process triggers something becoming invisible,
						// which can become an infinite loop. Waiting until the application is next
						// idle before we trigger any more seems to be enough to break the loop
						// (maybe after a couple of cycles).
						_runningValidations = false;
						Application.Idle += StartValidating;
						localCopy.FindForm()?.ValidateChildren();
					}
				};
				// And if it later gets added to some other parent, we need to add that to the process.
				ctrl.ParentChanged += (sender, args) =>
				{
					if (localCopy.Parent != null)
						ValidateOnHide(localCopy.Parent);
				};
				ctrl = ctrl.Parent;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void StartValidating(object o, EventArgs eventArgs)
		{
			Application.Idle -= StartValidating;
			_runningValidations = true;
		}

		/// ------------------------------------------------------------------------------------
		private void UnBindControl(Control ctrl, bool removeFromBoundControlCollection)
		{
			ctrl.Disposed -= HandleDisposed;

			switch (ctrl)
			{
				case ComboBox comboBox when comboBox.DropDownStyle == ComboBoxStyle.DropDownList:
					comboBox.SelectedValueChanged -= HandleBoundComboValueChanged;
					break;
				case DatePicker datePicker:
					datePicker.ValueChanged -= HandleDateValueChanged;
					break;
				default:
				{
					ctrl.Validating -= HandleValidatingControl;
					if (ctrl == _componentFileIdControl)
						ctrl.KeyPress -= HandleIdFieldKeyPress;
					break;
				}
			}

			if (removeFromBoundControlCollection &&  _boundControls != null && _boundControls.Contains(ctrl))
				_boundControls.Remove(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDisposed(object sender, EventArgs e)
		{
			UnBindControl(sender as Control, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when something happens (like choosing a preset) which modifies the values
		/// of the file directly, and we need to update the UI
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFieldsFromFile()
		{
			foreach (var ctrl in _boundControls)
				UpdateControlValueFromField(ctrl);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateControlValueFromField(Control ctrl)
		{
			var key = _makeIdFromControlName(ctrl);
			var stringValue = ComponentFile.GetStringValue(key, string.Empty);

			try
			{
				if (TranslateBoundValueBeingRetrieved != null)
				{
					var args = new TranslateBoundValueBeingRetrievedArgs(ctrl, stringValue);
					TranslateBoundValueBeingRetrieved(this, args);
					if (args.Handled)
						return;

					stringValue = (args.TranslatedValue ?? stringValue);
				}

				if (ctrl is CheckBox box)
				{
					bool.TryParse(stringValue, out var isChecked);
					box.Checked = isChecked;
				}
				else
				{
					ctrl.Text = stringValue;
				}
			}
			catch (Exception error)
			{
				if (error is AmbiguousDateException)
				{
					var notesField = ComponentFile.MetaDataFieldValues.FirstOrDefault(f => f.FieldId == "notes");
					if (notesField == null)
					{
						notesField = new FieldInstance("notes", "");
						ComponentFile.MetaDataFieldValues.Add(notesField);
					}

					notesField.Value = notesField.ValueAsString + Environment.NewLine +
						String.Format(LocalizationManager.GetString("CommonToMultipleViews.BindingHelper.AmbiguousDateNote",
							"***This record had an ambiguous {0}, produced by a bug in an old version of SayMore. The date was \"{1}\". " +
							"SayMore has attempted to interpret the date, but might have swapped the day and month. " +
							"Please accept our apologies for this error. After you have fixed the date or confirmed that it is correct, please delete this message.",
							"Text appended to the note for an element with an ambiguous date field value"), key, error.Message);

					//this will save that new value, and this note
					HandleValidatingControl(ctrl,new CancelEventArgs());
					return;

				}

				SIL.Reporting.ErrorReport.NotifyUserOfProblem(
					new SIL.Reporting.ShowOncePerSessionBasedOnExactMessagePolicy(), error,
						"SayMore had a problem displaying the {0}, which had a value of {1}." + Environment.NewLine +
						"Message " + error.Message + Environment.NewLine +
						"You should report this problem to the developers by clicking 'Details' below.",
						key, stringValue);
			}
		}

		/// ------------------------------------------------------------------------------------
		private Control GetBoundControlFromKey(string key)
		{
			var ctrlName = _makeControlNameFromId(key);
			return _boundControls.FirstOrDefault(c => c.Name == ctrlName);
		}

		/// ------------------------------------------------------------------------------------
		public string GetValue(string key)
		{
			return ComponentFile.GetStringValue(key, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void SetValues(IEnumerable<KeyValuePair<string, string>> values)
		{
			ComponentFile.MetadataValueChanged -= HandleValueChangedOutsideBinder;
			foreach (KeyValuePair<string, string> kvp in values)
				ComponentFile.SetStringValue(kvp.Key, kvp.Value);
			ComponentFile.MetadataValueChanged += HandleValueChangedOutsideBinder;

			SaveNow();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDateValueChanged(object sender, EventArgs e)
		{
			HandleValidatingControl(sender, new CancelEventArgs());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBoundComboValueChanged(object sender, EventArgs e)
		{
			HandleValidatingControl(sender, new CancelEventArgs());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleValidatingControl(object sender, CancelEventArgs e)
		{
			var ctrl = (Control)sender;
			var key = _makeIdFromControlName(ctrl);
			var box = ctrl as CheckBox;

			string newValue = null;
			var gotNewValueFromDelegate = false;

			if (TranslateBoundValueBeingSaved != null)
			{
				var args = new TranslateBoundValueBeingSavedArgs(ctrl);
				TranslateBoundValueBeingSaved(this, args);
				newValue = args.NewValue;
				gotNewValueFromDelegate = (newValue != null);
			}

			if (!gotNewValueFromDelegate)
			{
				if (box != null)
					newValue = box.Checked ? "true" : "false";
				else if (ctrl is ComboBox combo && combo.SelectedItem != null)
					newValue = combo.SelectedValue == null ? combo.SelectedItem.ToString() : combo.SelectedValue.ToString();
				else if (key != "date")
					newValue = ctrl.Text.Trim();
				else
				{
					// NB: we're doing a plain old-fashioned "parse" here because the editor is showing
					// it in the user's local culture, that's fine. But internally, we want to deal
					// only in DateTimes where possible, and in ISO8601 where strings are necessary.
					if (ctrl is DatePicker datePicker)
						newValue = datePicker.GetISO8601DateValueOrNull();
					else
						newValue = DateTime.Parse(newValue, CultureInfo.CurrentCulture).ToISO8601TimeFormatDateOnlyString();
				}
			}

			// Don't bother doing anything if the old value is the same as the new value.
			var oldValue = ComponentFile.GetStringValue(key, null);
			if (oldValue != null && oldValue == newValue)
				return;

			string failureMessage = null;

			ComponentFile.MetadataValueChanged -= HandleValueChangedOutsideBinder;

			// SP-742: Save changes before renaming the file ("Could not find a part of the path 'C:\...\NameOf.session'.")
			if (_componentFileIdControl == ctrl)
				SaveNow();

			newValue = (GetIsComponentFileId(ctrl) ?
				ComponentFile.TryChangeChangeId(newValue, out failureMessage) :
				ComponentFile.SetStringValue(key, newValue));

			ComponentFile.MetadataValueChanged += HandleValueChangedOutsideBinder;

			if (!gotNewValueFromDelegate && key != "date" && box == null)
				ctrl.Text = newValue;

			if (failureMessage != null)
			{
				e.Cancel = true;
				ValidationFailed?.Invoke(this, ctrl);
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
			}
			else
			{
				// ENHANCE: don't save so often, leave it to some higher level
				if (_componentFileIdControl != ctrl)
					SaveNow();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this event when a metadata value changes from somewhere other than from in
		/// this binding helper. When we get this message for bound fields, we need to make
		/// sure the bound control associated with the field, gets updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleValueChangedOutsideBinder(ComponentFile file, string fieldId,
			object oldValue, object newValue)
		{
			var ctrl = GetBoundControlFromKey(fieldId);
			if (ctrl != null)
				UpdateControlValueFromField(ctrl);
		}

		private void SaveNow()
		{
			ComponentFile.Save();
			if (OnDataSaved != null)
				OnDataSaved();
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class TranslateBoundValueBeingRetrievedArgs : EventArgs
	{
		public Control BoundControl { get; private set; }
		public string ValueFromFile { get; private set; }
		public string TranslatedValue { get; set; }

		/// <summary>
		/// When the Handled property is returned true, it means the delegate updated the
		/// control so nothing further is needed for the binder to do, even if the newValue
		/// comes back different from the valueFromFile. If Handled is false and newValue
		/// is null, then the valueFromFile is used. If Handled is false and newValue is not
		/// null, it will be used in place of valueFromFile.
		/// </summary>
		public bool Handled { get; set; }

		/// ------------------------------------------------------------------------------------
		public TranslateBoundValueBeingRetrievedArgs(Control boundControl, string valueFromFile)
		{
			BoundControl = boundControl;
			ValueFromFile = valueFromFile;
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class TranslateBoundValueBeingSavedArgs : EventArgs
	{
		public Control BoundControl { get; private set; }

		/// <summary>
		/// Initially, this value is null. If delegates do not want to translate the
		/// value, then NewValue should remain null.
		/// </summary>
		public string NewValue { get; set; }

		/// ------------------------------------------------------------------------------------
		public TranslateBoundValueBeingSavedArgs(Control boundControl)
		{
			BoundControl = boundControl;
		}
	}
}