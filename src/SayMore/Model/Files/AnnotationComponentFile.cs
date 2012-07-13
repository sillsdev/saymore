using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.UI.WindowsForms.FileSystem;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Model.Files
{
	public class AnnotationComponentFile : ComponentFile
	{
		public ComponentFile AssociatedComponentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		[Obsolete("For Mocking Only")]
		public AnnotationComponentFile() { }

		/// ------------------------------------------------------------------------------------
		public AnnotationComponentFile(ProjectElement parentElement,
			string pathToAnnotationFile, ComponentFile associatedComponentFile,
			FileType fileType, IEnumerable<ComponentRole> componentRoles)
			: base(parentElement, pathToAnnotationFile, fileType, null, null, null)
		{
			Tiers = new TierCollection();

			// The annotated file is the same as the annotation file.
			PathToAnnotatedFile = pathToAnnotationFile;
			AssociatedComponentFile = associatedComponentFile;
			_componentRoles = componentRoles;
			InitializeFileInfo();
			Load();

			SmallIcon = Resources.ElanIcon;
		}

		/// ------------------------------------------------------------------------------------
		public virtual TierCollection Tiers { get; private set; }

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
			return Settings.Default.AudioFileExtensions.Contains(
				Path.GetExtension(GetPathToAssociatedMediaFile().ToLower()));
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
			// path is ignored.

			base.OnBeforeSave(this);
			Tiers.Save(AssociatedComponentFile.PathToAnnotatedFile);
			base.OnAfterSave(this);
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			TryLoadAndReturnException();
		}

		/// ------------------------------------------------------------------------------------
		public Exception TryLoadAndReturnException()
		{
			var savTiers = Tiers;

			try
			{
				Tiers = TierCollection.LoadFromAnnotationFile(PathToAnnotatedFile);
			}
			catch (Exception e)
			{
				Tiers = savTiers;
				return e;
			}

			return null;
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
		/// <summary>
		/// Return the transcription and/or translation roles if all the segments are not
		/// empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetAssignedRoles()
		{
			var tier = Tiers.GetTranscriptionTier();
			if (tier != null && tier.GetIsComplete())
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kTranscriptionComponentRoleId);

			tier = Tiers.GetFreeTranslationTier();
			if (tier != null && tier.GetIsComplete())
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kFreeTranslationComponentRoleId);

			if (Tiers.GetIsFullyAnnotated(OralAnnotationType.CarefulSpeech))
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kCarefulSpeechComponentRoleId);

			if (Tiers.GetIsFullyAnnotated(OralAnnotationType.Translation))
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kOralTranslationComponentRoleId);
		}
	}
}
