using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;
using Palaso.UI.WindowsForms;

namespace SayMore.UI.LowLevelControls
{
	/// <summary>
	/// Provides a text box which dynamically updates the choices, as new
	/// ones become available. In the future, we could also make it handle
	/// more than one item (like "red, blue, green")
	///
	/// NB: this could eventually be more elegantly done with an Extender
	/// on TextEdit controls.
	/// </summary>
	public partial class AutoCompleteTextBox : TextBox
	{
		private LanguageNameGatherer _provider;

		public AutoCompleteTextBox()
		{
			InitializeComponent();
			Disposed += new EventHandler(AutoCompleteTextBox_Disposed);
		}

		void AutoCompleteTextBox_Disposed(object sender, EventArgs e)
		{
			if(_provider!=null)
			{
				_provider.NewDataAvailable -= _provider_NewDataAvailable;
			}
			_provider = null;

		}

		public AutoCompleteTextBox(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		public void SetChoiceProvider(LanguageNameGatherer provider)
		{
			_provider = provider;

			_provider.NewDataAvailable += new EventHandler(_provider_NewDataAvailable);
		}

		void _provider_NewDataAvailable(object sender, EventArgs e)
		{
			ReloadChoices();
		}

		private void ReloadChoices()
		{
			var languages = new AutoCompleteStringCollection();
			languages.AddRange(_provider.GetLanguages().ToArray());

			this.InvokeIfRequired(() => AutoCompleteCustomSource = languages);
		}

	}
}
