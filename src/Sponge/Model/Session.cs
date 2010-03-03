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
// File: Session.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.Reporting;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information about a single session.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("session")]
	public class Session
	{
		#region static methods and properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session having the specified name and for the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Create(SpongeProject prj, string name)
		{
			var session = new Session(prj, name);

			if (!Directory.Exists(session.Folder))
				Directory.CreateDirectory(session.Folder);

			return session;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session object by deserializing the specified file. The file can be just
		/// the name of the file, without the path, or the full path specification. If that
		/// fails, null is returned or, when there's an exception, it is thrown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Load(SpongeProject prj, string pathName)
		{
			var fileName = Path.GetFileName(pathName);
			var folder = fileName;
			if (folder.EndsWith("." + Sponge.SessionFileExtension))
				folder = fileName.Remove(folder.Length - (Sponge.SessionFileExtension.Length + 1));
			else
				fileName += ("." + Sponge.SessionFileExtension);

			folder = Path.Combine(prj.SessionsFolder, folder);
			fileName = Path.Combine(folder, fileName);

			Exception e;
			var session = XmlSerializationHelper.DeserializeFromFile<Session>(fileName, out e);
			if (e != null)
			{
				var msg = ExceptionHelper.GetAllExceptionMessages(e);
				Utils.MsgBox(msg);
				return null;
			}

			session.Project = prj;
			session.Name = Path.GetFileNameWithoutExtension(fileName);
			return session;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Session()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Session"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Session(SpongeProject prj, string name)
		{
			Project = prj;
			Name = name;
		}

		#region Serialized properties
		[XmlAttribute("id")]
		public string Id { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the date. Use this property rather than the SerializableDate.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DateTime Date
		{
			get
			{
				DateTime dt;
				if (SerializedDate != null && DateTime.TryParse(SerializedDate, out dt))
					return dt;

				return DateTime.Now;
			}
			set { SerializedDate = value.ToShortDateString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the serialized date. This property is only for serialization and
		/// deserialization. Use the Date property otherwise.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("date")]
		public string SerializedDate { get; set; }

		[XmlElement("title")]
		public string Title { get; set; }

		[XmlElement("participants")]
		public string Participants { get; set; }

		[XmlElement("access")]
		public string Access { get; set; }

		[XmlElement("setting")]
		public string Setting { get; set; }

		[XmlElement("location")]
		public string Location { get; set; }

		[XmlElement("situation")]
		public string Situation { get; set; }

		[XmlElement("synopsis")]
		public string Synopsis { get; set; }

		#endregion

		#region Non serialized properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Name { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the session file (without its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName
		{
			get
			{
				return (string.IsNullOrEmpty(Name) ? null :
					Name + "." + Sponge.SessionFileExtension);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path (including filename and extension) of the session's file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullFilePath
		{
			get { return (Folder == null || FileName == null ? null : Path.Combine(Folder, FileName)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the session's owning project
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SpongeProject Project { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the session's folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Folder
		{
			get { return Path.Combine(Project.SessionsFolder, Name); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full paths to all the session files found in the session's folder.
		/// If the folder for the session does not exist, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] Files
		{
			get
			{
				if (!Directory.Exists(Folder))
					return null;

				return (from x in Directory.GetFiles(Folder, "*.*")
						where (!x.EndsWith("." + Sponge.SessionMetaDataFileExtension) &&
							!x.EndsWith("." + Sponge.SessionFileExtension))
						orderby x
						select x).ToArray();
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of the session to it's file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Save()
		{
			// REVIEW: Should probably be more intelligent in responding to when saving fails.

			if (string.IsNullOrEmpty(Name))
				return false;

			if (!Directory.Exists(Folder))
				Directory.CreateDirectory(Folder);

			return XmlSerializationHelper.SerializeToFile(FullFilePath, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified files to the session folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AddFiles(string[] sessionFiles)
		{
			Debug.Assert(sessionFiles != null);

			if (!Directory.Exists(Folder))
				return false;

			bool fileWatchingState = Project.EnableFileWatching;
			Project.EnableFileWatching = false;

			foreach (string filePath in sessionFiles)
			{
				if (File.Exists(filePath) && Path.GetDirectoryName(filePath) != Folder)
				{
					var file = Path.GetFileName(filePath);

					// TODO: Deal with file when it already exists in session folder.
					File.Copy(filePath, Path.Combine(Folder, file));
				}
			}

			Project.EnableFileWatching = fileWatchingState;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
