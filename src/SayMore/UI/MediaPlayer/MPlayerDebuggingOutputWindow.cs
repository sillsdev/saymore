using System;
using System.Windows.Forms;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI.MediaPlayer
{
	public partial class MPlayerDebuggingOutputWindow : Form
	{
		/// ------------------------------------------------------------------------------------
		public MPlayerDebuggingOutputWindow()
		{
			InitializeComponent();
			_buttonClose.Click += delegate { Close(); };
		}

		/// --------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			if (Settings.Default.MPlayerDebuggingOutputWindow == null)
				Settings.Default.MPlayerDebuggingOutputWindow = FormSettings.Create(this);
			else
				Settings.Default.MPlayerDebuggingOutputWindow.InitializeForm(this);

			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		public string GetText()
		{
			return _textOutput.Text;
		}

		/// ------------------------------------------------------------------------------------
		public void AddText(string text)
		{
			Invoke((MethodInvoker)delegate
			{
				_textOutput.AppendText(text + Environment.NewLine);
				_textOutput.SelectionLength = 0;
				_textOutput.SelectionStart = _textOutput.Text.Length;
			});
		}

		/// ------------------------------------------------------------------------------------
		private void HandleClearClick(object sender, EventArgs e)
		{
			_textOutput.Clear();
		}
	}
}
