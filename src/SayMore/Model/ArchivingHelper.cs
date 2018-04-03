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
using SIL.Archiving.IMDI.Lists;
using SIL.WritingSystems;
using Application = System.Windows.Forms.Application;

namespace SayMore.Model
{
	static class ArchivingHelper
	{
		internal static IMDIPackage _Package;
		internal static ArchivingLanguage _defaultLanguage;
		internal static LanguageLookup _LanguageLookup = new LanguageLookup();

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
			var analysisLanguage = AnalysisLanguage();
			var analysisLanguageId = LanguageList.FindByISO3Code(analysisLanguage).Id;

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

			// session project
			if (_Package != null)
			{
				imdiSession.AddProject(_Package);
			}

			// session description (synopsis)
			var stringVal = saymoreSession.MetaDataFile.GetStringValue("synopsis", null);
			if (!string.IsNullOrEmpty(stringVal))
				imdiSession.AddDescription(new LanguageString { Value = stringVal, Iso3LanguageId = analysisLanguage});

			// session date
			stringVal = saymoreSession.MetaDataFile.GetStringValue("date", null);
			var sessionDateTime = DateTime.MinValue;
			if (!string.IsNullOrEmpty(stringVal))
			{
				sessionDateTime = DateTime.Parse(stringVal);
				imdiSession.SetDate(sessionDateTime.ToISO8601TimeFormatDateOnlyString());
			}

			// session languages
			if (_defaultLanguage != null)
				imdiSession.AddContentLanguage(_defaultLanguage, new LanguageString("Content Language", analysisLanguage));
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

			imdiSession.AddContentDescription(new LanguageString{Value = GetFieldValue(sessionFile, "notes"), Iso3LanguageId = analysisLanguage});

			// custom session fields
			foreach (var item in saymoreSession.MetaDataFile.GetCustomFields())
				imdiSession.AddContentKeyValuePair(item.FieldId.Substring(XmlFileSerializer.kCustomFieldIdPrefix.Length), item.ValueAsString);

			// actors
			var actors = new ArchivingActorCollection();
			var persons = saymoreSession.GetAllPersonsInSession();
			foreach (var person in persons)
			{
				var actor = InitializeActor(model, person, saymoreSession, sessionDateTime);

				// do this to get the ISO3 codes for the languages because they are not in saymore
				var language = GetOneLanguage(person.MetaDataFile.GetStringValue("primaryLanguage", null));
				if (language != null) actor.PrimaryLanguage = language;
				language = GetOneLanguage(person.MetaDataFile.GetStringValue("mothersLanguage", null));
				if (language != null) actor.MotherTongueLanguage = language;

				// otherLanguage0 - otherLanguage3
				for (var i = 0; i < 4; i++)
				{
					var languageKey = person.MetaDataFile.GetStringValue("otherLanguage" + i, null);
					if (string.IsNullOrEmpty(languageKey)) continue;
					language = GetOneLanguage(languageKey);
					if (language == null) continue;
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
				var notes = (from metaDataFieldValue in person.MetaDataFile.MetaDataFieldValues where metaDataFieldValue.FieldId == "notes" select metaDataFieldValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(notes))
					imdiSession.AddActorDescription(actor, new LanguageString { Value = notes, Iso3LanguageId = analysisLanguage });
			}

			// get contributors
			foreach (var contributor in saymoreSession.GetAllContributorsInSession())
			{
				var actr = actors.FirstOrDefault(a => a.Name == contributor.Name);
				if (actr != null) continue;
				var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PersonNotParticipating",
					"{0} is listed as a contributor but not a participant in {1} session.");
				model.AdditionalMessages[string.Format(msg, contributor.Name, saymoreSession.Id)] = ArchivingDlgViewModel.MessageType.Error;
				imdiSession.AddActor(contributor);
			}

