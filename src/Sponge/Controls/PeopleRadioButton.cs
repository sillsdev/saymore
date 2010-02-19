using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Control that acts like two radio buttons but uses an image of a male and female.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PeopleRadioButton : UserControl
	{
		private readonly Size m_maxMinSz;
		private readonly PictureBox m_picMale;
		private readonly PictureBox m_picFemale;
		private Gender m_value = Gender.Male;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="PeopleRadioButton"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PeopleRadioButton()
		{
			BackColor = Color.Transparent;

			m_picMale = new PictureBox();
			m_picFemale = new PictureBox();
			m_picMale.Tag = false;
			m_picFemale.Tag = false;

			m_picMale.Size = Resources.kimidMale_Selected.Size;
			m_picFemale.Width = m_picMale.Width;
			Size = new Size(m_picMale.Width + m_picFemale.Width + 4, m_picMale.Height);
			m_picMale.Dock = DockStyle.Left;
			m_picFemale.Dock = DockStyle.Right;
			m_maxMinSz = Size;

			Controls.AddRange(new[] { m_picMale, m_picFemale });

			m_picMale.Click += m_picMale_Click;
			m_picMale.MouseEnter += HandleImageMouseEnter;
			m_picMale.MouseLeave += HandleImageMouseLeave;
			m_picMale.Paint += HandleImagePaint;

			m_picFemale.Click += m_picFemale_Click;
			m_picFemale.MouseEnter += HandleImageMouseEnter;
			m_picFemale.MouseLeave += HandleImageMouseLeave;
			m_picFemale.Paint += HandleImagePaint;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Gender Value
		{
			get { return m_value; }
			set
			{
				if (!Enabled)
				{
					m_picMale.Image = Resources.kimidMale_NotSelected;
					m_picFemale.Image = Resources.kimidFemale_NotSelected;
				}
				else if (value == Gender.Male)
				{
					m_picMale.Image = Resources.kimidMale_Selected;
					m_picFemale.Image = Resources.kimidFemale_NotSelected;
					m_value = Gender.Male;
				}
				else
				{
					m_picMale.Image = Resources.kimidMale_NotSelected;
					m_picFemale.Image = Resources.kimidFemale_Selected;
					m_value = Gender.Female;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the control can respond to user interaction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new bool Enabled
		{
			get { return base.Enabled; }
			set
			{
				base.Enabled = true;
				Value = Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the the size that is the upper limit for the control. The setter for this is
		/// ignored, since the size is determined automatically.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Size MaximumSize
		{
			get { return m_maxMinSz; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the the size that is the lower limit for the control. The setter for this is
		/// ignored, since the size is determined automatically.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Size MinimumSize
		{
			get { return m_maxMinSz; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value using the specified string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string gender)
		{
			if (!string.IsNullOrEmpty(gender))
			{
				gender = gender.Trim().ToLower();

				try
				{
					Value = (Gender)Enum.Parse(typeof(Gender), gender, true);
					return;
				}
				catch
				{
					if (gender == "f" || gender == "woman" || gender == "girl" ||
						gender == "lady" || gender == "mom" || gender == "mother")
					{
						Value = Gender.Female;
						return;
					}
				}
			}

			Value = Gender.Male;
		}

		#region Picture box event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the picMale control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_picMale_Click(object sender, EventArgs e)
		{
			if (Enabled)
				Value = Gender.Male;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the picFemale control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_picFemale_Click(object sender, EventArgs e)
		{
			if (Enabled)
				Value = Gender.Female;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Repaint the picture when the mouse is over it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleImageMouseEnter(object sender, EventArgs e)
		{
			var pic = sender as PictureBox;
			pic.Tag = true;
			pic.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restore the image to it's state before the mouse was over it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleImageMouseLeave(object sender, EventArgs e)
		{
			var pic = sender as PictureBox;
			pic.Tag = false;
			pic.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the picture's Tag property is true (meaning the mouse is hovering over it)
		/// then paint the image to look like a selected toolbar button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleImagePaint(object sender, PaintEventArgs e)
		{
			var pic = sender as PictureBox;

			if (pic != null && (bool)pic.Tag)
			{
				Rectangle rc = pic.ClientRectangle;

				using (var br = new SolidBrush(ProfessionalColors.ButtonSelectedHighlight))
					e.Graphics.FillRectangle(br, rc);

				e.Graphics.DrawImage(pic.Image, rc);

				using (var pen = new Pen(ProfessionalColors.ButtonSelectedHighlightBorder))
				{
					rc.Height--;
					rc.Width--;
					e.Graphics.DrawRectangle(pen, rc);
				}
			}
		}

		#endregion
	}
}
