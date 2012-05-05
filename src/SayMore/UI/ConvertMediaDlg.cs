using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Media.FFmpeg;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI
{
	public partial class ConvertMediaDlg : Form
	{
		private readonly ConvertMediaDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public ConvertMediaDlg(ConvertMediaDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;

			if (Settings.Default.ConvertMediaDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.ConvertMediaDlg = FormSettings.Create(this);
			}

			InitializeFonts();
			_labelFileToConvertValue.Text = Path.GetFileName(_viewModel.InputFile);
			_comboAvailableConversions.Items.AddRange(_viewModel.AvailableConversions);
			_comboAvailableConversions.SelectedItem = _viewModel.SelectedConversion;
			_comboAvailableConversions.SelectionChangeCommitted += delegate
			{
				_viewModel.SelectedConversion = _comboAvailableConversions.SelectedItem as FFmpegConversionInfo;
				UpdateDisplay();
			};

			_buttonCancel.Click += delegate { Close(); };
			_buttonDownload.Click += delegate
			{
				using (var dlg = new FFmpegDownloadDlg())
					dlg.ShowDialog();

				UpdateDisplay();
			};

			_labelDownloadNeeded.Tag = _labelDownloadNeeded.Text;
			Localization.UI.LocalizeItemDlg.StringsLocalized += delegate
			{
				_labelDownloadNeeded.Tag = _labelDownloadNeeded.Text;
				UpdateDisplay();
			};

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ConvertMediaDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFonts()
		{
			_labelOverview.Font = Program.DialogFont;
			_labelAvailableConversions.Font = Program.DialogFont;
			_labelFileToConvert.Font = Program.DialogFont;
			_labelFileToConvertValue.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
			_labelDownloadNeeded.Font = Program.DialogFont;
			_comboAvailableConversions.Font = Program.DialogFont;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFactoryMp4ConversionName()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.ConvertMediaDlg.ConvertToMp4AndAACConversionName",
				"Convert to mpeg4 with AAC Audio");
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelDownloadNeeded.Text = string.Format(_labelDownloadNeeded.Tag as string,
				_comboAvailableConversions.SelectedItem);

			_buttonBeginConversion.Enabled = FFmpegHelper.DoesFFmpegForSayMoreExist &&
				_comboAvailableConversions.SelectedItem != null;

			_tableLayoutFFmpegMissing.Visible = !FFmpegHelper.DoesFFmpegForSayMoreExist;
		}
	}
}
