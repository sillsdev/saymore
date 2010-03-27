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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
	public class SessionFile : SessionFileBase
	{
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

			sessionFile.FullFilePath = fileName;

			// Initialize each field's display text from the template's field definitions.
			var template = sessionFile.Template;
			if (template != null)
			{
				//review: (jh) seemed to leave things empty if they didn't already have data: foreach (var sfd in sessionFile.Fields)
				foreach (var def in template.Fields)
				{
					//var def = template.GetFieldDefinition(sfd.Name);
					var field = sessionFile.Fields.FirstOrDefault(x => x.Name == def.FieldName);
					if(field==null)
					{
						sessionFile.Fields.Add(new SessionFileField(def.FieldName, def.DisplayName));
					}
					else
					{
						field.DisplayText = def.DisplayName;
					}
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
			return Path.ChangeExtension(fileName, Sponge.SessionMetaDataFileExtension);
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
		private SessionFile(string path) : base(path)
		{
			var template = Template;

			Fields = (template == null ? new List<SessionFileField>() :
				(template.Fields.Select(x => new SessionFileField(x.FieldName, x.DisplayName)).ToList()));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of a SessionFile to it's standoff markup file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(GetStandoffFile(FullFilePath), this);
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
		public List<SessionFileField> Fields { get; set; }

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FileName;
		}

		public IEnumerable<ToolStripItem> GetContextMenuItems(string sessionId)
		{
	 //enhance: move these two ones up to the base class
			yield return new ToolStripMenuItem("Show file in Windows Explorer...", null, HandleOpenInFileManager_Click);
			yield return new ToolStripMenuItem("Open in Program Associated with this File ...", null, HandleOpenInApp_Click);
			yield return new ToolStripMenuItem("Open in GoldWave...", null, HandleOpenInGoldWave_Click);

			bool needSeparator = true;
			foreach (var definition in SessionComponentDefinition.CreateHardCodedDefinitions())
			{
				if (definition.GetFileIsElligible(FullFilePath))
				{
					if (needSeparator)
					{
						needSeparator = false;
						yield return new ToolStripSeparator();
					}

					string label = string.Format("Rename For {0}", definition.Name);
					SessionComponentDefinition componentDefinition = definition;
					yield return new ToolStripMenuItem(label, null, (sender, args) => IdentifyAsComponent(componentDefinition, sessionId));
				}
			}
		}

		public void IdentifyAsComponent(SessionComponentDefinition definition, string sessionId)
		{
			string newPath = definition.GetCanoncialName(sessionId, FullFilePath);
			try
			{
				File.Move(FullFilePath, newPath);
				FullFilePath = newPath;
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open current session's folder in the OS' file manager. ENHANCE: After opening
		/// the file manager, it would be nice to select the current session file, but that
		/// appears to be harder to accomplish, so I leave that for a future exercise.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleOpenInFileManager_Click(object sender, EventArgs e)
		{
			Process.Start(Folder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Open current session file in its associated application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HandleOpenInApp_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(FullFilePath);
			}
			catch (Win32Exception)
			{
				// REVIEW: Is it OK to assume any Win32Exception is no application association?
				Utils.MsgBox(
					string.Format("No application is associated with {0}", FileName));
			}
		 }
		public void HandleOpenInGoldWave_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(@"c:\Program Files (x86)\GoldWave\GoldWave.exe", FullFilePath);
			}
			catch (Win32Exception)
			{
				// REVIEW: Is it OK to assume any Win32Exception is no application association?
				Utils.MsgBox(
					string.Format("Could not open with goldwave", FileName));
			}
		 }
	}
}
