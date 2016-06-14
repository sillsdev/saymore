using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SayMore.Utilities;
using SIL.IO;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace SayMore.UI
{
	public partial class FileLoadErrorsReportDlg : Form
	{
		private readonly List<XmlException> m_fileLoadErrors;

		public FileLoadErrorsReportDlg(List<XmlException> fileLoadErrors)
		{
			m_fileLoadErrors = fileLoadErrors;
			InitializeComponent();

			foreach (var fileLoadError in fileLoadErrors)
			{
				var item = new ListViewItem(fileLoadError.Message);
				if (fileLoadError.InnerException != null)
					item.SubItems.Add(fileLoadError.InnerException.Message);
				m_listFileErrors.Items.Add(item);
			}
		}

		private void m_listFileErrors_Resize(object sender, EventArgs e)
		{
			colHeaderFile.Width = m_listFileErrors.ClientSize.Width / 2;
			colHeaderError.Width = m_listFileErrors.ClientSize.Width - colHeaderFile.Width;
		}

		private void m_btnSave_Click(object sender, EventArgs e)
		{
			using (var dlg = new SaveFileDialog())
			{
				dlg.Title = LocalizationManager.GetString("DialogBoxes.FileLoadErrorsReportDlg.SaveFileDialog.Title",
					"Choose File Location");
				dlg.OverwritePrompt = false;
				dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				dlg.FileName = Path.ChangeExtension("File Load Errors Report", FileSystemUtils.kTextFileExtension);
				dlg.Filter = string.Format("{0}|{1}|{2}|{3}",
					String.Format(FileSystemUtils.LocalizedVersionOfTextFileDescriptor, "*" + FileSystemUtils.kTextFileExtension),
					"*" + FileSystemUtils.kTextFileExtension,
					String.Format(FileSystemUtils.LocalizedVersionOfAllFilesDescriptor, FileSystemUtils.kAllFilesFilter),
					FileSystemUtils.kAllFilesFilter);
				dlg.DefaultExt = FileSystemUtils.kTextFileExtension;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					using (var writer = new StreamWriter(dlg.FileName))
					{
						foreach (var loadError in m_fileLoadErrors)
						{
							writer.Write(loadError.Message);
							var innerException = loadError.InnerException;
							var tab = new StringBuilder();
							while (innerException != null)
							{
								tab.Append("\t");
								writer.WriteLine(tab + innerException.Message);
								innerException = innerException.InnerException;
							}
						}
					}
					PathUtilities.OpenFileInApplication(dlg.FileName);
				}
			}
		}

		private void m_btnExit_Click(object sender, EventArgs e)
		{
			Environment.Exit(Program.kFileLoadError);
		}

		private void m_btnContinue_Click(object sender, EventArgs e)
		{
			// Make sure we don't report these errors again.
			m_fileLoadErrors.Clear();
		}
	}
}