			// session files
			var files = saymoreSession.GetSessionFilesToArchive(model.GetType());
			foreach (var file in files)
			{
				if (file.ToUpper().EndsWith(".MOV", StringComparison.InvariantCulture))
				{
					var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.MovFileIncluded",
						"MOV file contained in {0} session.");
					model.AdditionalMessages[string.Format(msg, saymoreSession.Id)] = ArchivingDlgViewModel.MessageType.Error;
				}
				if (file.EndsWith(Settings.Default.MetadataFileExtension, StringComparison.InvariantCulture)) continue;
				imdiSession.AddFile(CreateArchivingFile(file));
				var info = saymoreSession.GetComponentFiles().FirstOrDefault(componentFile => componentFile.PathToAnnotatedFile == file);
				if (info == null) continue;
				if (_Package != null)
				{
					var conditions = (from infoValue in info.MetaDataFieldValues
						where infoValue.FieldId.ToLower().Contains("conditions_of_access")
						select infoValue.ValueAsString).FirstOrDefault();
					var restrictions = (from infoValue in info.MetaDataFieldValues
						where infoValue.FieldId.ToLower().Contains("restrictions")
						select infoValue.ValueAsString).FirstOrDefault();
					imdiSession.AddFileAccess(file, _Package, new LanguageString{Value = conditions, Iso3LanguageId = analysisLanguageId}, new LanguageString{Value = restrictions, Iso3LanguageId = analysisLanguageId});
				}
				var status = GetFieldValue(sessionFile, "status") == "Finished" ? "Stable" : "In Progress";
				imdiSession.AddFileKeyValuePair(file, "Status", status);
				var notes = (from infoValue in info.MetaDataFieldValues
						where infoValue.FieldId == "notes"
						select infoValue.ValueAsString).FirstOrDefault();
				var additionalInfo = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId.ToLower().Contains("additionalinformationobject")
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(additionalInfo))
					imdiSession.AddFileKeyValuePair(file, "AdditionalInformationObject", additionalInfo);
				if (!string.IsNullOrEmpty(notes))
					imdiSession.AddFileDescription(file, new LanguageString{Value = notes, Iso3LanguageId = analysisLanguage});
				if (!info.FileType.IsAudioOrVideo) continue;
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
				var duration = (from infoValue in info.MetaDataFieldValues
					where infoValue.FieldId == "Duration"
					select infoValue.ValueAsString).FirstOrDefault();
				if (!string.IsNullOrEmpty(duration))
					imdiSession.AddMediaFileTimes(file, "00:00:00", duration);
			}
		}

		private static string AnalysisLanguage()
		{
			var analysisLanguage = Settings.Default.UserInterfaceLanguage;
			return analysisLanguage.Length != 2 ? analysisLanguage : _LanguageLookup.GetLanguageFromCode(analysisLanguage).ThreeLetterTag;
		}

		internal static ArchivingLanguage GetOneLanguage(string languageKey)
		{
			ArchivingLanguage returnValue = null;
			var language = LanguageList.FindByEnglishName(languageKey);
			if (language == null || language.Iso3Code == "und")
				if (!string.IsNullOrEmpty(languageKey) && languageKey.Length == 3)
					language = LanguageList.FindByISO3Code(languageKey);
			if (language != null && language.Iso3Code != "und" && !string.IsNullOrEmpty(language.EnglishName))
				returnValue = new ArchivingLanguage(language.Iso3Code, language.Definition)
				{
					EnglishName = language.EnglishName
				};
			else if (_defaultLanguage != null)
			{
				returnValue = new ArchivingLanguage(_defaultLanguage.Iso3Code, _defaultLanguage.LanguageName)
				{
					EnglishName = _defaultLanguage.EnglishName
				};
			}
			return returnValue;
		}

		internal static ArchivingActor InitializeActor(ArchivingDlgViewModel model, Person person, Session saymoreSession, DateTime sessionDateTime)
		{
			// is this person protected
			var protect = bool.Parse(person.MetaDataFile.GetStringValue("privacyProtection", "false"));

			// display message if the birth year is not valid
			var birthYear = person.MetaDataFile.GetStringValue("birthYear", string.Empty).Trim();
			int age = 0;
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

			var roles = (from archivingActor in saymoreSession.GetAllContributorsInSession()
				where archivingActor.Name == person.Id
				select archivingActor.Role).ToList();
			if (roles.Count == 0)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PersonNotParticipating", "Participant {0} has no role in {1} session.");
				model.AdditionalMessages[string.Format(msg, person.Id, saymoreSession.Id)] = ArchivingDlgViewModel.MessageType.Error;
			}

			var role = string.Join(", ", new SortedSet<string>(roles));
			role = string.IsNullOrEmpty(role) ? "Participant" : role;

			var actor = new ArchivingActor
			{
				FullName = person.Id,
				Name = person.MetaDataFile.GetStringValue(PersonFileType.kCode, person.Id),
				Code = person.MetaDataFile.GetStringValue(PersonFileType.kCode, null),
				BirthDate = birthYear,
				Age = age.ToString(),
				Gender = person.MetaDataFile.GetStringValue(PersonFileType.kGender, null),
				Education = person.MetaDataFile.GetStringValue(PersonFileType.kEducation, null),
				Occupation = person.MetaDataFile.GetStringValue(PersonFileType.kPrimaryOccupation, null),
				Anonymize = protect,
				Role = role
			};
			return actor;
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
				var parts = saymoreProject.VernacularISO3CodeAndName.SplitTrimmed(':').ToArray();
				if (parts.Length == 2)
				{
					var language = LanguageList.FindByISO3Code(parts[0]);

					// SP-765:  Allow codes from Ethnologue that are not in the Arbil list
					if (string.IsNullOrEmpty(language?.EnglishName))
						_defaultLanguage = new ArchivingLanguage(parts[0], parts[1], parts[1]);
					else
						_defaultLanguage = new ArchivingLanguage(language.Iso3Code, parts[1], language.EnglishName);
					package.ContentIso3Languages.Add(_defaultLanguage);
				}
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

			_Package = package;
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
