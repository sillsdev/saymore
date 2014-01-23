using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.Extensions;
using Palaso.Reporting;
using SayMore.Model.Files;
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
			var destFolder = Program.CurrentProject.IMDIOutputDirectory;
			if (string.IsNullOrEmpty(destFolder))
				destFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IMDI Packages");

			var model = new IMDIArchivingDlgViewModel(Application.ProductName, element.Title, element.Id,
				element.ArchiveInfoDetails, element is Project, element.SetFilesToArchive, destFolder)
			{
				HandleNonFatalError = (exception, s) => ErrorReport.NotifyUserOfProblem(exception, s)
			};

			element.InitializeModel(model);

			using (var dlg = new IMDIArchivingDlg(model, ApplicationContainer.kSayMoreLocalizationId,
				Program.DialogFont, Settings.Default.ArchivingDialog))
			{
				dlg.ShowDialog(Program.ProjectWindow);
				Settings.Default.ArchivingDialog = dlg.FormSettings;

				// remember choice for next time
				if (model.OutputFolder != Program.CurrentProject.IMDIOutputDirectory)
				{
					Program.CurrentProject.IMDIOutputDirectory = model.OutputFolder;
					Program.CurrentProject.Save();
				}
			}
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
			var sessionFile = saymoreSession.MetaDataFile;

			// create IMDI session
			var imdiSession = model.AddSession(saymoreSession.Id);
			imdiSession.Title = saymoreSession.Title;

			// session location
			var address = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Address", null);
			var region = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Region", null);
			var country = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Country", null);
			var continent = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Continent", null);
			if (string.IsNullOrEmpty(address))
				address = saymoreSession.MetaDataFile.GetStringValue("location", null);

			imdiSession.Location = new ArchivingLocation { Address = address, Region = region, Country = country, Continent = continent };

			// session description (synopsis)
			var stringVal = saymoreSession.MetaDataFile.GetStringValue("synopsis", null);
			if (!string.IsNullOrEmpty(stringVal))
				imdiSession.AddDescription(new LanguageString { Value = stringVal });

			// session date
			stringVal = saymoreSession.MetaDataFile.GetStringValue("date", null);
			if (!string.IsNullOrEmpty(stringVal))
				imdiSession.SetDate(DateTime.Parse(stringVal).ToISO8601DateOnlyString());

			// session situation
			stringVal = saymoreSession.MetaDataFile.GetStringValue("situation", null);
			if (!string.IsNullOrEmpty(stringVal))
				imdiSession.AddKeyValuePair("Situation", stringVal);

			imdiSession.Genre = GetFieldValue(sessionFile, "genre");
			imdiSession.SubGenre = GetFieldValue(sessionFile, "additional_Sub-Genre");
			imdiSession.AccessCode = GetFieldValue(sessionFile, "access");
			imdiSession.Interactivity = GetFieldValue(sessionFile, "additional_Interactivity");
			imdiSession.Involvement = GetFieldValue(sessionFile, "additional_Involvement");
			imdiSession.PlanningType = GetFieldValue(sessionFile, "additional_Planning_Type");
			imdiSession.SocialContext = GetFieldValue(sessionFile, "additional_Social_Context");
			imdiSession.Task = GetFieldValue(sessionFile, "additional_Task");

			// custom session fields
			foreach (var item in saymoreSession.MetaDataFile.GetCustomFields())
				imdiSession.AddKeyValuePair(item.FieldId, item.ValueAsString);

			// actors
			var persons = saymoreSession.GetAllPersonsInSession();
			foreach (var person in persons)
			{
				ArchivingActor actor = new ArchivingActor
				{
					FullName = person.MetaDataFile.GetStringValue("fullName", person.Id),
					Name = person.Id,
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

		private static string GetFieldValue(ComponentFile file, string valueName)
		{
			var stringVal = file.GetStringValue(valueName, null);
			return string.IsNullOrEmpty(stringVal) ? null : stringVal;
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

					// SP-765:  Allow codes from Ethnologue that are not in the Arbil list
					if ((language == null) || (string.IsNullOrEmpty(language.EnglishName)))
						package.ContentIso3Languages.Add(new ArchivingLanguage(parts[0], parts[1], parts[1]));
					else
						package.ContentIso3Languages.Add(new ArchivingLanguage(language.Iso3Code, parts[1], language.EnglishName));
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
