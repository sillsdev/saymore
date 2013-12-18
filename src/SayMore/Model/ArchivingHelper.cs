using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Extensions;
using Palaso.Reporting;
using SayMore.Transcription.Model;
using SIL.Archiving;
using SIL.Archiving.Generic;
using SIL.Archiving.IMDI;
using SayMore.Properties;
using SIL.Archiving.IMDI.Lists;

namespace SayMore.Model
{
	static class ArchivingHelper
	{
		/// ------------------------------------------------------------------------------------
		internal static void ArchiveUsingIMDI(IIMDIArchivable element)
		{
			string destFolder;
			using (var chooseFolder = new FolderBrowserDialog())
			{
				chooseFolder.Description = LocalizationManager.GetString(
					"DialogBoxes.ArchivingDlg.ArchivingIMDILocationDescription",
					"Select a base folder where the IMDI directory structure should be created.");
				chooseFolder.ShowNewFolderButton = true;
				chooseFolder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				if (chooseFolder.ShowDialog() == DialogResult.Cancel)
					return;
				destFolder = chooseFolder.SelectedPath;
			}

			var model = new IMDIArchivingDlgViewModel(Application.ProductName, element.Title, element.Id,
				element.ArchiveInfoDetails, element is Project, element.SetFilesToArchive, destFolder)
			{
				HandleNonFatalError = (exception, s) => ErrorReport.NotifyUserOfProblem(exception, s),
				PathToProgramToLaunch = Settings.Default.ProgramToLaunchForIMDIPackage = GetProgramToLaunchForIMDIPackage()
			};

			element.InitializeModel(model);

			using (var dlg = new ArchivingDlg(model, ApplicationContainer.kSayMoreLocalizationId,
				Program.DialogFont, Settings.Default.ArchivingDialog))
			{
				dlg.ShowDialog();
				Settings.Default.ArchivingDialog = dlg.FormSettings;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string GetProgramToLaunchForIMDIPackage()
		{
			string defaultProgram = Settings.Default.ProgramToLaunchForIMDIPackage;
			if (!string.IsNullOrEmpty(defaultProgram))
			{
				if (File.Exists(defaultProgram))
					return defaultProgram;

				if (!Path.IsPathRooted(defaultProgram))
				{
					string rootedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), defaultProgram);
					if (File.Exists(rootedPath))
						return rootedPath;
					rootedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), defaultProgram);
					if (File.Exists(rootedPath))
						return rootedPath;
				}
			}

			using (var chooseIMDIProgram = new OpenFileDialog())
			{
				chooseIMDIProgram.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				chooseIMDIProgram.RestoreDirectory = true;
				chooseIMDIProgram.CheckFileExists = true;
				chooseIMDIProgram.CheckPathExists = true;
				chooseIMDIProgram.Filter = string.Format("{0} ({1})|{1}|{2} ({3})|{3}",
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ProgramsFileTypeLabel", "Programs"),
					"*.exe;*.pif;*.com;*.bat;*.cmd",
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AllFilesLabel", "All Files"),
					"*.*");
				chooseIMDIProgram.FilterIndex = 0;
				chooseIMDIProgram.Multiselect = false;
				chooseIMDIProgram.Title = LocalizationManager.GetString(
					"DialogBoxes.ArchivingDlg.SelectIMDIProgram", "Select the program to launch after IMDI package is created");
				chooseIMDIProgram.ValidateNames = true;
				if (chooseIMDIProgram.ShowDialog() == DialogResult.OK && File.Exists(chooseIMDIProgram.FileName))
					return chooseIMDIProgram.FileName;
			}

			return string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		static internal bool IncludeFileInArchive(string path, Type typeOfArchive, string metadataFileExtension)
		{
			if (path == null) return false;
			var ext = Path.GetExtension(path).ToLower();
			bool imdi = typeof(IMDIArchivingDlgViewModel).IsAssignableFrom(typeOfArchive);
			return (ext != ".pfsx" && (!imdi || (ext != metadataFileExtension)));
		}

		/// ------------------------------------------------------------------------------------
		static internal bool FileCopySpecialHandler(ArchivingDlgViewModel model, string source, string dest)
		{
			if (!source.EndsWith(AnnotationFileHelper.kAnnotationsEafFileSuffix))
				return false;

			// Fix EAF file to refer to modified name.
			AnnotationFileHelper annotationFileHelper = AnnotationFileHelper.Load(source);

			var mediaFileName = annotationFileHelper.MediaFileName;
			if (mediaFileName != null)
			{
				var normalizedName = model.NormalizeFilename(string.Empty, mediaFileName);
				if (normalizedName != mediaFileName)
				{
					annotationFileHelper.SetMediaFile(normalizedName);
					annotationFileHelper.Root.Save(dest);
					return true;
				}
			}
			return false;
		}

		internal static void SetIMDIMetadataToArchive(IIMDIArchivable element, ArchivingDlgViewModel model)
		{
			var project = element as Project;
			if (project != null)
			{
				AddIMDIProjectData(project, model);

				foreach (var session in project.GetAllSessions())
					AddIMDISession(session, model);
			}
			else
			{
				AddIMDISession((Session)element, model);
			}
		}

