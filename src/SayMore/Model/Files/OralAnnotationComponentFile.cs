using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.Transcription.Model;

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
		public string GetPathToSourceMediaFile()
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

		/// ------------------------------------------------------------------------------------
		public override void GenerateOralAnnotationFile(Control parentOfProgressPopup,
			GenerateOption option)
		{
			var eafFile = AssociatedComponentFile.GetAnnotationFile();
			if (eafFile == null)
				return; // nothing we can do.
			var tiers = eafFile.Tiers;

			if (option == GenerateOption.GenerateIfNeeded)
			{
				var oralAnnotationFilename = tiers.GetTimeTier().MediaFileName + Settings.Default.OralAnnotationGeneratedFileSuffix;
				var finfo = new FileInfo(oralAnnotationFilename);
				if (!finfo.Exists || finfo.Length == 0)
					option = GenerateOption.RegenerateNow;
			}
			AssociatedComponentFile.GenerateOralAnnotationFile(tiers, parentOfProgressPopup, option);
		}
	}
}
