using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SIL.IO;
using SIL.Reporting;
using SIL.Windows.Forms.FileSystem;
using SayMore.Properties;
using SayMore.Transcription.Model;
using static System.IO.Path;

namespace SayMore.Model.Files
{
	public class AnnotationComponentFile : ComponentFile
	{
		public const string kEafSettingsFileExtension = ".psfx";
		public ComponentFile AssociatedComponentFile { get; }
		private OralAnnotationComponentFile _oralAnnotationFile;

		/// ------------------------------------------------------------------------------------
		[Obsolete("For Mocking Only")]
		public AnnotationComponentFile():base(null) { }

		/// ------------------------------------------------------------------------------------
		public AnnotationComponentFile(ProjectElement parentElement,
			string pathToAnnotationFile, ComponentFile associatedComponentFile,
			List<FileType> fileTypes, IEnumerable<ComponentRole> componentRoles) :
			base(parentElement, pathToAnnotationFile,
			fileTypes.Single(t => t is AnnotationFileType), null, null, null)
		{
			Tiers = new TierCollection();

			AssociatedComponentFile = associatedComponentFile;
			_componentRoles = componentRoles;
			InitializeFileInfo();
			Load();

			SmallIcon = ResourceImageCache.ElanIcon;

			var oralAnnotationFilePath = GetSuggestedPathToOralAnnotationFile();
			if (File.Exists(oralAnnotationFilePath))
			{
				OralAnnotationFile = new OralAnnotationComponentFile(parentElement,
					oralAnnotationFilePath, associatedComponentFile, fileTypes, _componentRoles);
			}
			else
			{
				associatedComponentFile.PostGenerateOralAnnotationFileAction += generated =>
				{
					OralAnnotationFile = new OralAnnotationComponentFile(parentElement,
						oralAnnotationFilePath, associatedComponentFile, fileTypes, _componentRoles);
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual TierCollection Tiers { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string GetPathToAssociatedMediaFile()
		{
			return PathToAnnotatedFile.Substring(0,
				PathToAnnotatedFile.Length - AnnotationFileHelper.kAnnotationsEafFileSuffix.Length);
		}

		/// ------------------------------------------------------------------------------------
		public OralAnnotationComponentFile OralAnnotationFile
		{
			get => _oralAnnotationFile != null &&
				File.Exists(_oralAnnotationFile.PathToAnnotatedFile) ? _oralAnnotationFile : null;

			set => _oralAnnotationFile = (value != null &&
					File.Exists(value.PathToAnnotatedFile) ? value : null);
		}

		/// ------------------------------------------------------------------------------------
		public override int DisplayIndentLevel => 1;

		/// ------------------------------------------------------------------------------------
		public bool GetIsAnnotatingAudioFile()
		{
			return FileUtils.AudioFileExtensions.Contains(
				GetExtension(GetPathToAssociatedMediaFile().ToLower()));
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
		public sealed override void Load()
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
				Logger.WriteEvent("Handled Exception in AnnotationComponentFile.TryLoadAndReturnException:\r\n{0}", e.ToString());
				Tiers = savTiers;
				return e;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of to the component file's oral annotation file, even if the file
		/// doesn't exist. If the component file is not of a type that can have an annotation
		/// file, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetSuggestedPathToOralAnnotationFile()
		{
			return AssociatedComponentFile.PathToAnnotatedFile + Settings.Default.OralAnnotationGeneratedFileSuffix;
		}

		/// ------------------------------------------------------------------------------------
		public override void RenameAnnotatedFile(string newPath)
		{
			var oldEafSettingsFile = ChangeExtension(PathToAnnotatedFile,
				kEafSettingsFileExtension);
			base.RenameAnnotatedFile(newPath);
			AnnotationFileHelper.ChangeMediaFileName(PathToAnnotatedFile, GetPathToAssociatedMediaFile());

			if (!File.Exists(oldEafSettingsFile))
				return;

			var newEafSettingsFile = ChangeExtension(PathToAnnotatedFile,
				kEafSettingsFileExtension);
			File.Move(oldEafSettingsFile, newEafSettingsFile);

			_oralAnnotationFile?.RenameAnnotatedFile(GetSuggestedPathToOralAnnotationFile());
		}

		/// ------------------------------------------------------------------------------------
		protected internal override bool Delete()
		{
			// If the annotation file has an associated ELAN preference file, then delete it.
			var prefFilePath = ChangeExtension(PathToAnnotatedFile, ".pfsx");
			var oralAnnotationFile = OralAnnotationFile;

			if (!base.Delete())
				return false;

			if (File.Exists(prefFilePath))
				ConfirmRecycleDialog.Recycle(prefFilePath);

			if (oralAnnotationFile != null)
			{
				oralAnnotationFile.Delete();
				_oralAnnotationFile = null;
			}

			var segmentAnnotationFileFolder = SegmentAnnotationFileFolder;
			DirectoryInfo dirInfo = new DirectoryInfo(segmentAnnotationFileFolder);
			if (dirInfo.Exists)
			{
				try
				{
					foreach (FileInfo file in dirInfo.EnumerateFiles())
						ConfirmRecycleDialog.Recycle(file.FullName);
					Directory.Delete(segmentAnnotationFileFolder, true);
				}
				catch (Exception e)
				{
					Logger.WriteEvent("Handled Exception in AnnotationComponentFile.Delete:\r\n{0}", e.ToString());
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public string SegmentAnnotationFileFolder => 
			AssociatedComponentFile + Settings.Default.OralAnnotationsFolderSuffix;

		/// ------------------------------------------------------------------------------------
		public override bool HasSubordinateFiles => 
			_oralAnnotationFile != null || Directory.Exists(SegmentAnnotationFileFolder);

		/// ------------------------------------------------------------------------------------
		public override bool GetCanHaveAnnotationFile()
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeRenamedForRole => false;

		/// ------------------------------------------------------------------------------------
		public override bool CanBeCustomRenamed => false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the transcription and/or translation roles if all the segments are not
		/// empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetAssignedRoles()
		{
			var transcriptionTier = Tiers.GetTranscriptionTier();
			if (transcriptionTier != null && transcriptionTier.GetIsComplete())
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kTranscriptionComponentRoleId);

			// Stage status (complete-incomplete) is not displaying the status for Written Translations correctly, if one or more segments are ignored.
			var translationTier = Tiers.GetFreeTranslationTier();
			if (translationTier != null && translationTier.GetIsComplete(transcriptionTier))
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kFreeTranslationComponentRoleId);

			if (Tiers.GetIsAdequatelyAnnotated(OralAnnotationType.CarefulSpeech))
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kCarefulSpeechComponentRoleId);

			if (Tiers.GetIsAdequatelyAnnotated(OralAnnotationType.Translation))
				yield return _componentRoles.Single(r => r.Id == ComponentRole.kOralTranslationComponentRoleId);
		}
	}
}
