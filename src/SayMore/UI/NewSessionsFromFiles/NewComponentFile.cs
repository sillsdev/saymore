// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2014, SIL International. All Rights Reserved.
// <copyright from='2010' to='2014' company='SIL International'>
//		Copyright (c) 2014, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
//
// File: NewComponentFile.cs
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.NewSessionsFromFiles
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is like a ComponentFile except that it's for ComponentFiles displayed in
	/// the grid of files from which a user may create new sessions (i.e. when clicking the
	/// "New From Device" button on the sessions screen).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewComponentFile : ComponentFile
	{
		public delegate NewComponentFile NewComponentFileFactory(string pathToAnnotatedFile);

		private readonly IEnumerable<FileType> _fileTypes;
		public bool Selected { get; set; }

		/// ------------------------------------------------------------------------------------
		public NewComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles, XmlFileSerializer xmlFileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider, PresetGatherer presetProvider,
			FieldUpdater fieldUpdater)
			: base(null, pathToAnnotatedFile, fileTypes, componentRoles,
				xmlFileSerializer, statisticsProvider, presetProvider, fieldUpdater)
		{
			_fileTypes = fileTypes;
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
			DetermineFileType(newPath, _fileTypes);
			InitializeFileInfo();
		}
	}
}
