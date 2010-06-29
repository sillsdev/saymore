using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is kind of an experiment at the moment...
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProvideProperty("AutoCompleteKey", typeof(IComponent))]
	[ProvideProperty("UpdateGatherer", typeof(IComponent))]
	public class AutoCompleteHelper : Component, IExtenderProvider
	{
		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		private Container components;
		private readonly Dictionary<TextBox, string> _keysForTextBoxes = new Dictionary<TextBox, string>();
		private List<TextBox> _textBoxesNeedingNewList = new List<TextBox>(0);
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
			var ctrl = extendee as TextBox;
			if (ctrl == null)
				return false;

			if (!_keysForTextBoxes.ContainsKey(ctrl))
				_keysForTextBoxes[ctrl] = string.Empty;//default to no autocomplete

			return true;
		}

		#endregion

		#region Properties provided by this extender
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whethere or not the data gatherer is updated after the
		/// data in a supported text box is changed. TODO: Add support for this property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("AutoCompleteHelper Properties")]
		public bool GetUpdateGatherer(object obj)
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public void SetUpdateGatherer(object obj, bool updateGatherer)
		{
		}

		/// ------------------------------------------------------------------------------------
		[Localizable(false)]
		[Category("AutoCompleteHelper Properties")]
		public string GetAutoCompleteKey(object obj)
		{

			string key;
			if(_keysForTextBoxes.TryGetValue(obj as TextBox, out key))
			{
				return key;
			}
			return string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		public void SetAutoCompleteKey(object obj, string key)
		{
			var textbox = obj as TextBox;

			if (textbox == null)
				return;

			_keysForTextBoxes[textbox] = key;

			// Do this just in case this is being called from outside the initialize
			// components method and after the component file has been set.
			if (!string.IsNullOrEmpty(key)) //review (jh) doesn't know why we do this just when its empty
				AddSupport(textbox);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetAutoCompleteProvider(IMultiListDataProvider provider)
		{
			_provider = provider;
			_provider.NewDataAvailable += HandleNewDataAvailable;

			foreach (var textbox in _keysForTextBoxes.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Key))
				AddSupport(textbox);
		}

		/// ------------------------------------------------------------------------------------
		public void AddSupport(TextBox textbox)
		{
			textbox.Validated += HandleTextBoxValidated;
			textbox.Enter += HandleTextBoxEnter;
			textbox.Disposed += HandleDisposed;
			textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			textbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
		}

		/// ------------------------------------------------------------------------------------
		public void RemoveSupport(TextBox textbox)
		{
			textbox.AutoCompleteMode = default(AutoCompleteMode);
			textbox.AutoCompleteSource = AutoCompleteSource.None;
			textbox.Validating -= HandleTextBoxValidated;
			textbox.Enter -= HandleTextBoxEnter;
			textbox.Disposed -= HandleDisposed;
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewDataAvailable(object sender, EventArgs e)
		{
			_autoCompleteLists = _provider.GetValueLists();
			_textBoxesNeedingNewList = _keysForTextBoxes.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Key).ToList();
		}

		/// ------------------------------------------------------------------------------------
		void HandleTextBoxEnter(object sender, EventArgs e)
		{
			var textbox = sender as TextBox;
			if (textbox == null)
				return;

			if (_textBoxesNeedingNewList.Contains(textbox))
			{
				_textBoxesNeedingNewList.Remove(textbox);
				var newValues = new AutoCompleteStringCollection();

				IEnumerable<string> values;
				if (_autoCompleteLists.TryGetValue(_keysForTextBoxes[textbox], out values))
					newValues.AddRange(values.ToArray());

				textbox.AutoCompleteCustomSource = newValues;
			}

			textbox.SelectAll();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxValidated(object sender, EventArgs e)
		{
			// TODO: When support for UpdateGatherer property.
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDisposed(object sender, EventArgs e)
		{
			RemoveSupport(sender as TextBox);
		}
	}
}