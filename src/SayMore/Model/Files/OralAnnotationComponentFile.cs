using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;

namespace SayMore.Model.Files
{
	public class OralAnnotationComponentFile : ComponentFile
	{
		public ComponentFile AssociatedComponentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public OralAnnotationComponentFile(ProjectElement parentElement, string pathtoAnnotatedFile,
			ComponentFile associatedComponentFile, IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles, FileSerializer fileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider, PresetGatherer presetProvider,
			FieldUpdater fieldUpdater) :
			base(parentElement, pathtoAnnotatedFile, fileTypes, componentRoles,
				fileSerializer, statisticsProvider, presetProvider, fieldUpdater)
		{
			AssociatedComponentFile = associatedComponentFile;
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToOriginalMediaFile()
		{
			return PathToAnnotatedFile.Substring(0,
				PathToAnnotatedFile.Length -  Settings.Default.OralAnnotationGeneratedFileSuffix.Length);
		}

		/// ------------------------------------------------------------------------------------
		public override int DisplayIndentLevel
		{
			get { return 1; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetCanHaveAnnotationFile()
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeRenamedForRole
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeCustomRenamed
		{
			get { return false; }
		}
	}
}
