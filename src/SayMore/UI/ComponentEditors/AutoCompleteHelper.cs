using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("AutoCompleteKey", typeof(Control))]
	[ProvideProperty("UpdateGatherer", typeof(Control))]
	public class AutoCompleteHelper : Component, IExtenderProvider
	{
		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		private Container components;
		private readonly Dictionary<Control, string> _keysForControls = new Dictionary<Control, string>();
		private List<Control> _controlsNeedingNewList = new List<Control>(0);
		private IMultiListDataProvider _provider;

		/// <summary>
		/// Given a key, get a list of strings for autocomplete
		/// </summary>
		private Dictionary<string, IEnumerable<string>> _autoCompleteLists;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		public AutoCompleteHelper()
		{
			// Required for Windows.Forms Class Composition Designer support
			components = new Container();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for instance that supports Class Composition designer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AutoCompleteHelper(IContainer container) : this()
		{
			container.Add(this);
		}

		#endregion

		#region IExtenderProvider Members
		/// ------------------------------------------------------------------------------------
		public bool CanExtend(object extendee)
		{
			if ((extendee is ComboBox && ((ComboBox)extendee).DropDownStyle != ComboBoxStyle.DropDown) ||
				!(extendee is TextBox || extendee is MultiValueComboBox))
			{
				return false;
			}

			var control = extendee as Control;
			if (!_keysForControls.ContainsKey(control))
				_keysForControls[control] = string.Empty; //default to no autocomplete

			return true;
		}

		#endregion

		#region Properties provided by this extender
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the data gatherer is updated after the
		/// data in a supported control is changed. TODO: Add support for this property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("AutoCompleteHelper Properties")]
		public bool GetUpdateGatherer(Control control)
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public void SetUpdateGatherer(Control control, bool updateGatherer)
		{
		}

		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("AutoCompleteHelper Properties")]
		public string GetAutoCompleteKey(Control control)
		{
			string key;
			return (_keysForControls.TryGetValue(control, out key) ? key : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void SetAutoCompleteKey(Control control, string key)
		{
			_keysForControls[control] = key;

			if (!string.IsNullOrEmpty(key))
				AddSupport(control);
			else
				RemoveSupport(control);

			// Do this just in case this is being called from outside the initialize
			// components method and after the component file has been set.
			//if (!string.IsNullOrEmpty(key)) //review (jh) doesn't know why we do this just when its empty
				//AddSupport(control);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetAutoCompleteProvider(IMultiListDataProvider provider)
		{
			_provider = provider;
			foreach (var control in _keysForControls.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Key))
				AddSupport(control);
			_provider.NewDataAvailable += HandleNewDataAvailable;
			HandleNewDataAvailable(null, null);
		}

		/// ------------------------------------------------------------------------------------
		public void AddSupport(Control control)
		{
			control.Validated += HandleControlValidated;
			control.Enter += HandleControlEnter;
			control.KeyDown += HandleControlKeyDown;
			control.Disposed += HandleDisposed;

			if (control is TextBox)
			{
				((TextBox)control).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				((TextBox)control).AutoCompleteSource = AutoCompleteSource.CustomSource;
			}
			else if (control is ComboBox)
			{
				((ComboBox)control).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				((ComboBox)control).AutoCompleteSource = AutoCompleteSource.CustomSource;
			}
			else if (control is MultiValueComboBox)
			{
				((MultiValueComboBox)control).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				((MultiValueComboBox)control).AutoCompleteSource = AutoCompleteSource.CustomSource;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveSupport(Control control)
		{
			if (control is TextBox)
			{
				((TextBox)control).AutoCompleteMode = default(AutoCompleteMode);
				((TextBox)control).AutoCompleteSource = AutoCompleteSource.None;
			}
			else if (control is ComboBox)
			{
				((ComboBox)control).AutoCompleteMode = default(AutoCompleteMode);
				((ComboBox)control).AutoCompleteSource = AutoCompleteSource.None;
			}
			else if (control is MultiValueComboBox)
			{
				((MultiValueComboBox)control).AutoCompleteMode = default(AutoCompleteMode);
				((MultiValueComboBox)control).AutoCompleteSource = AutoCompleteSource.None;
			}

			control.Validating -= HandleControlValidated;
			control.KeyDown -= HandleControlKeyDown;
			control.Enter -= HandleControlEnter;
			control.Disposed -= HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewDataAvailable(object sender, EventArgs e)
		{
			_autoCompleteLists = _provider.GetValueLists(true);

			_controlsNeedingNewList = (from kvp in _keysForControls
									   where !string.IsNullOrEmpty(kvp.Value)
									   select kvp.Key).ToList();
		}

		/// ------------------------------------------------------------------------------------
		static void HandleControlKeyDown(object sender, KeyEventArgs e)
		{
			// .Net has, what I would consider, a bug when a control has an auto-complete
			// list. It ignores Ctrl+A and sometimes doesn't ignore it, but clobbers all
			// the text in the control. Grr! (SP-114)
			if (e.Control && e.KeyCode == Keys.A)
			{
				SelectAllControlsText(sender as Control);
				e.SuppressKeyPress = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleControlEnter(object sender, EventArgs e)
		{
			var control = sender as Control;
			if (control == null)
				return;

			if (_controlsNeedingNewList.Contains(control))
			{
				_controlsNeedingNewList.Remove(control);
				var newValues = new AutoCompleteStringCollection();

				IEnumerable<string> values;
				if (_autoCompleteLists.TryGetValue(_keysForControls[control], out values))
					newValues.AddRange(values.ToArray());

				if (control is TextBox)
					((TextBox)control).AutoCompleteCustomSource = newValues;
				else if (control is ComboBox)
					((ComboBox)control).AutoCompleteCustomSource = newValues;
				//else if (control is MultiValueComboBox)
				//    ((MultiValueComboBox)control).AutoCompleteCustomSource = newValues;
			}

			SelectAllControlsText(control);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleControlValidated(object sender, EventArgs e)
		{
			// TODO: When support for UpdateGatherer property.
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDisposed(object sender, EventArgs e)
		{
			RemoveSupport(sender as Control);
		}

		/// ------------------------------------------------------------------------------------
		private static void SelectAllControlsText(Control control)
		{
			if (control is TextBox)
				((TextBox)control).SelectAll();
			else if (control is ComboBox)
				((ComboBox)control).SelectAll();
			else if (control is MultiValueComboBox)
				((MultiValueComboBox)control).SelectAll();
		}
	}
}