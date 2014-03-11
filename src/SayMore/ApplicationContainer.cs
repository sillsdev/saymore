using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using L10NSharp;
using Palaso.IO;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI;
using SayMore.UI.ProjectChoosingAndCreating;
using SayMore.Utilities;

namespace SayMore
{
	/// <summary>
	/// This is sortof a wrapper around the DI container. I'm not thrilled with the name I've
	/// used (jh).
	/// </summary>
	public class ApplicationContainer : IDisposable
	{
		private IContainer _container;
		private ISplashScreen _splashScreen;
		public const string kSayMoreLocalizationId = "SayMore";

		/// ------------------------------------------------------------------------------------
		public ApplicationContainer() : this(false)
		{
		}

		/// ------------------------------------------------------------------------------------
		public ApplicationContainer(bool showSplashScreen)
		{
			if (showSplashScreen)
				ShowSplashScreen();

			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

			//this one overrides the above, for commands, so that anything requiring
			//IEnumerable<ICommand> will get a list of *instances*, one each, of each command.
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				.As<ICommand>()
				.Where(t => t.GetInterfaces().Contains(typeof(ICommand))).InstancePerLifetimeScope();

			builder.RegisterInstance<LocalizationManager>(CreateLocalizationManager()).SingleInstance();


			//			var filesTypes = GetFilesTypes(parentContainer);
//			builder.RegisterInstance(filesTypes).As(typeof(IEnumerable<FileType>));

			// When something needs the list of filetypes, get them from this method
//			builder.Register<IEnumerable<FileType>>(c => GetFilesTypes(c));
			builder.RegisterInstance(ComponentRoles).As(typeof(IEnumerable<ComponentRole>));

			// When something needs a list of XML field serializers, get them from this method
			builder.RegisterInstance(XmlFieldSerializers).As(typeof(IDictionary<string, IXmlFieldSerializer>));

			_container = builder.Build();
		}

