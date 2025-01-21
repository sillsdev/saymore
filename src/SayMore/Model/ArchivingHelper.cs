using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
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
using SayMore.UI;
using SIL.Archiving.IMDI.Lists;
using SIL.Windows.Forms.ClearShare;
using SIL.WritingSystems;
using static System.String;
using static SayMore.Model.Files.PersonFileType;
using static SayMore.Model.LanguageHelper;
using static SayMore.Model.MetadataMigrator.Result;
using Application = System.Windows.Forms.Application;

namespace SayMore.Model
{
	static class ArchivingHelper
	{
		internal static Project Project = null; // Set for testing

		/// ------------------------------------------------------------------------------------
		internal static void ArchiveUsingIMDI(IIMDIArchivable element, Form parentForm)
		{
			const string kVietnam = "Vietnam";
			if (Project == null)
				Project = element as Project ?? Program.CurrentProject;
			var isInVietnam = Project.Country == kVietnam ||
				(element is Session sElem &&
					sElem.MetaDataFile.GetStringValue("additional_Location_Country", null) == kVietnam);
			
			// Do language migration if needed
			MetadataMigrator migrator = null;
			foreach (var session in GetAllSessionsToBeArchived(element))
			{
				foreach (var person in session.GetAllPersonsInSession())
				{
					if (migrator == null)
						migrator = new MetadataMigrator(() => new InteractiveLanguageDisambiguator(parentForm), isInVietnam);
					switch (migrator.MigrateAmbiguousLanguages(person.MetaDataFile))
					{
						case Cancelled:
							return;
						case Migrated:
							person.Save();
							break;
					}
				}
			}

			var destFolder = Program.CurrentProject.IMDIOutputDirectory;

			// Move IMDI export folder to be under the mydocs/saymore
			if (IsNullOrEmpty(destFolder))
				destFolder = Path.Combine(NewProjectDlgViewModel.ParentFolderPathForNewProject, "IMDI Packages");

			// SP-813: If project was moved, the stored IMDI path may not be valid, or not accessible
			if (!CheckForAccessiblePath(destFolder))
			{
				destFolder = Path.Combine(NewProjectDlgViewModel.ParentFolderPathForNewProject, "IMDI Packages");
			}

			// now that we added a separate title field for projects, make sure it's not empty
			var title = IsNullOrEmpty(element.Title) ? element.Id : element.Title;

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
			var title = IsNullOrEmpty(element.Title) ? element.Id : element.Title;

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
		internal static bool CheckForAccessiblePath(string directory)
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
		internal static bool FileCopySpecialHandler(ArchivingDlgViewModel model, string source, string dest)
		{
			if (!source.EndsWith(AnnotationFileHelper.kAnnotationsEafFileSuffix))
				return false;

			// Fix EAF file to refer to modified name.
			AnnotationFileHelper annotationFileHelper = AnnotationFileHelper.Load(source);

			var mediaFileName = annotationFileHelper.MediaFileName;
			if (mediaFileName != null)
			{
				var normalizedName = model.NormalizeFilename(Empty, mediaFileName);
				if (normalizedName != mediaFileName)
				{
					annotationFileHelper.SetMediaFile(normalizedName);
					annotationFileHelper.Root.Save(dest);
					return true;
				}
			}
			return false;
		}

		internal static IEnumerable<Session> GetAllSessionsToBeArchived(IIMDIArchivable element)
		{
			if (element is Project project)
			{
				foreach (var session in project.GetAllSessions())
					yield return session;
			}
			else
				yield return (Session)element;
		}

		internal static void SetIMDIMetadataToArchive(IIMDIArchivable element, ArchivingDlgViewModel model)
		{
			if (element is Project project)
				AddIMDIProjectData(project, model);
			
			foreach (var session in GetAllSessionsToBeArchived(element))
				AddIMDISession(session, model);
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

			// set its Project, if we can. (Depends on prior call to AddIMDIProject to populate the model's ArchivingPackage
			// with Project data. If that didn't happen, e.g., when exporting just a session, we don't have access to
			// any project data, so just leave it out.)
			if (model.ArchivingPackage?.FundingProject != null)
			{
				imdiSession.AddProject(model.ArchivingPackage as ArchivingPackage);
			}

			// session location
			var address = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Address", null);
			var region = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Region", null);
			var country = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Country", null);
			var continent = saymoreSession.MetaDataFile.GetStringValue("additional_Location_Continent", null);
			if (IsNullOrEmpty(address))
				address = saymoreSession.MetaDataFile.GetStringValue("location", null);

			imdiSession.Location = new ArchivingLocation { Address = address, Region = region, Country = country, Continent = continent };

			// session description (synopsis)
			var stringVal = saymoreSession.MetaDataFile.GetStringValue("synopsis", null);
			if (!IsNullOrEmpty(stringVal))
				imdiSession.AddDescription(new LanguageString { Value = stringVal });

			// session date
			stringVal = saymoreSession.MetaDataFile.GetStringValue("date", null);
			var sessionDateTime = DateTime.MinValue;
			if (!IsNullOrEmpty(stringVal))
			{
				sessionDateTime = DateTime.Parse(stringVal);
				imdiSession.SetDate(sessionDateTime.ToISO8601TimeFormatDateOnlyString());
			}

			// session languages
			if (Project != null)
			{
				var vernacularLanguage = ParseLanguage(
					Project.VernacularISO3CodeAndName.GetIso639ThreeCharCode(), null);
				if (vernacularLanguage != null)
					imdiSession.AddContentLanguage(vernacularLanguage, new LanguageString("Content Language", analysisLanguage));
			}
			imdiSession.AddContentLanguage(new ArchivingLanguage(analysisLanguage), new LanguageString("Working Language", analysisLanguage));

			// session situation
			stringVal = saymoreSession.MetaDataFile.GetStringValue("situation", null);
			if (!IsNullOrEmpty(stringVal))
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
			var contributions =
				saymoreSession.MetaDataFile.GetValue(SessionFileType.kContributionsFieldName, null) as ContributionCollection;

			foreach (var person in persons)
			{

				var actor = InitializeActor(model, person, sessionDateTime, GetRole(person.Id, actors, contributions));

				// Do this to get the ISO3 codes for the languages because they are not in SayMore
				var language = GetOneLanguage(person.MetaDataFile
					.GetStringValue(kPrimaryLanguage, null).GetIso639ThreeCharCode());
				if (language != null)
					actor.PrimaryLanguage = language;
				language = GetOneLanguage(person.MetaDataFile
					.GetStringValue("mothersLanguage", null).GetIso639ThreeCharCode());
				if (language != null) 
					actor.MotherTongueLanguage = language;

				// otherLanguage0 - otherLanguage3
				for (var i = 0; i < 4; i++)
				{
					var languageKey = person.MetaDataFile.GetStringValue("otherLanguage" + i, null);
					if (IsNullOrEmpty(languageKey))
						continue;
					language = GetOneLanguage(languageKey.GetIso639ThreeCharCode());
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
				if (!IsNullOrEmpty(notes))
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
				model.AdditionalMessages[Format(msg, contributor.Name, saymoreSession.Id)] =
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
				if (!IsNullOrEmpty(notes))
					imdiSession.AddFileDescription(file, new LanguageString { Value = notes, Iso3LanguageId = analysisLanguage });
				if (!info.FileType.IsAudioOrVideo)
					continue;
				var duration = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Duration"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!IsNullOrEmpty(duration))
					imdiSession.AddMediaFileTimes(file, "00:00:00", duration);
				var device = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Device"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!IsNullOrEmpty(device))
					imdiSession.AddFileKeyValuePair(file, "RecordingEquipment", device);
				var microphone = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Microphone"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!IsNullOrEmpty(microphone))
					imdiSession.AddFileKeyValuePair(file, "RecordingEquipment", microphone);
			}
		}

		private static string GetAnalysisLanguageIdentifier(Project saymoreProject)
		{
			var analysisLanguage = saymoreProject?.AnalysisISO3CodeAndName;
			if (IsNullOrEmpty(analysisLanguage))
				analysisLanguage = FallbackAnalysisLanguage;
			analysisLanguage = analysisLanguage.SplitOnColon()[0];
			analysisLanguage = analysisLanguage.GetIso639ThreeCharCode();
			return analysisLanguage;
		}

		private static string GetRole(string personId, ArchivingActorCollection actors, ContributionCollection contributions)
		{
			return contributions?.Where(c => c.ContributorName == personId).Select(c => c.Role).FirstOrDefault()?.Name;
		}

		internal static ArchivingLanguage GetOneLanguage(string languageStr)
		{
			if (languageStr == null)
				return null;

			var parts = languageStr.SplitOnColon();
			if (TryGetArchivingLanguageFromCode(parts[0], out var result))
				return result;

			languageStr = parts.Length > 1 ? parts[1] : parts[0];
			var language = LanguageList.FindByEnglishName(languageStr);

			if (language != null && language.Iso3Code != "und" && !IsNullOrEmpty(language.EnglishName))
				return new ArchivingLanguage(language.Iso3Code.GetIso639ThreeCharCode(),
					language.Definition, language.EnglishName);
			
			if (parts.Length > 1 && TryGetArchivingLanguageFromCode(languageStr, out result))
				return result;

			return null;
		}

		private static bool TryGetArchivingLanguageFromCode(string languageStr, out ArchivingLanguage archivingLanguage)
		{
			archivingLanguage = null;

			if (languageStr == "und")
				return false;

			IetfLanguageTag.TryGetParts(languageStr, out var code, out _, out _, out _);
			if (code != null)
			{
				var langInfo = _LanguageLookup.GetLanguageFromCode(code);
				if (langInfo != null)
				{
					var language = LanguageList.FindByISO3Code(langInfo.ThreeLetterTag);
					archivingLanguage = new ArchivingLanguage(langInfo.ThreeLetterTag, langInfo.DesiredName,
						language?.EnglishName ?? langInfo.Names.FirstOrDefault());
					return true;
				}
			}

			// I think in most all cases, *if languageStr is a valid code*, the above logic will
			// have gotten the language. However, there is at least one case where it doesn't. 
			// ISO-639-2 defines "eng" as the 3-letter code for English (en), but LanguageLookup
			// doesn't include that. The following logic will find it.
			if (languageStr.Length == 3)
			{
				var language = LanguageList.FindByISO3Code(languageStr);
				if (language != null && language.Iso3Code != "und" && !IsNullOrEmpty(language.EnglishName))
				{
					archivingLanguage = new ArchivingLanguage(languageStr, language.DisplayName,
						language.EnglishName);
					return true;
				}
			}
			return false;
		}

		internal static string FallbackAnalysisLanguage
		{
			get
			{
				var language = Settings.Default.UserInterfaceLanguage.GetIso639ThreeCharCode();
				return $@"{language}: {_LanguageLookup.GetLanguageFromCode(language).DesiredName}";
			}
		}

		internal static ArchivingActor InitializeActor(ArchivingDlgViewModel model, Person person, DateTime sessionDateTime, string role)
		{
			// is this person protected
			var protect = bool.Parse(person.MetaDataFile.GetStringValue("privacyProtection", "false"));

			// display message if the birth year is not valid
			var birthYear = person.MetaDataFile.GetStringValue("birthYear", Empty).Trim();
			var age = 0;
			if (!birthYear.IsValidBirthYear() || IsNullOrEmpty(birthYear))
			{
				var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.InvalidBirthYearMsg",
					"The Birth Year for {0} should be a 4 digit number. It is used to calculate the age for the IMDI export.");
				model.AdditionalMessages[Format(msg, person.Id)] = ArchivingDlgViewModel.MessageType.Warning;
			}
			else
			{
				age = IsNullOrEmpty(birthYear) ? 0 : sessionDateTime.Year - int.Parse(birthYear);
				if (age < 2 || age > 130)
				{
					var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.InvalidAgeMsg",
						"The age for {0} must be between 2 and 130");
					model.AdditionalMessages[Format(msg, person.Id)] = ArchivingDlgViewModel.MessageType.Warning;
				}
			}

			var actor = new ArchivingActor
			{
				FullName = person.Id,
				Name = person.MetaDataFile.GetStringValue(kCode, person.Id),
				Code = person.MetaDataFile.GetStringValue(kCode, null),
				BirthDate = birthYear,
				Age = age > 0 ? age.ToString() : "Unspecified",
				Gender = person.MetaDataFile.GetStringValue(kGender, null),
				Education = person.MetaDataFile.GetStringValue(kEducation, null),
				Occupation = person.MetaDataFile.GetStringValue(kPrimaryOccupation, null),
				Anonymize = protect,
				Role = role,
			};
			return actor;
		}

