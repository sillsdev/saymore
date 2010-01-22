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
					Create(dlg.PathOfNewProject) : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Sponge project in the specified folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static SpongeProject Create(string prjName)
		{
			var prj = new SpongeProject();
			prj.ProjectPath = Path.Combine(MainProjectsFolder, prjName);
			prj.ProjectFileName = Path.ChangeExtension(prjName.Replace(" ", string.Empty), "sprj");
			Directory.CreateDirectory(prj.ProjectPath);
			Directory.CreateDirectory(prj.SessionsPath);
			return prj;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent folder for all project folders.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MainProjectsFolder
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments), "Sponge");
			}
		}

		#endregion

		#region Properties
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
			string fullPath = Path.Combine(ProjectPath, ProjectFileName);
			Utils.SerializeData(fullPath, this);
		}
	}
}
