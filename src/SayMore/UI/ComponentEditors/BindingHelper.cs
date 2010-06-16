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
	public class BindingHelper : Component, IExtenderProvider
	{
		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		public event GetBoundControlValueHandler GetBoundControlValue;

		private Container components;
		private readonly Dictionary<Control, bool> _extendedControls = new Dictionary<Control, bool>();
		private ComponentFile _file;

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

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetComponentFile(ComponentFile file)
		{
			_file = file;

			foreach (var kvp in _extendedControls)
			{
				kvp.Key.Font = SystemFonts.IconTitleFont;

				if (kvp.Value)
					BindControl(kvp.Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when something happens (like chosing a preset) which modifies the values
		/// of the file directly, and we need to update the UI
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFieldsFromFile()
		{
			foreach (var kvp in _extendedControls)
			{
				if (kvp.Value)
					UpdateControlValueFromField(kvp.Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void BindControl(Control ctrl)
		{
			UpdateControlValueFromField(ctrl);
			ctrl.Validating += HandleValidatingControl;
			ctrl.Disposed += HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateControlValueFromField(Control ctrl)
		{
			var key = ctrl.Name.TrimStart('_');
			ctrl.Text = _file.GetStringValue(key, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void UnBindControl(Control ctrl)
		{
			ctrl.Validating -= HandleValidatingControl;
			ctrl.Disposed -= HandleDisposed;
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

		/// ------------------------------------------------------------------------------------
		private void HandleValidatingControl(object sender, CancelEventArgs e)
		{
			var control = (Control)sender;
			var key = control.Name.TrimStart('_');

			string newValue = null;
			var gotNewValueFromDelegate = (GetBoundControlValue != null &&
				!GetBoundControlValue(this, control, out newValue));

			string failureMessage;

			newValue = _file.SetValue(key, (newValue ?? control.Text.Trim()), out failureMessage);

			if (!gotNewValueFromDelegate)
				control.Text = newValue;

			if (failureMessage != null)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
			}

			//enchance: don't save so often, leave it to some higher level
			_file.Save();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDisposed(object sender, System.EventArgs e)
		{
			UnBindControl(sender as Control);
		}
	}
}