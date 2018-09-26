using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Fields;
using SIL.Extensions;
using SIL.Reporting;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.Overview;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;
using SayMore.Utilities;
using SIL.Archiving;
using SIL.Archiving.Generic;
using SIL.Archiving.IMDI;
using SayMore.Properties;
using SIL.Archiving.IMDI.Lists;
using SIL.WritingSystems;
using Application = System.Windows.Forms.Application;

namespace SayMore.Model
{
	static class ArchivingHelper
	{
		internal static LanguageLookup _LanguageLookup = new LanguageLookup();
		internal static Project Project = null; // Set for testing

		/// ------------------------------------------------------------------------------------
		internal static void ArchiveUsingIMDI(IIMDIArchivable element)
		{
			var destFolder = Program.CurrentProject.IMDIOutputDirectory;

			// Move IMDI export folder to be under the mydocs/saymore
			if (string.IsNullOrEmpty(destFolder))
				destFolder = Path.Combine(NewProjectDlgViewModel.ParentFolderPathForNewProject, "IMDI Packages");

			// SP-813: If project was moved, the stored IMDI path may not be valid, or not accessible
			if (!CheckForAccessiblePath(destFolder))
			{
				destFolder = Path.Combine(NewProjectDlgViewModel.ParentFolderPathForNewProject, "IMDI Packages");
			}

			// now that we added a separate title field for projects, make sure it's not empty
			var title = string.IsNullOrEmpty(element.Title) ? element.Id : element.Title;

			var model = new IMDIArchivingDlgViewModel(Application.ProductName, title, element.Id,
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
		internal static void ArchiveUsingRAMP(IRAMPArchivable element)
		{
			// now that we added a separate title field for projects, make sure it's not empty
			var title = string.IsNullOrEmpty(element.Title) ? element.Id : element.Title;

			var model = new RampArchivingDlgViewModel(Application.ProductName, title, element.Id,
				element.ArchiveInfoDetails, element.SetFilesToArchive, element.GetFileDescription)
			{
				FileCopyOverride = FileCopySpecialHandler
			};
			model.AppSpecificFilenameNormalization = element.CustomFilenameNormalization;
			model.OverrideDisplayInitialSummary = fileLists => element.DisplayInitialArchiveSummary(fileLists, model);
			model.HandleNonFatalError = (exception, s) => ErrorReport.NotifyUserOfProblem(exception, s);

			element.InitializeModel(model);
			element.SetAdditionalMetsData(model);

			using (var dlg = new ArchivingDlg(model, ApplicationContainer.kSayMoreLocalizationId,
				Program.DialogFont, Settings.Default.ArchivingDialog))
			{
				dlg.ShowDialog();
				Settings.Default.ArchivingDialog = dlg.FormSettings;
			}
		}

		/// <remarks>SP-813: If project was moved, the stored IMDI path may not be valid, or not accessible</remarks>
		static internal bool CheckForAccessiblePath(string directory)
		{
			try
			{
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				var file = Path.Combine(directory, "Export.imdi");

				if (File.Exists(file)) File.Delete(file);

				File.WriteAllText(file, @"Export.imdi");

				if (File.Exists(file)) File.Delete(file);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		static internal bool IncludeFileInArchive(string path, Type typeOfArchive, string metadataFileExtension)
		{
			if (path == null) return false;
			var ext = Path.GetExtension(path).ToLower();
			bool imdi = typeof(IMDIArchivingDlgViewModel).IsAssignableFrom(typeOfArchive);
			return (ext != ".pfsx" && ext != ".sprj" && (!imdi || (ext != metadataFileExtension)));
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

		internal static void AddIMDISession(Session saymoreSession, ArchivingDlgViewModel model)
		{
			var sessionFile = saymoreSession.MetaDataFile;
			if (Project == null)
				Project = Program.CurrentProject;
			var analysisLanguage = GetAnalysisLanguageIdentifier(Project);

			// create IMDI session
			var imdiSession = model.AddSession(saymoreSession.Id);
			imdiSession.Title = saymoreSession.Title;

			// set its Project. (Depends on prior call to AddIMDIProject to populate the model's ArchivingPackage
			// with Project data.)
			imdiSession.AddProject(model.ArchivingPackage as ArchivingPackage);

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
			var sessionDateTime = DateTime.MinValue;
			if (!string.IsNullOrEmpty(stringVal))
			{
				sessionDateTime = DateTime.Parse(stringVal);
				imdiSession.SetDate(sessionDateTime.ToISO8601TimeFormatDateOnlyString());
			}

			// session languages
			if (Project != null)
			{
				var vernacularLanguage = ParseLanguage(ForceIso639ThreeChar(Project.VernacularISO3CodeAndName), null);
				if (vernacularLanguage != null)
					imdiSession.AddContentLanguage(vernacularLanguage, new LanguageString("Content Language", analysisLanguage));
			}
			imdiSession.AddContentLanguage(new ArchivingLanguage(analysisLanguage), new LanguageString("Working Language", analysisLanguage));

			// session situation
			stringVal = saymoreSession.MetaDataFile.GetStringValue("situation", null);
			if (!string.IsNullOrEmpty(stringVal))
				imdiSession.AddContentKeyValuePair("Situation", stringVal);

			imdiSession.Genre = GetFieldValue(sessionFile, "genre");
			imdiSession.SubGenre = GetFieldValue(sessionFile, "additional_Sub-Genre");
			imdiSession.AccessCode = GetFieldValue(sessionFile, "access");
			imdiSession.Interactivity = GetFieldValue(sessionFile, "additional_Interactivity");
			imdiSession.Involvement = GetFieldValue(sessionFile, "additional_Involvement");
			imdiSession.PlanningType = GetFieldValue(sessionFile, "additional_Planning_Type");
			imdiSession.SocialContext = GetFieldValue(sessionFile, "additional_Social_Context");
			imdiSession.Task = GetFieldValue(sessionFile, "additional_Task");

			imdiSession.AddContentDescription(new LanguageString { Value = GetFieldValue(sessionFile, "notes"), Iso3LanguageId = analysisLanguage });

			// custom session fields (the custom prefix is used internally)
			foreach (var item in saymoreSession.MetaDataFile.GetCustomFields())
			{
				var fieldId = item.FieldId.Substring(XmlFileSerializer.kCustomFieldIdPrefix.Length);
				if (fieldId == "ELAR Topic")
				{
					imdiSession.AddContentKeyValuePair(fieldId.Substring(5), item.ValueAsString);
				}
				else if (fieldId == "ELAR Keyword")
				{
					fieldId = fieldId.Substring(5);
					foreach (var kw in item.ValueAsString.Split(','))
					{
						imdiSession.AddContentKeyValuePair(fieldId, kw.Trim());
					}
				}
				else
				{
					imdiSession.AddContentKeyValuePair(fieldId, item.ValueAsString);
				}
			}

			// actors
			var actors = new ArchivingActorCollection();
			var persons = saymoreSession.GetAllPersonsInSession();
			var allParticipantsWithRoles = saymoreSession.MetaDataFile.MetaDataFieldValues.Count > 0?
				(from p in saymoreSession.MetaDataFile.GetStringValue(SessionFileType.kParticipantsFieldName, string.Empty).Split(FieldInstance.kDefaultMultiValueDelimiter)
											where p.Trim() != string.Empty
											select p.Trim()).ToList()
				: new List<string>();
			foreach (var person in persons)
			{

				var actor = InitializeActor(model, person, sessionDateTime, GetRole(person.Id, actors, allParticipantsWithRoles));

				// do this to get the ISO3 codes for the languages because they are not in saymore
				var language = GetOneLanguage(ForceIso639ThreeChar(person.MetaDataFile.GetStringValue("primaryLanguage", null)));
				if (language != null) actor.PrimaryLanguage = language;
				language = GetOneLanguage(ForceIso639ThreeChar(person.MetaDataFile.GetStringValue("mothersLanguage", null)));
				if (language != null) actor.MotherTongueLanguage = language;

				// otherLanguage0 - otherLanguage3
				for (var i = 0; i < 4; i++)
				{
					var languageKey = person.MetaDataFile.GetStringValue("otherLanguage" + i, null);
					if (string.IsNullOrEmpty(languageKey))
						continue;
					language = GetOneLanguage(ForceIso639ThreeChar(languageKey));
					if (language == null)
						continue;
					actor.Iso3Languages.Add(language);
				}

				// ethnic group
				var ethnicGroup = person.MetaDataFile.GetStringValue("ethnicGroup", null);
				if (ethnicGroup != null)
					actor.EthnicGroup = ethnicGroup;

				// custom person fields
				foreach (var item in person.MetaDataFile.GetCustomFields())
					actor.AddKeyValuePair(item.FieldId.Substring(XmlFileSerializer.kCustomFieldIdPrefix.Length), item.ValueAsString);

				// actor files
				var actorFiles = Directory.GetFiles(person.FolderPath)
					.Where(f => IncludeFileInArchive(f, typeof(IMDIArchivingDlgViewModel), Settings.Default.PersonFileExtension));
				foreach (var file in actorFiles)
					actor.Files.Add(CreateArchivingFile(file));

				// add actor to imdi session
				actors.Add(actor);
				imdiSession.AddActor(actor);

				// actor contact address
				var actorAddress = person.MetaDataFile.GetStringValue("howToContact", null);
				if (actorAddress != null)
				{
					imdiSession.AddActorContact(actor, new ArchivingContact
					{
						Address = actorAddress
					});
				}

				// Description (notes)
				var notes = (from metaDataFieldValue in person.MetaDataFile.MetaDataFieldValues
					where metaDataFieldValue.FieldId == "notes"
					select metaDataFieldValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(notes))
					imdiSession.AddActorDescription(actor, new LanguageString {Value = notes, Iso3LanguageId = analysisLanguage});
			}

			// get contributors
			foreach (var contributor in saymoreSession.GetAllContributorsInSession())
			{
				var actr = actors.FirstOrDefault(a => a.Name == contributor.Name);
				if (actr != null)
					continue;
				var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PersonNotParticipating",
					"{0} is listed as a contributor but not a participant in {1} session.");
				model.AdditionalMessages[string.Format(msg, contributor.Name, saymoreSession.Id)] =
					ArchivingDlgViewModel.MessageType.Error;
				imdiSession.AddActor(contributor);
			}

			// session files
			var files = saymoreSession.GetSessionFilesToArchive(model.GetType());
			foreach (var file in files)
			{
				if (file.EndsWith(Settings.Default.MetadataFileExtension, StringComparison.InvariantCulture))
					continue;
				imdiSession.AddFile(CreateArchivingFile(file));
				var info = saymoreSession.GetComponentFiles()
					.FirstOrDefault(componentFile => componentFile.PathToAnnotatedFile == file);
				if (info == null)
					continue;
				var notes = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "notes"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(notes))
					imdiSession.AddFileDescription(file, new LanguageString { Value = notes, Iso3LanguageId = analysisLanguage });
				if (!info.FileType.IsAudioOrVideo)
					continue;
				var duration = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Duration"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(duration))
					imdiSession.AddMediaFileTimes(file, "00:00:00", duration);
				var device = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Device"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(device))
					imdiSession.AddFileKeyValuePair(file, "RecordingEquipment", device);
				var microphone = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Microphone"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(microphone))
					imdiSession.AddFileKeyValuePair(file, "RecordingEquipment", microphone);
			}
		}

		private static string GetAnalysisLanguageIdentifier(Project saymoreProject)
		{
			var analysisLanguage = saymoreProject?.AnalysisISO3CodeAndName;
			if (string.IsNullOrEmpty(analysisLanguage))
				analysisLanguage = AnalysisLanguage();
			if (analysisLanguage.Contains(":"))
				analysisLanguage = analysisLanguage.Split(':')[0];
			// In case of things like pt-PT; no-op if no hyphen
			analysisLanguage = analysisLanguage.Split('-')[0];
			analysisLanguage = ForceIso639ThreeChar(analysisLanguage);
			return analysisLanguage;
		}

		private static string GetRole(string personId, ArchivingActorCollection actors, System.Collections.Generic.IEnumerable<string> allParticipantsWithRoles)
		{
			return (from p in allParticipantsWithRoles
				where p.StartsWith(personId + " (")
				let openParen = p.IndexOf("(", StringComparison.InvariantCulture)
				let closeParen = p.IndexOf(")", StringComparison.InvariantCulture)
				select p.Substring(openParen + 1, closeParen - openParen - 1))
				.FirstOrDefault(role => !PersonHasRole(personId, actors, role));
		}

		private static bool PersonHasRole(string personId, ArchivingActorCollection actors, string role)
		{
			if (actors.Count == 0)
				return false;
			foreach (var a in actors)
			{
				if (a.FullName == personId && a.Role == role)
					return true;
			}

			return false;
		}

		private static string ForceIso639ThreeChar(string analysisLanguage)
		{
			if (analysisLanguage?.Length == 2)
				analysisLanguage = _LanguageLookup.GetLanguageFromCode(analysisLanguage).ThreeLetterTag;
			return analysisLanguage;
		}

		internal static string AnalysisLanguage()
		{
			var analysisLanguage = ForceIso639ThreeChar(Settings.Default.UserInterfaceLanguage);
			return $@"{analysisLanguage}: {_LanguageLookup.GetLanguageFromCode(analysisLanguage).DesiredName}";
		}

		internal static ArchivingLanguage GetOneLanguage(string languageKey)
		{
			ArchivingLanguage returnValue = null;
			var language = LanguageList.FindByEnglishName(languageKey);
			if (language == null || language.Iso3Code == "und")
				if (!string.IsNullOrEmpty(languageKey) && languageKey.Length == 3)
					language = LanguageList.FindByISO3Code(languageKey);
			if (language != null && language.Iso3Code != "und" && !string.IsNullOrEmpty(language.EnglishName))
				returnValue = new ArchivingLanguage(ForceIso639ThreeChar(language.Iso3Code), language.Definition)
				{
					EnglishName = language.EnglishName
				};
		return returnValue;
		}

		internal static ArchivingActor InitializeActor(ArchivingDlgViewModel model, Person person, DateTime sessionDateTime, string role)
		{
			// is this person protected
			var protect = bool.Parse(person.MetaDataFile.GetStringValue("privacyProtection", "false"));

			// display message if the birth year is not valid
			var birthYear = person.MetaDataFile.GetStringValue("birthYear", string.Empty).Trim();
			var age = 0;
			if (!birthYear.IsValidBirthYear() || string.IsNullOrEmpty(birthYear))
			{
				var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.InvalidBirthYearMsg",
					"The Birth Year for {0} should be a 4 digit number. It is used to calculate the age for the IMDI export.");
				model.AdditionalMessages[string.Format(msg, person.Id)] = ArchivingDlgViewModel.MessageType.Warning;
			}
			else
			{
				age = string.IsNullOrEmpty(birthYear) ? 0 : sessionDateTime.Year - int.Parse(birthYear);
				if (age < 2 || age > 130)
				{
					var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.InvalidAgeMsg",
						"The age for {0} must be between 2 and 130");
					model.AdditionalMessages[string.Format(msg, person.Id)] = ArchivingDlgViewModel.MessageType.Warning;
				}
			}

			var actor = new ArchivingActor
			{
				FullName = person.Id,
				Name = person.MetaDataFile.GetStringValue(PersonFileType.kCode, person.Id),
				Code = person.MetaDataFile.GetStringValue(PersonFileType.kCode, null),
				BirthDate = birthYear,
				Age = age > 0 ? age.ToString() : "Unspecified",
				Gender = person.MetaDataFile.GetStringValue(PersonFileType.kGender, null),
				Education = person.MetaDataFile.GetStringValue(PersonFileType.kEducation, null),
				Occupation = person.MetaDataFile.GetStringValue(PersonFileType.kPrimaryOccupation, null),
				Anonymize = protect,
				Role = role,
			};
			return actor;
		}

		private static string GetFieldValue(ComponentFile file, string valueName)
		{
			var stringVal = file.GetStringValue(valueName, null);
			return string.IsNullOrEmpty(stringVal) ? null : stringVal;
		}

		internal static void AddIMDIProjectData(Project saymoreProject, ArchivingDlgViewModel model)
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
			package.AddDescription(new LanguageString(saymoreProject.ProjectDescription, GetAnalysisLanguageIdentifier(saymoreProject)));

			// content type
			package.ContentType = null;

			// funding project
			package.FundingProject = new ArchivingProject
			{
				Title = saymoreProject.FundingProjectTitle,
				Name = saymoreProject.FundingProjectTitle
			};

			// author
			package.Author = saymoreProject.ContactPerson;

			// applications
			package.Applications = null;

			// access date
			package.Access.DateAvailable = saymoreProject.DateAvailable;

			// access owner
			package.Access.Owner = saymoreProject.RightsHolder;

			// publisher
			package.Publisher = saymoreProject.Depositor;

			// subject language
			if (!string.IsNullOrEmpty(saymoreProject.VernacularISO3CodeAndName))
			{
				ParseLanguage(saymoreProject.VernacularISO3CodeAndName, package);
			}

			// analysis language
			if (!string.IsNullOrEmpty(saymoreProject.AnalysisISO3CodeAndName))
			{
				ParseLanguage(saymoreProject.AnalysisISO3CodeAndName, package);
			}

			// project description documents
			var docsPath = Path.Combine(saymoreProject.FolderPath, ProjectDescriptionDocsScreen.kFolderName);
			if (Directory.Exists(docsPath))
			{
				var files = Directory.GetFiles(docsPath, "*.*", SearchOption.TopDirectoryOnly);

				// the directory exists and contains files
				if (files.Length > 0)
					AddDocumentsSession(ProjectDescriptionDocsScreen.kArchiveSessionName, files, model);
			}

			// other project documents
			docsPath = Path.Combine(saymoreProject.FolderPath, ProjectOtherDocsScreen.kFolderName);
			if (Directory.Exists(docsPath))
			{
				var files = Directory.GetFiles(docsPath, "*.*", SearchOption.TopDirectoryOnly);

				// the directory exists and contains files
				if (files.Length > 0)
					AddDocumentsSession(ProjectOtherDocsScreen.kArchiveSessionName, files, model);
			}
		}

