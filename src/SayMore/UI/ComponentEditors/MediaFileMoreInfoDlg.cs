using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.PortableSettingsProvider;
using SayMore.Media;
using SayMore.Properties;
using SayMore.UI.Charts;

namespace SayMore.UI.ComponentEditors
{
	public partial class MediaFileMoreInfoDlg : Form
	{
		private readonly string _mediaFilePath;

		/// ------------------------------------------------------------------------------------
		public MediaFileMoreInfoDlg()
		{
			InitializeComponent();

			_buttonClose.Click += delegate { Close(); };
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
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.MediaFileMoreInfoDlg.InitializeForm(this);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (_webBrowserInfo.DocumentStream != null)
				_webBrowserInfo.DocumentStream.Dispose();

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadBrowserControl()
		{
			if (_webBrowserInfo.DocumentStream != null)
				_webBrowserInfo.DocumentStream.Dispose();

			var html = GetMediaInfo();

			_webBrowserInfo.DocumentStream = TransformInfoOutput(html);
			_webBrowserInfo.Document.Encoding = "utf-8";
		}

		/// ------------------------------------------------------------------------------------
		private string GetMediaInfo()
		{
			try
			{
				return MediaFileInfo.GetInfoAsHtml(_mediaFilePath, _buttonLessInfo.Visible);
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.ComponentEditors.MediaFileMoreInfoDlg.GettingInfoErrorMsg",
					"There was an error trying to get more information for the media file:\r\n\r\n{0}");

				ErrorReport.NotifyUserOfProblem(e, msg, _mediaFilePath);
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the specified input file using the xslt contained in the specifed
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

			var styleInfo = string.Format("\r\n<style type=\"text/css\">{0}</style>",
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
			LoadBrowserControl();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLessInfoButtonClick(object sender, EventArgs e)
		{
			_buttonLessInfo.Visible = false;
			_buttonEvenMoreInfo.Visible = true;
			LoadBrowserControl();
		}
	}
}
