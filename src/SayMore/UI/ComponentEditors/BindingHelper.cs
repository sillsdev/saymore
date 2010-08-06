using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
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
		private readonly Func<Control, string> MakeIdFromControl = (x => x.Name.TrimStart('_'));

		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		//public event GetBoundControlValueHandler GetBoundControlValue;

		private Container components;

		private readonly Dictionary<Control, bool> _extendedControls =
			new Dictionary<Control, bool>();

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
			bool isBound;
			return (_extendedControls.TryGetValue(obj as Control, out isBound) ? isBound : false);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsBound(object obj, bool bind)
		{
			var ctrl = obj as Control;
			_extendedControls[ctrl] = bind;

			// Do this just in case this is being called from outside the initialize
			// components method and after the component file has been set.
			if (!bind)
				UnBindControl(ctrl);
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
		}

		/// ------------------------------------------------------------------------------------
		private void BindControl(Control ctrl)
		{
			if (!_boundControls.Contains(ctrl))
				_boundControls.Add(ctrl);

			ctrl.Disposed -= HandleDisposed;
			ctrl.Validating -= HandleValidatingControl;
			UpdateControlValueFromField(ctrl);
			ctrl.Validating += HandleValidatingControl;
			ctrl.Disposed += HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void UnBindControl(Control ctrl)
		{
			ctrl.Disposed -= HandleDisposed;
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
			var key = MakeIdFromControl(ctrl);
			var stringValue = _file.GetStringValue(key, string.Empty);
			try
			{
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
			var key = MakeIdFromControl(ctrl);

			string newValue = null;
			//this was used for comboboxes when they were special,
			//we may eventually have restricted choices again, but not for now
//			var gotNewValueFromDelegate = (GetBoundControlValue != null &&
//				GetBoundControlValue(this, ctrl, out newValue));

//			if (!gotNewValueFromDelegate)
				newValue = ctrl.Text.Trim();

			// Don't bother doing anything if the old value is the same as the new value.
			var oldValue = _file.GetStringValue(key, null);
			if (oldValue != null && oldValue == newValue)
				return;

			string failureMessage;

			newValue = (_componentFileIdControl == ctrl ?
				_file.TryChangeChangeId(newValue, out failureMessage) :
				_file.SetValue(key, newValue, out failureMessage));

			//if (!gotNewValueFromDelegate)
				ctrl.Text = newValue;

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);

			//enchance: don't save so often, leave it to some higher level
			if (_componentFileIdControl != ctrl)
				_file.Save();
		}
	}
}