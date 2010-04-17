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
// File: PathValidator.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class PathValidator
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validates the path entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ValidatePathEntry(string basePath, string newFolderName,
			Label lblMsg, string validMsg, string invalidMsg, ToolTip tooltip)
		{
			if (tooltip != null)
				tooltip.SetToolTip(lblMsg, null);

			if (!PathOK(basePath, newFolderName))
			{
				newFolderName = newFolderName ?? string.Empty;
				lblMsg.Text = (newFolderName.Length > 0 ? invalidMsg : string.Empty);
				return false;
			}

			var fullPath = Path.Combine(basePath, newFolderName);
			string[] dirs = fullPath.Split(Path.DirectorySeparatorChar);
			if (dirs.Length > 1)
			{
				string root = Path.Combine(dirs[dirs.Length - 3], dirs[dirs.Length - 2]);
				root = Path.Combine(root, dirs[dirs.Length - 1]);
				lblMsg.Text = string.Format(validMsg ?? string.Empty, root);

				if (tooltip != null)
					tooltip.SetToolTip(lblMsg, fullPath);
			}

			lblMsg.Invalidate();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the name is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool PathOK(string basePath, string relativePath)
		{
			if (basePath == null || basePath.Trim().Length < 1)
				return false;

			if (relativePath == null || relativePath.Trim().Length < 1)
				return false;

			if (basePath.IndexOfAny(Path.GetInvalidPathChars()) > -1)
				return false;

			if (relativePath.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
				return false;

			var path = Path.Combine(basePath, relativePath);
			return (!Directory.Exists(path) && !File.Exists(path));
		}
	}
}