using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.UI.WindowsForms.FileSystem;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Model.Files
{
	public class AnnotationComponentFile : ComponentFile
	{
		public new delegate AnnotationComponentFile Factory(
			ProjectElement parentElement, string pathToAnnotationFile);

		private AnnotationFileHelper _helper;

		/// ------------------------------------------------------------------------------------
		public AnnotationComponentFile(ProjectElement parentElement,
			string pathToAnnotationFile, TextAnnotationFileType fileType,
			IEnumerable<ComponentRole> componentRoles)
			: base(parentElement, pathToAnnotationFile, fileType, null, null, null)
		{
			// The annotated file is the same as the annotation file.
			PathToAnnotatedFile = pathToAnnotationFile;
			_componentRoles = componentRoles;
			InitializeFileInfo();
			Load();

			SmallIcon = Resources.ElanIcon;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> Tiers { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string GetPathToAssociatedMediaFile()
		{
			return PathToAnnotatedFile.Substring(0,
				PathToAnnotatedFile.Length - ".annotations.eaf".Length);
		}

		/// ------------------------------------------------------------------------------------
		public override int DisplayIndentLevel
		{
			get { return 1; }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsAnnotatingAudioFile()
		{
			return Settings.Default.AudioFileExtensions.Contains(Path.GetExtension(GetPathToAssociatedMediaFile()));
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
			// path is ignored.

			base.OnBeforeSave(this);
			_helper.Save(Tiers.First(t => t.DataType == TierType.Text) as TextTier);
			base.OnAfterSave(this);
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			_helper = AnnotationFileHelper.Load(PathToAnnotatedFile);
			Tiers = _helper.GetTiers();
		}

		/// ------------------------------------------------------------------------------------
		public override void RenameAnnotatedFile(string newPath)
		{
			var oldPfsxFile = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
			base.RenameAnnotatedFile(newPath);
			AnnotationFileHelper.ChangeMediaFileName(PathToAnnotatedFile, GetPathToAssociatedMediaFile());

			if (!File.Exists(oldPfsxFile))
				return;

			var newPfsxFile = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
			File.Move(oldPfsxFile, newPfsxFile);
		}

		/// ------------------------------------------------------------------------------------
		public void Delete()
		{
			// If the annotation file has an associated ELAN preference file, then delete it.
			var path = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
			if (File.Exists(path))
				ConfirmRecycleDialog.Recycle(path);

			// Delete this annotation file.
			ConfirmRecycleDialog.Recycle(PathToAnnotatedFile);
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetTranscriptionTier()
		{
			return Tiers.FirstOrDefault(t => t.DisplayName == TextTier.TranscriptionTierName) as TextTier;
		}

		/// ------------------------------------------------------------------------------------
		public TextTier GetFreeTranslationTier()
		{
			var transcriptionTier = GetTranscriptionTier();

			return (transcriptionTier == null ? null :
				transcriptionTier.DependentTiers.FirstOrDefault(t =>
					t.DisplayName == TextTier.FreeTranslationTierName) as TextTier);
		}

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the transcription and/or translation roles if all the segments are not
		/// empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetAssignedRoles(Type elementType)
		{
			var tier = GetTranscriptionTier();
			if (tier != null && tier.GetIsComplete())
				yield return _componentRoles.Single(r => r.Id == "transcription");

			tier = GetFreeTranslationTier();
			if (tier != null && tier.GetIsComplete())
				yield return _componentRoles.Single(r => r.Id == "transcriptionN");
		}
	}
}
