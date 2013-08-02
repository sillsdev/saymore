using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SayMore.Properties;

namespace SayMore.Model.Files
{
	public class OralAnnotationComponentFile : ComponentFile
	{
		public ComponentFile AssociatedComponentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public OralAnnotationComponentFile(ProjectElement parentElement, string pathToAnnotatedFile,
			ComponentFile associatedComponentFile, IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles) :
			base(parentElement, pathToAnnotatedFile, fileTypes, componentRoles,
				associatedComponentFile.XmlFileSerializer, associatedComponentFile.StatisticsProvider,
				associatedComponentFile.PresetProvider, associatedComponentFile.FieldUpdater)
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
			get { return 2; }
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

			if (option == GenerateOption.GenerateIfNeeded)
			{
				var oralAnnotationFilename = GetPathToSourceMediaFile() + Settings.Default.OralAnnotationGeneratedFileSuffix;
				var finfo = new FileInfo(oralAnnotationFilename);
				if (!finfo.Exists || finfo.Length == 0)
					option = GenerateOption.RegenerateNow;
			}
			AssociatedComponentFile.GenerateOralAnnotationFile(eafFile.Tiers, parentOfProgressPopup, option);
		}
	}
}
