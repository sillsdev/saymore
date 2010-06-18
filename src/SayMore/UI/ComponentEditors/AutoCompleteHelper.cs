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
	[ProvideProperty("ProvideAutoCompleteSupport", typeof(IComponent))]
	[ProvideProperty("UpdateGatherer", typeof(IComponent))]
	public class AutoCompleteHelper : Component, IExtenderProvider
	{
		public delegate bool GetBoundControlValueHandler(BindingHelper helper,
			Control boundControl, out string newValue);

		private Container components;
		private readonly Dictionary<TextBox, bool> _extendedTextBoxes = new Dictionary<TextBox, bool>();
		private List<TextBox> _textBoxesNeedingNewList = new List<TextBox>(0);
		private IDataGatherer _provider;
		private string[] _autoCompleteValues;

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

			if (!_extendedTextBoxes.ContainsKey(ctrl))
				_extendedTextBoxes[ctrl] = false;

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
		public bool GetProvideAutoCompleteSupport(object obj)
		{
			bool isSupportedProvided;
			return (_extendedTextBoxes.TryGetValue(obj as TextBox, out isSupportedProvided) ?
				isSupportedProvided : false);
		}

		/// ------------------------------------------------------------------------------------
		public void SetProvideAutoCompleteSupport(object obj, bool provideSupport)
		{
			var textbox = obj as TextBox;

			if (textbox == null)
				return;

			_extendedTextBoxes[textbox] = provideSupport;

			// Do this just in case this is being called from outside the initialize
			// components method and after the component file has been set.
			if (!provideSupport)
				AddSupport(textbox);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void SetDataGatherer(IDataGatherer provider)
		{
			_provider = provider;
			_provider.NewDataAvailable += HandleNewDataAvailable;

			foreach (var textbox in _extendedTextBoxes.Where(x => x.Value).Select(x => x.Key))
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
			_autoCompleteValues = _provider.GetValues().ToArray();
			_textBoxesNeedingNewList = _extendedTextBoxes.Where(x => x.Value).Select(x => x.Key).ToList();
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
				newValues.AddRange(_autoCompleteValues);
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