		private static ArchivingLanguage ParseLanguage(string languageDesignator, IArchivingPackage package)
		{
			ArchivingLanguage archivingLanguage = null;
			var parts = languageDesignator.SplitTrimmed(':').ToArray();
			if (parts.Length == 2)
			{
				var language = LanguageList.FindByISO3Code(parts[0]);

				// SP-765:  Allow codes from Ethnologue that are not in the Arbil list
				archivingLanguage = string.IsNullOrEmpty(language?.EnglishName)
					? new ArchivingLanguage(ForceIso639ThreeChar(parts[0]), parts[1], parts[1])
					: new ArchivingLanguage(ForceIso639ThreeChar(language.Iso3Code), parts[1], language.EnglishName);
				package?.MetadataIso3Languages.Add(archivingLanguage);
			}
			else if (parts.Length == 1)
			{
				var language = LanguageList.FindByISO3Code(parts[0]);
				if (!string.IsNullOrEmpty(language?.EnglishName))
				{
					archivingLanguage = new ArchivingLanguage(ForceIso639ThreeChar(language.Iso3Code), parts[1], language.EnglishName);
					package?.MetadataIso3Languages.Add(archivingLanguage);
				}
			}

			return archivingLanguage;
		}

		private static ArchivingFile CreateArchivingFile(string fileName)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;
			var metaFileSuffix = Settings.Default.MetadataFileExtension;

			var arcFile = new ArchivingFile(fileName);

			// is this an annotation file?
			if (fileName.EndsWith(annotationSuffix))
				arcFile.DescribesAnotherFile = fileName.Substring(0, fileName.Length - annotationSuffix.Length);

			// is this a meta file?
			if (fileName.EndsWith(metaFileSuffix))
				arcFile.DescribesAnotherFile = fileName.Substring(0, fileName.Length - metaFileSuffix.Length);

			return arcFile;
		}

		private static void AddDocumentsSession(string sessionName, string[] sourceFiles, ArchivingDlgViewModel model)
		{
			// create IMDI session
			var imdiSession = model.AddSession(sessionName);
			imdiSession.Title = sessionName;

			foreach (var file in sourceFiles)
				imdiSession.AddFile(CreateArchivingFile(file));
		}
	}
}