		private static void AddIMDISession(Session saymoreSession, ArchivingDlgViewModel model)
		{
			// create IMDI session
			var imdiSession = model.AddSession(saymoreSession.Id);
			imdiSession.Title = saymoreSession.Title;

			// session location
			var location = saymoreSession.MetaDataFile.GetStringValue("location", null);
			if (!string.IsNullOrEmpty(location))
				imdiSession.Location = new ArchivingLocation { Address = location };

			// session description (synopsis)
			var synopsis = saymoreSession.MetaDataFile.GetStringValue("synopsis", null);
			if (!string.IsNullOrEmpty(synopsis))
				imdiSession.AddDescription(new LanguageString { Value = synopsis });

			// session date
			var date = saymoreSession.MetaDataFile.GetStringValue("date", null);
			if (!string.IsNullOrEmpty(date))
				imdiSession.SetDate(date);

			// session situation
			var situation = saymoreSession.MetaDataFile.GetStringValue("situation", null);
			if (!string.IsNullOrEmpty(date))
				imdiSession.AddKeyValuePair("Situation", situation);

			// session genre
			var genre = saymoreSession.MetaDataFile.GetStringValue("genre", null);
			if (!string.IsNullOrEmpty(genre))
				imdiSession.Genre = genre;

			// session access
			var access = saymoreSession.MetaDataFile.GetStringValue("access", null);
			if (!string.IsNullOrEmpty(access))
				imdiSession.AccessCode = access;

			// custom session fields
			foreach (var item in saymoreSession.MetaDataFile.GetCustomFields())
				imdiSession.AddKeyValuePair(item.FieldId, item.ValueAsString);

			// actors
			var persons = saymoreSession.GetAllPersonsInSession();
			foreach (var person in persons)
			{
				ArchivingActor actor = new ArchivingActor
				{
					FullName = person.Id,
					BirthDate = person.MetaDataFile.GetStringValue("birthYear", string.Empty),
					Gender = person.MetaDataFile.GetStringValue("gender", null),
					Education = person.MetaDataFile.GetStringValue("education", null),
					Occupation = person.MetaDataFile.GetStringValue("primaryOccupation", null)
				};

				// do this to get the ISO3 codes for the languages because they are not in saymore
				var language = LanguageList.FindByEnglishName(person.MetaDataFile.GetStringValue("primaryLanguage", null));
				if (language != null)
					actor.PrimaryLanguage = new ArchivingLanguage(language.Iso3Code, language.EnglishName);

				language = LanguageList.FindByEnglishName(person.MetaDataFile.GetStringValue("mothersLanguage", null));
				if (language != null)
					actor.MotherTongueLanguage = new ArchivingLanguage(language.Iso3Code, language.EnglishName);

				// otherLanguage0 - otherLanguage3
				for (var i = 0; i < 4; i++)
				{
					language = LanguageList.FindByEnglishName(person.MetaDataFile.GetStringValue("otherLanguage" + i, null));
					if (language != null)
						actor.Iso3Languages.Add(new ArchivingLanguage(language.Iso3Code, language.EnglishName));
				}

				// custom person fields
				foreach (var item in person.MetaDataFile.GetCustomFields())
					actor.AddKeyValuePair(item.FieldId, item.ValueAsString);

				// actor files
				var actorFiles = Directory.GetFiles(person.FolderPath)
					.Where(f => IncludeFileInArchive(f, typeof(IMDIArchivingDlgViewModel), Settings.Default.PersonFileExtension));
				foreach (var file in actorFiles)
					actor.Files.Add(CreateArchivingFile(file));

				// add actor to imdi session
				imdiSession.AddActor(actor);
			}

			// session files
			var files = saymoreSession.GetSessionFilesToArchive(model.GetType());
			foreach (var file in files)
				imdiSession.AddFile(CreateArchivingFile(file));
		}

		private static void AddIMDIProjectData(Project saymoreProject, ArchivingDlgViewModel model)
		{
			var package = (IMDIPackage) model.ArchivingPackage;

			// location
			package.Location = new ArchivingLocation
			{
				Address = saymoreProject.Location,
				Region = saymoreProject.Region,
				Country = saymoreProject.Country,
				Continent = saymoreProject.Continent
			};

			// description
			package.AddDescription(new LanguageString(saymoreProject.ProjectDescription, null));

			// content type
			package.ContentType = saymoreProject.ContentType;

			// funding project
			package.FundingProject = new ArchivingProject
			{
				Title = saymoreProject.FundingProjectTitle,
				Name = saymoreProject.FundingProjectTitle
			};

			// athor
			package.Author = saymoreProject.ContactPerson;

			// applications
			package.Applications = saymoreProject.Applications;

			// access date
			package.Access.DateAvailable = saymoreProject.DateAvailable;

			// access owner
			package.Access.Owner = saymoreProject.RightsHolder;

			// publisher
			package.Publisher = saymoreProject.Depositor;

			// related publications
			package.AddKeyValuePair("Related Publications", saymoreProject.RelatedPublications);

			// subject language
			if (!string.IsNullOrEmpty(saymoreProject.VernacularISO3CodeAndName))
			{
				var parts = saymoreProject.VernacularISO3CodeAndName.SplitTrimmed(':').ToArray();
				if (parts.Length == 2)
				{
					var language = LanguageList.FindByISO3Code(parts[0]);
					if (language == null)
						package.ContentIso3Languages.Add(new ArchivingLanguage(parts[0], parts[1]));
					else
						package.ContentIso3Languages.Add(new ArchivingLanguage(language.Iso3Code, language.EnglishName));
				}
			}
		}

		private static ArchivingFile CreateArchivingFile(string fileName)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;
			const string metaFileSuffix = ".meta";

			var arcFile = new ArchivingFile(fileName);

			// is this an annotation file?
			if (fileName.EndsWith(annotationSuffix) || fileName.EndsWith(metaFileSuffix))
				arcFile.DescribesAnotherFile = fileName.Substring(0, fileName.Length - annotationSuffix.Length);

			// is this a meta file5555
			if (fileName.EndsWith(metaFileSuffix))
				arcFile.DescribesAnotherFile = fileName.Substring(0, fileName.Length - metaFileSuffix.Length);

			return arcFile;
		}
	}
}
