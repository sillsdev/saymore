using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using L10NSharp;
using L10NSharp.UI;
using L10NSharp.XLiffUtils;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Media;
using SayMore.Properties;
using SayMore.UI.Charts;
using static System.String;

namespace SayMore.UI.ComponentEditors
{
	public partial class MediaFileMoreInfoDlg : Form
	{
        private readonly string _mediaFilePath;
        private string _source;
        private static bool alreadyDisplayedEvenMoreInfoDisclaimer = false;

		/// ------------------------------------------------------------------------------------
		public MediaFileMoreInfoDlg()
		{
			InitializeComponent();

			_buttonClose.Click += delegate { Close(); };
            LocalizeItemDlg<XLiffDocument>.StringsLocalized += HandleStringsLocalized;

            HandleStringsLocalized();
        }

		/// ------------------------------------------------------------------------------------
		public MediaFileMoreInfoDlg(string mediaFileInfo) : this()
		{
			if (Settings.Default.MediaFileMoreInfoDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.MediaFileMoreInfoDlg = FormSettings.Create(this);
			}

			_mediaFilePath = mediaFileInfo;
			LoadBrowserControl();
		}
        

        /// ------------------------------------------------------------------------------------
        protected void HandleStringsLocalized(ILocalizationManager lm = null)
        {
            if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
            {
                _lblSource.Tag = _lblSource.Text;
                UpdateSourceLabelDisplay();
            }
        }

        /// ------------------------------------------------------------------------------------
        private void UpdateSourceLabelDisplay()
        {
            if (_source == null)
                _lblSource.Hide();
            else
            {
                _lblSource.Text = Format((string)_lblSource.Tag, _source);
                _lblSource.Show();
            }
        }

        /// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.MediaFileMoreInfoDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (_webBrowserInfo.DocumentStream != null)
			{
				_webBrowserInfo.DocumentStream.Dispose();
				_webBrowserInfo.DocumentStream = null;
			}

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private bool LoadBrowserControl()
		{
			if (_webBrowserInfo.DocumentStream != null)
			{
				_webBrowserInfo.DocumentStream.Dispose();
				_webBrowserInfo.DocumentStream = null;
			}

			var html = GetMediaInfo(out var source);
            _source = source;
            UpdateSourceLabelDisplay();
            if (html == null)
                return false;

			_webBrowserInfo.DocumentStream = TransformInfoOutput(html);
			_webBrowserInfo.Document.Encoding = "utf-8";
            return true;
        }

        /// ------------------------------------------------------------------------------------
		private string GetMediaInfo(out string source)
        {
            bool verbose = _buttonLessInfo.Visible;
			try
			{
				return MediaFileInfo.GetInfoAsHtml(_mediaFilePath, verbose, GetLocalizedLabel,
                    out source);
            }
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.GettingInfoErrorMsg",
					"There was an error trying to get more information for the media file:\r\n\r\n{0}");

				ErrorReport.NotifyUserOfProblem(e, msg, _mediaFilePath);
                // Since we failed, the "source" is sort of meaningless
                source = null;
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the specified input file using the xslt contained in the specified
		/// stream. The result is returned in a temporary file. It's expected the caller
		/// will move the file to the desired location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemoryStream TransformInfoOutput(string htmlInput)
		{
			var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlInput));
			var outputStream = new MemoryStream();
			var assembly = Assembly.GetExecutingAssembly();

			using (var xsltStream = assembly.GetManifestResourceStream(
				"SayMore.UI.ComponentEditors.ModifyMediaFileInfoOutput.xslt"))
			{
				if (xsltStream == null)
					return inputStream;

				try
				{
					var inputReader = XmlReader.Create(inputStream);
					var outputWriter = XmlWriter.Create(outputStream);

					using (var xsltReader = new XmlTextReader(xsltStream))
					{
						var xslt = new XslCompiledTransform(true);
						xslt.Load(xsltReader);
						xslt.Transform(inputReader, outputWriter);
						xsltReader.Close();
					}
				}
				catch
				{
					outputStream.Close();
					outputStream.Dispose();
					inputStream.Position = 0L;
					return inputStream;
				}
			}

			inputStream.Close();
			inputStream.Dispose();

			outputStream.Position = 0;
			var reader = new StreamReader(outputStream);
			var transformedHtml = reader.ReadToEnd();
			outputStream.Close();

			var styleInfo = Format("\r\n<style type=\"text/css\">{0}</style>",
				Resources.MoreMediaInfoStyles);

			transformedHtml = transformedHtml.Replace("<html>", HTMLChartBuilder.XMLDocTypeInfo);
			transformedHtml = transformedHtml.Replace("</head>", styleInfo + "</head>");

