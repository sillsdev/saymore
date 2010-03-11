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
using System.Collections.Generic;
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
		private readonly Dictionary<string, TimeSpan> _durations= new Dictionary<string, TimeSpan>();

		#region static methods and properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session having the specified name and for the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Create(SpongeProject prj, string name)
		{
			var session = new Session(prj, name);
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
			string errorMsg;
			var session = Load(prj, pathName, out errorMsg);

			if (errorMsg == null)
				return session;

			Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Could not load session \"{0}\". {1}", pathName, errorMsg);
			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a session object by deserializing the specified file. The file can be just
		/// the name of the file, without the path, or the full path specification. If that
		/// fails, null is returned or, when there's an exception, it is thrown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Session Load(SpongeProject prj, string pathName, out string errorMsg)
		{
			errorMsg = null;

			var fileName = Path.GetFileName(pathName);
			var folder = fileName;
			if (folder.EndsWith("." + Sponge.SessionFileExtension))
				folder = fileName.Remove(folder.Length - (Sponge.SessionFileExtension.Length + 1));
			else
				fileName += ("." + Sponge.SessionFileExtension);

			folder = Path.Combine(prj.SessionsFolder, folder);
			var path = Path.Combine(folder, fileName);

			if(!File.Exists(path))
			{
				errorMsg = string.Format("The session file at \"{0}\" is missing.",path);
				return null;
			}

			Exception e;
			var session = XmlSerializationHelper.DeserializeFromFile<Session>(path, out e);
			if (e != null)
			{
				errorMsg = ExceptionHelper.GetAllExceptionMessages(e);
				return null;
			}
			if (session == null)//jh: I've noticed that DeserializeFromFile likes to return null, with no error.
			{
				errorMsg = "Cause unknown";
				return null;
			}

			session.Project = prj;
			session.Id = Path.GetFileNameWithoutExtension(fileName);
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
		private Session(SpongeProject prj, string id)
		{
			Project = prj;
			Id = id;
		}

		#region Serialized properties
//	    [XmlAttribute("id")]
		[XmlIgnore]
		public string Id
		{
			get;
			private set; //during construction, only
		}

		/// <summary>
		/// why is this separate from the property?  Because
		/// 1) You're not supposed to do anything non-trivial in property accessors (like renaming folders)
		/// 2) It may fail, and needs a way to indicate that to the caller.
		///
		/// NB: at the moment, all the change is done immediately, so a Save() is needed to keep things consistent.
		/// We could imagine just making the change pending until the next Save.
		/// </summary>
		/// <returns>true if the change was possible</returns>
		public bool ChangeIdAndSave(string newId)
		{
			newId = newId.Trim();
			if (Id == newId)
			{
				Save();
				return true;
			}

			if(newId == string.Empty)
			{
				return false;
			}

			var parent = Directory.GetParent(Folder).FullName;
			string newFolderPath = Path.Combine(parent, newId);
			if(Directory.Exists(newFolderPath))
			{
				return false;
			}

			try
			{
				foreach(var file in Directory.GetFiles(Folder))
				{
					var name = Path.GetFileName(file);
					if (name.ToLower().StartsWith(Id.ToLower()))// to be conservative, let's only trigger if it starts with the id
					{
						//todo: do a case-insensitive replacement
						//todo... this could over-replace
						File.Move(file, Path.Combine(Folder, name.Replace(Id, newId)));
					}
				}
				Directory.Move(Folder, newFolderPath);
			}
			catch(Exception)
			{
				return false;
			}
			Id = newId;
			Save();
			return true;
		}

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

		[XmlElement("eventType")]
		public string EventTypeId { get; set; }

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
//		/// ------------------------------------------------------------------------------------
//		/// <summary>
//		/// Gets the session's name.
//		/// </summary>
//		/// ------------------------------------------------------------------------------------
//		[XmlIgnore]
//		public string Name { get; private set; }

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
				return (string.IsNullOrEmpty(Id) ? null :
					Id + "." + Sponge.SessionFileExtension);
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
			get { return Path.Combine(Project.SessionsFolder, Id); }
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the discourse type of the event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DiscourseType EventType
		{
			get { return Sponge.DiscourseTypes.FirstOrDefault(x => x.Id == EventTypeId); }
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

			if (string.IsNullOrEmpty(Id))
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
			return Id;
		}

		public TimeSpan GetDurationOfMatchingFile(Func<string, bool> filter)
		{
			foreach (var path in Files)
			{
				if(filter(path))
				{
					TimeSpan duration;
					if (!_durations.TryGetValue(path, out duration))
					{
						//jh: a managed debugging assistant complains here, but I haven't found the solution
						//yet.
						using (var audio = new Microsoft.DirectX.AudioVideoPlayback.Audio(path))
						{
						   _durations.Add(path, new TimeSpan(0, 0, 0, (int) audio.Duration));
						}
					}
					return _durations[path];
				}
			}
			return new TimeSpan();
		}
	}
}
