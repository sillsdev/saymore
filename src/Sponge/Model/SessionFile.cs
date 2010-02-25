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
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Palaso.Reporting;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("sessionFile")]
	public class SessionFile
	{
		private string m_fileName;

		public const string TagDelimiter = ";";

		#region static methods
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
		/// Creates a SessionFile object for the specified file name. The first attempt to
		/// create a SessionFile object is via deserializing the specified file's standoff
		/// markup file. If that fails, it's assumed there is no standoff markup, therefore,
		/// a new SessionFile is instantiated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SessionFile Create(string fileName)
		{
			var standOffPath = GetStandoffFile(fileName);

			Exception e;
			var sessionFile = XmlSerializationHelper.DeserializeFromFile<SessionFile>(standOffPath, out e);
			if (e != null)
			{
				var msg = ExceptionHelper.GetAllExceptionMessages(e);
				Utils.MsgBox(msg);
			}

			if (sessionFile == null)
				return new SessionFile(fileName);

			sessionFile.FileName = fileName;

			// Initialize each field's display text from the template's field definitions.
			var template = sessionFile.Template;
			if (template != null)
			{
				foreach (var sfd in sessionFile.Data)
				{
					var def = template.GetFieldDefinition(sfd.FieldName);
					sfd.DisplayText = (def == null ? string.Empty : def.DisplayName);
				}
			}

			return sessionFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs the full path to the (stand-off markup) data file for the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetStandoffFile(string fileName)
		{
			return Path.ChangeExtension(fileName, Sponge.SessionFileExtension);
		}

		#endregion

		#region Contstructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFile"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFile()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFile"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SessionFile(string fileName) : this()
		{
			FileName = fileName;

			var template = Template;

			Data = (template == null ? new List<SessionFileData>() :
				(template.Fields.Select(x => new SessionFileData(x.FieldName, x.DisplayName)).ToList()));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of a SessionFile to it's standoff markup file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(GetStandoffFile(m_fileName), this);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tags associated with the session file. This is a semi-colon list
		/// of tags.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("tags")]
		public string Tags { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tags associated with the session file. This is a comma or
		/// semi-colon list of tags.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> TagList
		{
			get
			{
				if (Tags == null)
					return new List<string>(0);

				var list = Tags.Split(TagDelimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				return (from x in list
					   where x.Trim() != string.Empty
					   select x.Trim()).ToList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the notes associated with the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("notes")]
		public string Notes { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the standoff markup data for the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("data")]
		public List<SessionFileData> Data { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the file (without its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName
		{
			get { return Path.GetFileName(m_fileName); }
			private set
			{
				m_fileName = value;
				var icon = Icon.ExtractAssociatedIcon(m_fileName);
				LargeIcon = new Bitmap(icon.ToBitmap());
				SetMiscFileInfo();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file's path (i.e. full path without file name).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FilePath
		{
			get { return Path.GetDirectoryName(m_fileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last time the session's file was modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DateModified
		{
			get { return File.GetLastWriteTime(m_fileName).ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session file's type (i.e. the text that is displayed in the OS's file
		/// manager Type column).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileType { get; private set; }

		[XmlIgnore]
		public string FileSize { get; private set; }

		[XmlIgnore]
		public Bitmap LargeIcon { get; private set; }

		[XmlIgnore]
		public Bitmap SmallIcon { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the template contianing the field definitions associated with the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SessionFileInfoTemplate Template
		{
			get { return SessionFileInfoTemplateList.GetTemplateByExt(Path.GetExtension(FileName)); }
		}

		#endregion

		#region Methods for setting file information
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the small icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetMiscFileInfo()
		{
#if !MONO
			SHFILEINFO shinfo = new SHFILEINFO();
			SHGetFileInfo(m_fileName, 0, ref
				shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME |
				SHGFI_SMALLICON | SHGFI_ICON | SHGFI_DISPLAYNAME);

			SmallIcon = Icon.FromHandle(shinfo.hIcon).ToBitmap();
			FileType = shinfo.szTypeName;
			SetFileSize();
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
			// TODO: Figure out how to get FileType in Mono.
#endif
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the session file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetFileSize()
		{
			var fi = new FileInfo(m_fileName);

			if (fi.Length < 1000)
				FileSize = string.Format("{0} B", fi.Length);
			else if (fi.Length < Math.Pow(1024, 2))
			{
				var size = fi.Length / 1024;
				if (size < 1)
					size = 1;

				FileSize = string.Format("{0} KB", size.ToString("###"));
			}
			else if (fi.Length < Math.Pow(1024, 3))
			{
				var size = Math.Round(fi.Length / Math.Pow(1024, 2), 2, MidpointRounding.AwayFromZero);
				FileSize = string.Format("{0} MB", size.ToString("###.##"));
			}
			else
			{
				var size = Math.Round(fi.Length / Math.Pow(1024, 3), 2, MidpointRounding.AwayFromZero);
				FileSize = string.Format("{0} GB", size.ToString("###,###.##"));
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FileName;
		}
	}
}
