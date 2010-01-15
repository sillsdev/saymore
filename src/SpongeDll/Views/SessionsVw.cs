using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Container for the sessions view.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SessionsVw : BaseSplitVw
	{
		public SessionsVw()
		{
			InitializeComponent();


			gridFiles.AlternatingRowsDefaultCellStyle.BackColor =
				ColorHelper.CalculateColor(Color.Black, gridFiles.DefaultCellStyle.BackColor, 10);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This fixes a paint error in .Net that manifests itself when tab controls are
		/// resized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabSessions_SizeChanged(object sender, EventArgs e)
		{
			tabSessions.Invalidate();
		}

		private void btnAddFile_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Filter = "All Files (*.*)|*.*";
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;

				if (dlg.ShowDialog() == DialogResult.OK)
					AddFile(dlg.FileName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFile(string fileName)
		{




			//object[] cells = new object[gridFiles.ColumnCount];
			////cells[0] = bm;
			//cells[0] = IconHelper.GetIconAsBitmapFromFile(fileName);
			//cells[1] = Path.GetFileName(fileName);
			//cells[2] = Convert.ToString(shinfo.szTypeName.Trim());
			//FileInfo fi = new FileInfo(fileName);
			//cells[4] = fi.LastWriteTime.ToString();
			//gridFiles.AddRow(cells);
		}
	}
}
