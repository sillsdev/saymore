using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Autofac;
using L10NSharp;
using SIL.IO;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI;
using SayMore.UI.ProjectChoosingAndCreating;
using SayMore.Utilities;
using SIL.Reporting;
using static System.Char;
using static System.Environment.SpecialFolder;
using Resources = SayMore.Properties.Resources;

namespace SayMore
{
	/// <summary>
	/// This is sort of a wrapper around the DI container. I'm not thrilled with the name I've
	/// used (jh).
	/// </summary>
	public class ApplicationContainer : IDisposable
	{
		private IContainer _container;
		private ISplashScreen _splashScreen;
		public const string kSayMoreLocalizationId = "SayMore";
		private const string kPalasoLocalizationId = "Palaso";

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

			builder.RegisterInstance(CreateLocalizationManager()).SingleInstance();

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
		public static string GetVersionInfo(string fmt, BuildType.VersionType buildType)
		{
			var asm = Assembly.GetExecutingAssembly();
			var ver = asm.GetName().Version;
			var file = asm.CodeBase.Replace("file:", string.Empty);
			file = file.TrimStart('/');
			var fi = new FileInfo(file);

			return string.Format(fmt, ver.Major, ver.Minor, ver.Build,
				GetBuildTypeDescriptor(buildType), fi.LastWriteTime.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
		public static string GetBuildTypeDescriptor(BuildType.VersionType buildType)
		{
			string type;
			switch (buildType)
			{
				case BuildType.VersionType.Debug:
					type = "Debug"; // Not localizable
					break;
				case BuildType.VersionType.Alpha:
					type = LocalizationManager.GetString("BuildType.Alpha", "Alpha");
					break;
				case BuildType.VersionType.Beta:
					type = LocalizationManager.GetString("BuildType.Beta", "Beta");
					break;
				case BuildType.VersionType.ReleaseCandidate:
					type = LocalizationManager.GetString("BuildType.ReleaseCandidate", "Release Candidate");
					break;
				default:
					string sBuildType = buildType.ToString();
					var sb = new StringBuilder(sBuildType);
					for (int i = 1; i < sBuildType.Length; i++)
					{
						if (IsUpper(sb[i]))
							sb.Insert(i++, ' ');
					}
					type = LocalizationManager.GetDynamicString("SayMore", "BuildType." + sBuildType, sb.ToString());
					break;
			}

			return $"({type})";
		}

		/// ------------------------------------------------------------------------------------
		public void ShowSplashScreen()
		{
			if (_splashScreen == null)
			{
				_splashScreen = new SplashScreen();
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
						FileSystemUtils.GetIsAudioVideo, ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Translation",
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

		public ILocalizationManager CreateLocalizationManager()
		{
			const string emailForLocalizations = "sil.saymore@gmail.com";

			var installedStringFileFolder = Path.GetDirectoryName(
				FileLocationUtilities.GetFileDistributedWithApplication("SayMore.es.xlf"));
			var relativePathForWritingL10nFiles = 
				Path.Combine(Program.kCompanyAbbrev, Application.ProductName);
			var currentUiLanguage = Settings.Default.UserInterfaceLanguage;

			LocalizationManager.DeleteOldTranslationFiles(kSayMoreLocalizationId,
				Path.Combine(Environment.GetFolderPath(CommonApplicationData),
					relativePathForWritingL10nFiles), installedStringFileFolder);

			var localizationManager = LocalizationManager.Create(currentUiLanguage,
				kSayMoreLocalizationId + ".exe", Application.ProductName,
				Application.ProductVersion, installedStringFileFolder,
				relativePathForWritingL10nFiles, Resources.SayMore, emailForLocalizations,
				new [] {"SayMore" });

			LocalizationManager.Create(currentUiLanguage,
				kPalasoLocalizationId, kPalasoLocalizationId, Application.ProductVersion,
				installedStringFileFolder, relativePathForWritingL10nFiles, Resources.SayMore,
				emailForLocalizations, new [] {"SIL.Archiving", "SIL.Windows.Forms.FileSystem",
				"SIL.Windows.Forms.ClearShare", "SIL.Windows.Forms.Miscellaneous",
				"SIL.Reporting", "SIL.Windows.Forms.WritingSystems"});

			Settings.Default.UserInterfaceLanguage = LocalizationManager.UILanguageId;

			Logger.WriteEvent("Initial UI Locale: " + Settings.Default.UserInterfaceLanguage);

			return localizationManager;
		}

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