		private static string GetFieldValue(ComponentFile file, string valueName)
		{
			var stringVal = file.GetStringValue(valueName, null);
			return IsNullOrEmpty(stringVal) ? null : stringVal;
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
			if (!IsNullOrEmpty(saymoreProject.VernacularISO3CodeAndName))
			{
				ParseLanguage(saymoreProject.VernacularISO3CodeAndName, package);
			}

			// analysis language
			if (!IsNullOrEmpty(saymoreProject.AnalysisISO3CodeAndName))
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
			var parts = GetParts(languageDesignator);
			if (parts.Count == 2)
			{
				var language = LanguageList.FindByISO3Code(parts[0]);

				// SP-765:  Allow codes from Ethnologue that are not in the Arbil list
				archivingLanguage = IsNullOrEmpty(language?.EnglishName)
					? new ArchivingLanguage(parts[0].GetIso639ThreeCharCode(), parts[1], parts[1])
					: new ArchivingLanguage(language.Iso3Code.GetIso639ThreeCharCode(), parts[1], language.EnglishName);
				package?.MetadataIso3Languages.Add(archivingLanguage);
			}
			else if (parts.Count == 1)
			{
				var language = LanguageList.FindByISO3Code(parts[0]);
				if (!IsNullOrEmpty(language?.EnglishName))
				{
					archivingLanguage = new ArchivingLanguage(language.Iso3Code.GetIso639ThreeCharCode(), parts[1], language.EnglishName);
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
