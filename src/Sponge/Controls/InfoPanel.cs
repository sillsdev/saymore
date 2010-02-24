using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class InfoPanel : UserControl
	{
		public event EventHandler MoreActionButtonClicked;

		private int m_pendingSplitterPos;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="InfoPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public InfoPanel()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			var frm = FindForm();
			if (frm != null)
				frm.Shown += HandleParentFormShown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the splitter position only when the parent form is shown since that's the only
		/// time when the panel is at its full width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleParentFormShown(object sender, EventArgs e)
		{
			if (m_pendingSplitterPos > 0)
				splitContainer.SplitterDistance = m_pendingSplitterPos;

			m_pendingSplitterPos = 0;
			FindForm().Shown += HandleParentFormShown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the panel with the specified labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(IEnumerable fldInfo)
		{
			Utils.SetWindowRedraw(flwFileInfo, false);
			ClearInfo();

			if (fldInfo != null)
			{
				using (Graphics g = flwFileInfo.CreateGraphics())
				{
					int maxLblWidth = 0;
					foreach (var obj in fldInfo)
					{
						var info = obj as IInfoPanelField;
						if (info != null)
						{
							var ltb = new LabeledTextBox(info.DisplayText);
							ltb.Name = info.FieldName;
							ltb.InnerTextBox.Text = info.Value;
							ltb.Margin = new Padding(ltb.Margin.Left, 0, ltb.Margin.Right, 0);
							ltb.Font = flwFileInfo.Font;
							var dx = TextRenderer.MeasureText(g, info.DisplayText, ltb.Font).Width;
							maxLblWidth = Math.Max(maxLblWidth, dx);
							flwFileInfo.Controls.Add(ltb);
						}
					}

					foreach (Control ltb in flwFileInfo.Controls)
					{
						((LabeledTextBox)ltb).InnerLabel.Width = maxLblWidth;
						ltb.Width = maxLblWidth + 150;
					}
				}
			}

			Utils.SetWindowRedraw(flwFileInfo, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values in the panel's fields to the specified list of IInfoPanelField
		/// objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable fldInfo)
		{
			foreach (var obj in fldInfo)
			{
				var info = obj as IInfoPanelField;
				if (info != null)
				{
					var ltb = flwFileInfo.Controls[info.FieldName] as LabeledTextBox;
					if (ltb != null)
						info.Value = ltb.InnerTextBox.Text;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the info. labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ClearInfo()
		{
			flwFileInfo.Controls.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				lblFile.Font = value;
				lblNotes.Font = value;
				hctNotes.Font = value;
				flwFileInfo.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image Icon
		{
			get { return picIcon.Image; }
			set { picIcon.Image = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FileName
		{
			get { return lblFile.Text; }
			set { lblFile.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the notes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Notes
		{
			get { return hctNotes.Text.Trim(); }
			set { hctNotes.Text = (value != null ? value.Trim() : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the panel's splitter position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SplitterPosition
		{
			get { return splitContainer.SplitterDistance; }
			set
			{
				if (IsHandleCreated)
					splitContainer.SplitterDistance = value;
				else
					m_pendingSplitterPos = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the btnMoreAction button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoreAction_Click(object sender, EventArgs e)
		{
			if (MoreActionButtonClicked != null)
				MoreActionButtonClicked(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the SplitterMoved event of the splitContainer control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			lblNotes.Left = splitContainer.Left + e.SplitX + 5;
			lblFile.Width = lblNotes.Left - lblFile.Left - 5;
		}
	}
}
