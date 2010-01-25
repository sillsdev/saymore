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
// File: SpongeProject.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Sponge.ConfigTools;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information for a single Sponge project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("spongeproject")]
	public class SpongeProject
	{
		#region Static methods/properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject Create(IWin32Window parent)
		{
			using (var dlg = new NewProjectDlg())
			{
				return (dlg.ShowDialog(parent) == DialogResult.OK ?
					Create(dlg.NewProjectName) : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified project file. The file is a full path and file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SpongeProject Load(string prjFilePath)
		{
			Exception e;
			var prj = Utils.DeserializeData(prjFilePath, typeof(SpongeProject), out e) as SpongeProject;
			if (e != null)
			{
				Utils.MsgBox(e.Message);
				return null;
			}

			prj.ProjectPath = Path.GetDirectoryName(prjFilePath);
			prj.ProjectFileName = prjFilePath;
			int i = prj.ProjectPath.LastIndexOf(Path.DirectorySeparatorChar);
			prj.ProjectName = (i >= 0 ? prj.ProjectPath.Substring(i + 1) : prj.ProjectPath);
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Sponge project with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static SpongeProject Create(string prjName)
		{
			var prj = new SpongeProject();
			prj.ProjectName = prjName;
			prj.ProjectPath = Path.Combine(ProjectsFolder, prjName);
			prj.ProjectFileName = prjName.Replace(" ", string.Empty) + ".sprj";

			Directory.CreateDirectory(prj.ProjectPath);
			Directory.CreateDirectory(prj.SessionsPath);
			prj.Save();
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent folder for all project folders.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProjectsFolder
		{
			get { return Path.Combine(SpongeRunner.MainAppSettingsFolder, "Projects"); }
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectName { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's path (not including the project file name).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's file name (not including its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectFileName { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's path and file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullProjectPath
		{
			get { return Path.Combine(ProjectPath, ProjectFileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the folder in which all the project's sessions are saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SessionsPath
		{
			get { return Path.Combine(ProjectPath, "Sessions"); }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			Utils.SerializeData(FullProjectPath, this);
		}
	}
}
