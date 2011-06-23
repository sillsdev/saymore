
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.UI.WindowsForms.FileSystem;
using SayMore.Transcription.Model;

namespace SayMore.Model.Files
{
	public class AnnotationComponentFile : ComponentFile
	{
		public new delegate AnnotationComponentFile Factory(
			ProjectElement parentElement, string pathToAnnotationFile);

		public Action PreSaveAction;
		public Action PostSaveAction;

		private EafFileHelper _eafFileHelper;

		/// ------------------------------------------------------------------------------------
		public AnnotationComponentFile(ProjectElement parentElement,
			string pathToAnnotationFile, TextAnnotationFileType fileType)
			: base(parentElement, pathToAnnotationFile, fileType, null, null, null)
		{
			// The annotated file is the same as the annotation file.
			PathToAnnotatedFile = pathToAnnotationFile;
			InitializeFileInfo();
			Load();

			SmallIcon = Properties.Resources.ElanIcon;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ITier> Tiers { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string GetAssociatedMediaFile()
		{
			return PathToAnnotatedFile.Substring(0, PathToAnnotatedFile.Length - ".annotations.eaf".Length);
		}

		/// ------------------------------------------------------------------------------------
		public override int DisplayIndentLevel
		{
			get { return 1; }
		}

		/// ------------------------------------------------------------------------------------
		public override void Save(string path)
		{
			if (PreSaveAction != null)
				PreSaveAction();

			// path is ignored.
			_eafFileHelper.Save(Tiers.First(t => t.DataType == TierType.Audio ||
				t.DataType == TierType.Video), Tiers.Where(t => t.DataType == TierType.Text));

			if (PostSaveAction != null)
				PostSaveAction();
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			_eafFileHelper = new EafFileHelper(PathToAnnotatedFile, GetAssociatedMediaFile());
			Tiers = _eafFileHelper.GetTiers();
		}

		/// ------------------------------------------------------------------------------------
		public override void RenameAnnotatedFile(string newPath)
		{
			var oldPfsxFile = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
			base.RenameAnnotatedFile(newPath);
			EafFileHelper.UpdateMediaFileName(PathToAnnotatedFile, GetAssociatedMediaFile());

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
	}
}