			return new MemoryStream(Encoding.UTF8.GetBytes(transformedHtml));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleEvenMoreInfoButtonClick(object sender, EventArgs e)
		{
            _buttonEvenMoreInfo.Visible = false;
			_buttonLessInfo.Visible = true;
            var origSource = _source;
            if (LoadBrowserControl() && origSource != _source &&
                !alreadyDisplayedEvenMoreInfoDisclaimer)
            {
                // Note: I'm hard-coding the utility program names in the localizer comment
                // because as things currently stand, that's definitely what they will be.
                // If the utility programs change in the future, this comment might need to
                // change.
                var msg = Format(LocalizationManager.GetString(
                    "DialogBoxes.MediaFileMoreInfoDlg.EvenMoreInfoDisclaimer",
                    "The information shown here is obtained using {0}. " +
                    "Some of the details might differ from what {1} reported.",
                    "Parameters are utility program names. Param 0: \"MediaInfo.DLL\";" +
                    " Param 1: \"FFprobe\""), _source, origSource);
                MessageBox.Show(this, msg, ProductName, MessageBoxButtons.OK);
                alreadyDisplayedEvenMoreInfoDisclaimer = true;
            }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLessInfoButtonClick(object sender, EventArgs e)
		{
			_buttonLessInfo.Visible = false;
			_buttonEvenMoreInfo.Visible = true;
			LoadBrowserControl();
		}

		private string GetLocalizedLabel(MediaFileInfo.HtmlLabels label)
		{
            switch (label)
            {
                case MediaFileInfo.HtmlLabels.General:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.General",
                        "General");
                case MediaFileInfo.HtmlLabels.FilePath:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.CompleteName",
                        "Complete name:");
                case MediaFileInfo.HtmlLabels.FileSize:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FileSize",
                        "File size (bytes):");
                case MediaFileInfo.HtmlLabels.Duration:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Duration",
                        "Duration:");
                case MediaFileInfo.HtmlLabels.StartTime:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.",
                        "Start time:");
                case MediaFileInfo.HtmlLabels.BitRate:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.",
                        "Bit rate:");
                case MediaFileInfo.HtmlLabels.Tags:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Tags",
                        "Tags");
                case MediaFileInfo.HtmlLabels.Format:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Format",
                        "Format");
                case MediaFileInfo.HtmlLabels.FmtName:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FmtName",
                        "Name:");
                case MediaFileInfo.HtmlLabels.FmtLongName:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FmtLongName",
                        "Long name:");
                case MediaFileInfo.HtmlLabels.FmtStreamCount:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FmtStreamCount",
                        "Stream count:");
                case MediaFileInfo.HtmlLabels.FmtProbeScore:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FmtProbeScore",
                        "Probe score:");
                case MediaFileInfo.HtmlLabels.Id:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.StreamIndex",
                        "ID:");
                case MediaFileInfo.HtmlLabels.CodecName:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.CodecName",
                        "Codec name:");
                case MediaFileInfo.HtmlLabels.CodecLongName:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.CodecLongName",
                        "Codec long name:");
                case MediaFileInfo.HtmlLabels.CodecTag:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.CodecTag",
                        "Codec tag:");
                case MediaFileInfo.HtmlLabels.Language:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Language",
                        "Language:");
                case MediaFileInfo.HtmlLabels.Disposition:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Disposition",
                        "Disposition");
                case MediaFileInfo.HtmlLabels.BitDepth:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.BitDepth",
                        "Bit depth:");
                case MediaFileInfo.HtmlLabels.Audio:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Audio",
                        "Audio");
                case MediaFileInfo.HtmlLabels.NumberedAudioStream:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.NumberedAudioStream",
                        "Audio Stream #{0}");
                case MediaFileInfo.HtmlLabels.ChannelCount:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.ChannelCount",
                        "Channel count:");
                case MediaFileInfo.HtmlLabels.ChannelLayout:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.ChannelLayout",
                        "Channel layout:");
                case MediaFileInfo.HtmlLabels.SampleRateHz:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.SampleRateHz",
                        "Sample rate (Hz):");
                case MediaFileInfo.HtmlLabels.Profile:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Profile",
                        "Profile:");
                case MediaFileInfo.HtmlLabels.Video:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Video",
                        "Video");
                case MediaFileInfo.HtmlLabels.NumberedVideoStream:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.NumberedVideoStream",
                        "Video Stream #{0}");
                case MediaFileInfo.HtmlLabels.BitsPerRawSample:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.BitsPerRawSample",
                        "Bits per raw sample:");
                case MediaFileInfo.HtmlLabels.DisplayAspectRatio:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.DisplayAspectRatio",
                        "Display aspect ratio:");
                case MediaFileInfo.HtmlLabels.SampleAspectRatio:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.SampleAspectRatio",
                        "Sample aspect ratio:");
                case MediaFileInfo.HtmlLabels.Width:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Width",
                        "Width:");
                case MediaFileInfo.HtmlLabels.Height:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Height",
                        "Height:");
                case MediaFileInfo.HtmlLabels.FrameRate:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.FrameRate",
                        "Frame rate:");
                case MediaFileInfo.HtmlLabels.PixelFormat:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.PixelFormat",
                        "Pixel format:");
                case MediaFileInfo.HtmlLabels.Rotation:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Rotation",
                        "Rotation:");
                case MediaFileInfo.HtmlLabels.AverageFrameRate:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.AverageFrameRate",
                        "Average frame rate:");
                case MediaFileInfo.HtmlLabels.SubtitleStreamCount:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.SubtitleStreamCount",
                        "Subtitle stream count:");
                case MediaFileInfo.HtmlLabels.ErrorData:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.ErrorData",
                        "Error data");
                case MediaFileInfo.HtmlLabels.Source:
                    return LocalizationManager.GetString("DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.HtmlLabels.Source",
                        "Data obtained using:");
                default:
                    return label.ToString();
            }
		}
	}
}