		/// ------------------------------------------------------------------------------------
		public static string GetVersionInfo(string fmt)
		{
			var asm = Assembly.GetExecutingAssembly();
			var ver = asm.GetName().Version;
			var file = asm.CodeBase.Replace("file:", string.Empty);
			file = file.TrimStart('/');
			var fi = new FileInfo(file);

			return string.Format(fmt, ver.Major, ver.Minor,
				ver.Build, fi.LastWriteTime.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
		public void ShowSplashScreen()
		{
			if (_splashScreen == null)
			{
				_splashScreen = new UI.SplashScreen();
				_splashScreen.ShowWithoutFade();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void CloseSplashScreen()
		{
			if (_splashScreen != null)
				_splashScreen.Close();

			_splashScreen = null;
		}

		/// ------------------------------------------------------------------------------------
		public void SetProjectNameOnSplashScreen(string projectPath)
		{
			if (_splashScreen != null)
				_splashScreen.Message = Path.GetFileNameWithoutExtension(projectPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Someday, we may put this under user control. For now, they are hard-coded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<ComponentRole> ComponentRoles
		{
			get
			{
				yield return new
					ComponentRole(typeof(Session), ComponentRole.kSourceComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.SourceRecording", "Source Recording"),
						ComponentRole.MeasurementTypes.Time,
						FileSystemUtils.GetIsAudioVideo, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Source",
						Settings.Default.WorkflowStageColor1,
						Settings.Default.WorkflowStageTextColor1);//todo... but maybe we dont' show this as a stage?

				yield return
					new ComponentRole(typeof(ProjectElement), ComponentRole.kConsentComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.InformedConsent", "Informed Consent"),
						ComponentRole.MeasurementTypes.None, (p => true), ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Consent",
						Settings.Default.WorkflowStageColor2,
						Settings.Default.WorkflowStageTextColor2);

				yield return
					new ComponentRole(typeof(Session), ComponentRole.kCarefulSpeechComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.CarefulSpeech", "Careful Speech"),
						ComponentRole.MeasurementTypes.Time,
						FileSystemUtils.GetIsAudioVideo, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Careful",
						Settings.Default.WorkflowStageColor3,
						Settings.Default.WorkflowStageTextColor3);

				yield return
					new ComponentRole(typeof(Session), ComponentRole.kOralTranslationComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.OralTranslation", "Oral Translation"),
						ComponentRole.MeasurementTypes.Time,
						FileSystemUtils.GetIsAudioVideo, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "OralTranslation",
						Settings.Default.WorkflowStageColor4,
						Settings.Default.WorkflowStageTextColor4);

				yield return
					new ComponentRole(typeof(Session), ComponentRole.kTranscriptionComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.Transcription", "Transcription"),
						ComponentRole.MeasurementTypes.Words,
						FileUtils.GetIsText, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Transcription",
						Settings.Default.WorkflowStageColor5,
						Settings.Default.WorkflowStageTextColor5);

				yield return
					new ComponentRole(typeof(Session), ComponentRole.kFreeTranslationComponentRoleId,
						LocalizationManager.GetString("SessionsView.SessionsList.Stages.WrittenTranslation", "Written Translation"),
						ComponentRole.MeasurementTypes.Words,
						FileUtils.GetIsText, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Translation",
						Settings.Default.WorkflowStageColor6,
						Settings.Default.WorkflowStageTextColor6);
			}
		}

		public  LocalizationManager CreateLocalizationManager()
		{
			var defaultTmxFilename = LocalizationManager.GetTmxFileNameForLanguage(kSayMoreLocalizationId, LocalizationManager.kDefaultLang);
			// Move any non-installed tmx files to the new location.
			foreach (var oldTmxFile in Directory.GetFiles(Program.CommonAppDataFolder,
				LocalizationManager.GetTmxFileNameForLanguage(kSayMoreLocalizationId, "*")))
			{
				var filename = Path.GetFileName(oldTmxFile);
				if (filename != null && filename != defaultTmxFilename)
				{
					if (File.Exists(FileLocator.GetFileDistributedWithApplication(true, filename)))
					{
						// This is a copy of factory localization file, so we can probably safely delete it.
						try
						{
							File.Delete(oldTmxFile);
						}
						catch (Exception)
						{
							// Oh, well, just leave it. It's not going to hurt anything.
						}
					}
					else
					{
						if (!Directory.Exists(Program.CustomizedLocalizationsFolder))
							Directory.CreateDirectory(Program.CustomizedLocalizationsFolder);
						File.Move(oldTmxFile, Path.Combine(Program.CustomizedLocalizationsFolder, filename));
					}
				}
			}

			var installedStringFileFolder = Path.GetDirectoryName(FileLocator.GetFileDistributedWithApplication("SayMore.es.tmx"));
			var localizationManager = LocalizationManager.Create(Settings.Default.UserInterfaceLanguage, kSayMoreLocalizationId,
				"SayMore", System.Windows.Forms.Application.ProductVersion, installedStringFileFolder, Program.CommonAppDataFolder,
				Program.CustomizedLocalizationsFolder, Resources.SayMore, "issues@saymore.palaso.org", "SayMore", "SIL.Archiving");
			Settings.Default.UserInterfaceLanguage = LocalizationManager.UILanguageId;
			return localizationManager;
		}

//        public LocalizationManager LocalizationManager
//        {
//            get { return _container.Resolve<LocalizationManager>(); }
//        }

		/// ------------------------------------------------------------------------------------
		public static IDictionary<string, IXmlFieldSerializer> XmlFieldSerializers
		{
			get
			{
				var list = new Dictionary<string, IXmlFieldSerializer>();

				var contributionSerializer = new ContributionSerializer();
				list[contributionSerializer.ElementName] = contributionSerializer;

				// Add other field serializers here.

				return list;
			}
		}

		/// ------------------------------------------------------------------------------------
		public WelcomeDialog CreateWelcomeDialog()
		{
			return _container.Resolve<WelcomeDialog>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_container.Dispose();
			_container = null;
		}

		/// ------------------------------------------------------------------------------------
		public ProjectContext CreateProjectContext(string projectSettingsPath)
		{
			return new ProjectContext(projectSettingsPath, _container);
		}
	}
}