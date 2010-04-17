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
// File: SpongeSettingsProvider.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Subclass the settings provider so this application can specify the location for
	/// the user settings file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SpongeSettingsProvider : PortableSettingsProvider
	{
		// This allows tests to specify a temp. location which can be deleted on test cleanup.
		public static string SettingsFileFolder { get; set; }

		public override string SettingsFilePath
		{
			get
			{
				//REVIEW: we're abandoning the fixed document folder
				//return SettingsFileFolder ?? Sponge.MainApplicationFolder;
				if(!string.IsNullOrEmpty(SettingsFileFolder))
					return SettingsFileFolder;
				else
				{
					var path =Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sponge");
					if(!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					return path;
				}
			}
		}
	}
}
