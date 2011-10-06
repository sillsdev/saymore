using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Palaso.Extensions;
using SayMore.Model.Files;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("IsBound", typeof(Control))]
	[ProvideProperty("IsComponentFileId", typeof(Control))]
	public class BindingHelper : Component, IExtenderProvider
	{
		private readonly Func<Control, string> MakeIdFromControlName = (ctrl => ctrl.Name.TrimStart('_'));
		private readonly Func<string, string> MakeControlNameFromId = (id => "_" + id);

		public delegate bool TranslateBoundValueBeingSavedHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		/// <summary>
		/// When this method returns true, it means the delegate updated the control so nothing
		/// further is needed for the binder to do, even if the newValue comes back different
		/// from the valueFromFile. If false is returned and newValue is null, then the
		/// valueFromFile is used. If false is returned and newValue is not null, it will be
		/// used in place of valueFromFile.
		/// </summary>
		public delegate bool TranslateBoundValueBeingRetrievedHandler(BindingHelper helper,
			Control boundControl, string valueFromFile, out string newValue);

		public event TranslateBoundValueBeingSavedHandler TranslateBoundValueBeingSaved;
		public event TranslateBoundValueBeingRetrievedHandler TranslateBoundValueBeingRetrieved;

		public ComponentFile ComponentFile { get; private set; }

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
			yield return typeof(DateTimePicker);
			yield return typeof(ComboBox);
			yield return typeof(MultiValueComboBox);
		}

		#endregion

		#region Properties provided by this extender
		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("BindingHelper Properties")]
		public bool GetIsBound(Control ctrl)
		{
			bool isBound;
			return (ctrl != null && _extendedControls.TryGetValue(ctrl, out isBound) && isBound);
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
				UnBindControl(ctrl);
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
				ctrl.Font = SystemFonts.IconTitleFont;
				BindControl(ctrl);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void BindControl(Control ctrl)
		{
			if (!_boundControls.Contains(ctrl))
				_boundControls.Add(ctrl);

			if (ctrl is ComboBox && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
				((ComboBox)ctrl).SelectedValueChanged -= HandleBoundComboValueChanged;
			else
				ctrl.Validating -= HandleValidatingControl;

			ctrl.Disposed -= HandleDisposed;
			UpdateControlValueFromField(ctrl);
			ctrl.Disposed += HandleDisposed;

			if (ctrl is ComboBox && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
				((ComboBox)ctrl).SelectedValueChanged += HandleBoundComboValueChanged;
			else
				ctrl.Validating += HandleValidatingControl;
		}

		/// ------------------------------------------------------------------------------------
		private void UnBindControl(Control ctrl)
		{
			ctrl.Disposed -= HandleDisposed;

			if (ctrl is ComboBox && ((ComboBox)ctrl).DropDownStyle == ComboBoxStyle.DropDownList)
				((ComboBox)ctrl).SelectedValueChanged -= HandleBoundComboValueChanged;
			else
				ctrl.Validating -= HandleValidatingControl;

			if (_boundControls != null && _boundControls.Contains(ctrl))
				_boundControls.Remove(ctrl);
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
		private void UpdateControlValueFromField(Control ctrl)
		{
			var key = MakeIdFromControlName(ctrl);
			var stringValue = ComponentFile.GetStringValue(key, string.Empty);

			try
			{
				if (TranslateBoundValueBeingRetrieved != null)
				{
					string translatedValue;
					if (TranslateBoundValueBeingRetrieved(this, ctrl, stringValue, out translatedValue))
						return;

					stringValue = (translatedValue ?? stringValue);
				}

				ctrl.Text = stringValue;
			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
					new Palaso.Reporting.ShowOncePerSessionBasedOnExactMessagePolicy(), error,
					"SayMore had a problem displaying the {0}, which had a value of {1}. You should report this problem to the developers by clicking 'Details' below.",
					key, stringValue);
			}
		}

		/// ------------------------------------------------------------------------------------
		private Control GetBoundControlFromKey(string key)
		{
			var ctrlName = MakeControlNameFromId(key);
			return _boundControls.FirstOrDefault(c => c.Name == ctrlName);
		}

		/// ------------------------------------------------------------------------------------
		public string GetValue(string key)
		{
			return ComponentFile.GetStringValue(key, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public string SetValue(string key, string value)
		{
			string failureMessage;
			ComponentFile.MetadataValueChanged -= HandleValueChangedOutsideBinder;
			var modifiedValue = ComponentFile.SetStringValue(key, value, out failureMessage);
			ComponentFile.MetadataValueChanged += HandleValueChangedOutsideBinder;

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			ComponentFile.Save();

			return modifiedValue;
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
			var key = MakeIdFromControlName(ctrl);

			string newValue = null;

			var gotNewValueFromDelegate = (TranslateBoundValueBeingSaved != null &&
				TranslateBoundValueBeingSaved(this, ctrl, out newValue));

			if (!gotNewValueFromDelegate)
				newValue = ctrl.Text.Trim();

			// Don't bother doing anything if the old value is the same as the new value.
			var oldValue = ComponentFile.GetStringValue(key, null);
			if (oldValue != null && oldValue == newValue)
				return;

			string failureMessage;

			ComponentFile.MetadataValueChanged -= HandleValueChangedOutsideBinder;

			if (key == "date")
			{
				//NB: we're doing a plain old-fashioned "parse" here because the editor is showing it in the user's local culture,
				//that's fine.  But internally, we want to deal only in DateTimes where possible, and in ISO8601 where strings
				//are necessary.
				newValue = DateTime.Parse(newValue, CultureInfo.CurrentCulture).ToISO8601DateOnlyString();
			}

			newValue = (_componentFileIdControl == ctrl ?
				ComponentFile.TryChangeChangeId(newValue, out failureMessage) :
				ComponentFile.SetStringValue(key, newValue, out failureMessage));

			ComponentFile.MetadataValueChanged += HandleValueChangedOutsideBinder;

			if (!gotNewValueFromDelegate)
				ctrl.Text = newValue;

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			if (_componentFileIdControl != ctrl)
				ComponentFile.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this event when a meta data value changes from somewhere other than from in
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
	}
}