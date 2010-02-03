// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: SessionFile.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionFile
	{
		private readonly string m_fileName;

		public Bitmap LargeIcon { get; private set; }
		public Bitmap SmallIcon { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an array of SessionFile objects from the specified list of session names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SessionFile[] CreateArray(string[] sessionFileNames)
		{
			if (sessionFileNames == null)
				return null;

			var sessionFiles = new List<SessionFile>(sessionFileNames.Length);
			foreach (string file in sessionFileNames)
				sessionFiles.Add(new SessionFile(file));

			return sessionFiles.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFile"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFile(string fileName)
		{
			m_fileName = fileName;

			Icon icon = Icon.ExtractAssociatedIcon(m_fileName);
			LargeIcon = new Bitmap(icon.ToBitmap());
			SetSmallIcon();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the file (without its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FileName
		{
			get { return Path.GetFileName(m_fileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the small icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetSmallIcon()
		{
#if !MONO
				SHFILEINFO shinfo = new SHFILEINFO();
				IntPtr i = SHGetFileInfo(m_fileName, 0, ref
					shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME |
					SHGFI_SMALLICON | SHGFI_ICON | SHGFI_DISPLAYNAME);

				SmallIcon = Icon.FromHandle(shinfo.hIcon).ToBitmap();
#else
				// REVIEW: Figure out a better way to get this in Mono.
				Icon icon = Icon.ExtractAssociatedIcon(m_fileName);
				var largeIcons = new ImageList();
				largeIcons.Images.Add(icon);
				var bmSmall = new Bitmap(16, 16);

				using (var g = Graphics.FromImage(bmSmall))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage(LargeIcon, new Rectangle(0, 0, 16, 16),
						new Rectangle(new Point(0, 0), LargeIcon.Size), GraphicsUnit.Pixel);
				}

				SmallIcon = bmSmall;
#endif
		}

		#region Windows only stuff for getting file information
#if !MONO
		public const uint SHGFI_DISPLAYNAME = 0x00000200;
		public const uint SHGFI_TYPENAME = 0x400;
		public const uint SHGFI_EXETYPE = 0x2000;
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
		public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint
			dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
#endif
		#endregion
	}
}
