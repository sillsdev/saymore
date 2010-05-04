using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sponge2.Model.Files;

namespace Sponge2.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("IsBound", typeof(IComponent))]
	public class BindingHelper : Component, IExtenderProvider
	{
		private Container components;
		private readonly Dictionary<Control, bool> _extendedTextBoxes = new Dictionary<Control, bool>();
		private ComponentFile _file;

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

			if (extend && !_extendedTextBoxes.ContainsKey(ctrl))
				_extendedTextBoxes[ctrl] = true;

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
			return (_extendedTextBoxes.TryGetValue(obj as Control, out isBound) ? isBound : false);
		}

		/// ------------------------------------------------------------------------------------
		public void SetIsBound(object obj, bool bind)
		{
			var ctrl = obj as Control;
			_extendedTextBoxes[ctrl] = bind;

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

			foreach (var kvp in _extendedTextBoxes)
			{
				kvp.Key.Font = SystemFonts.IconTitleFont;

				if (kvp.Value)
					BindControl(kvp.Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void BindControl(Control ctrl)
		{
			var key = ctrl.Name.TrimStart('_');
			ctrl.Text = _file.GetStringValue(key, string.Empty);
			ctrl.Validating += HandleValidatingControl;
			ctrl.HandleDestroyed += HandleHandleDestroyed;
		}

		/// ------------------------------------------------------------------------------------
		public void UnBindControl(Control ctrl)
		{
			ctrl.Validating -= HandleValidatingControl;
			ctrl.HandleDestroyed -= HandleHandleDestroyed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleValidatingControl(object sender, CancelEventArgs e)
		{
			var control = (Control)sender;
			var key = control.Name.TrimStart('_');

			string failureMessage;
			control.Text = _file.SetValue(key, control.Text.Trim(), out failureMessage);
			if(failureMessage !=null)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
			}

			//enchance: don't save so often, leave it to some higher level

			_file.Save();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHandleDestroyed(object sender, System.EventArgs e)
		{
			var control = (Control)sender;
			control.Validating -= HandleValidatingControl;
			control.HandleDestroyed -= HandleHandleDestroyed;
		}
	}
}