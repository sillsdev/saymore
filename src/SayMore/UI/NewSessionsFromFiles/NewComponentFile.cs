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
// File: NewComponentFile.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using SayMore.Model.Files;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewComponentFile : ComponentFile
	{
		public bool Selected { get; set; }

		/// ------------------------------------------------------------------------------------
		public NewComponentFile(string filePath) :
			base(filePath, ApplicationContainer.FilesTypes, null, null)
		{
			Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void Rename(string newPath)
		{
			PathToAnnotatedFile = newPath;
			DetermineFileType(newPath, ApplicationContainer.FilesTypes);
			InitializeFileTypeInfo(newPath);
		}
	}
}
