using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;

namespace SayMore.Model.Files
{
	public class OralAnnotationComponentFile : ComponentFile
	{
		/// ------------------------------------------------------------------------------------
		public OralAnnotationComponentFile(ProjectElement parentElement, string pathtoAnnotatedFile,
			IEnumerable<FileType> fileTypes, IEnumerable<ComponentRole> componentRoles,
			FileSerializer fileSerializer, IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider, FieldUpdater fieldUpdater) :
			base(parentElement, pathtoAnnotatedFile, fileTypes, componentRoles,
				fileSerializer, statisticsProvider, presetProvider, fieldUpdater)
		{
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToOriginalMediaFile()
		{
			return PathToAnnotatedFile.Substring(0,
				PathToAnnotatedFile.Length -  Settings.Default.OralAnnotationGeneratedFileAffix.Length);
		}

		/// ------------------------------------------------------------------------------------
		public override int DisplayIndentLevel
		{
			get { return 1; }
		}

		///// ------------------------------------------------------------------------------------
		//public override void RenameAnnotatedFile(string newPath)
		//{
		//    var oldPfsxFile = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
		//    base.RenameAnnotatedFile(newPath);
		//    AnnotationFileHelper.ChangeMediaFileName(PathToAnnotatedFile, GetPathToAssociatedMediaFile());

		//    if (!File.Exists(oldPfsxFile))
		//        return;

		//    var newPfsxFile = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
		//    File.Move(oldPfsxFile, newPfsxFile);
		//}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ToolStripItem> GetRenamingMenuCommands(Action<string> refreshAction)
		{
			return new ToolStripItem[] { };
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetCanHaveAnnotationFile()
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetCanBeCustomRenamed()
		{
			return false;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Return the transcription and/or translation roles if all the segments are not
		///// empty.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public override IEnumerable<ComponentRole> GetAssignedRoles(Type elementType)
		//{
		//    var tier = GetTranscriptionTier();
		//    if (tier != null && tier.GetIsComplete())
		//        yield return _componentRoles.Single(r => r.Id == "transcription");

		//    tier = GetFreeTranslationTier();
		//    if (tier != null && tier.GetIsComplete())
		//        yield return _componentRoles.Single(r => r.Id == "transcriptionN");
		//}
	}
}
