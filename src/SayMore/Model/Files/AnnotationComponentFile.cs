
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
			// path is ignored.
			_eafFileHelper.Save(Tiers.First(t => t.DataType == TierType.Audio ||
				t.DataType == TierType.Video), Tiers.Where(t => t.DataType == TierType.Text));
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
			// If the annotation has an associated ELAN preference file, then delete it.
			var path = Path.ChangeExtension(PathToAnnotatedFile, ".pfsx");
			if (File.Exists(path))
				ConfirmRecycleDialog.Recycle(path);

			// Delete this annotation file.
			ConfirmRecycleDialog.Recycle(PathToAnnotatedFile);
		}
	}
}
