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
// File: WelcomeDialogViewModel.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SIL.Localization;
using Sponge2.Model;
using SilUtils;
using Sponge2;
using Sponge2.ProjectChoosingAndCreating.NewProjectDialog;

namespace Sponge2.ProjectChoosingAndCreating
{
	/// ----------------------------------------------------------------------------------------
	public class WelcomeDialogViewModel
	{
		public string ProjectSettingsFilePath { get; set; }

		/// ------------------------------------------------------------------------------------
		public string GetVersionInfo(string fmt)
		{
			var ver = Assembly.GetExecutingAssembly().GetName().Version;

			// The build number is just the number of days since 01/01/2000
			DateTime bldDate = new DateTime(2000, 1, 1).AddDays(ver.Build);

			return string.Format(fmt, ver.Major, ver.Minor, ver.Revision,
								 bldDate.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
//		public bool CreateNewProject(string prjFolder)
//		{
//			int isep = prjFolder.LastIndexOf(Path.DirectorySeparatorChar);
//			var prjFile = (isep >= 0 ? prjFolder.Substring(isep + 1) : prjFolder) + ".sprj";
//			prjFile = prjFile.Replace(" ", string.Empty);
//
//			ProjectSettingsFilePath = Path.Combine(prjFolder, prjFile);
//
//			// Review: do we care if their is already an .sprj file
//			// in the folder having a different name?
//
//			if (!File.Exists(ProjectSettingsFilePath))
//				return false; // Project.CreateAtLocation(ProjectSettingsFilePath);
//
//			var msg = LocalizationManager.LocalizeString(
//				"WelcomeDialog.ProjectAlreadyExistsMsg",
//				"A project already exists in the folder '{0}'.", "Dialog Boxes");
//
//			Utils.MsgBox(string.Format(msg, prjFolder), MessageBoxIcon.Exclamation);
//			ProjectSettingsFilePath = null;
//			return false;
//		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a list of recently used projects where each items key is the full path
		/// to the project file and the value is just the name of the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> RecentlyUsedProjects
		{
			get
			{
				foreach (var prjPath in MruProjects.Paths)
				{
					yield return new KeyValuePair<string, string>(prjPath,
																  Path.GetFileNameWithoutExtension(prjPath));
				}
			}
		}

		public bool CreateNewProject(string parentFolderPath, string newProjectName)
		{
			try
			{
				var project =Project.CreateAtLocation(parentFolderPath, newProjectName);
				ProjectSettingsFilePath = project.SettingsFilePath;
			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.ReportNonFatalException(error);
				return false;
			}
			return true;
		}
	}
}