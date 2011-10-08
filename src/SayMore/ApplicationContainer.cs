using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ProjectChoosingAndCreating;
using SilTools;
using SplashScreen=SayMore.UI.SplashScreen;

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
				.Where(t => t.GetInterfaces().Contains(typeof(ICommand)));

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
				ver.Build, fi.CreationTime.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
		public void ShowSplashScreen()
		{
			if (_splashScreen == null)
			{
				_splashScreen = new SplashScreen();
				_splashScreen.Show();
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
					ComponentRole(typeof(Event), Settings.Default.OriginalRecordingComponentRoleId,
						Program.GetString("EventsView.EventsList.Stages.OriginalRecording", "Original Recording"),
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Original",
						Settings.Default.WorkflowStageColor1,
						Settings.Default.WorkflowStageTextColor1);//todo... but maybe we dont' show this as a stage?

				yield return
					new ComponentRole(typeof(Person), "consent",
						Program.GetString("EventsView.EventsList.Stages.InformedConsent", "Informed Consent"),
						ComponentRole.MeasurementTypes.None, (p => true), "$ElementId$_Consent",
						Settings.Default.WorkflowStageColor2,
						Settings.Default.WorkflowStageTextColor2);

				yield return
					new ComponentRole(typeof(Event), "carefulSpeech",
						Program.GetString("EventsView.EventsList.Stages.CarefulSpeech", "Careful Speech"),
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Careful",
						Settings.Default.WorkflowStageColor3,
						Settings.Default.WorkflowStageTextColor3);

				yield return
					new ComponentRole(typeof(Event), "oralTranslation",
						Program.GetString("EventsView.EventsList.Stages.OralTranslation", "Oral Translation"),
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_OralTranslation",
						Settings.Default.WorkflowStageColor4,
						Settings.Default.WorkflowStageTextColor4);

				yield return
					new ComponentRole(typeof(Event), "transcription",
						Program.GetString("EventsView.EventsList.Stages.Transcription", "Transcription"),
						ComponentRole.MeasurementTypes.Words,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Transcription",
						Settings.Default.WorkflowStageColor5,
						Settings.Default.WorkflowStageTextColor5);

				yield return
					new ComponentRole(typeof(Event), "transcriptionN",
						Program.GetString("EventsView.EventsList.Stages.WrittenTranslation", "Written Translation"),
						ComponentRole.MeasurementTypes.Words,
						ComponentRole.GetIsText, "$ElementId$_Translation",
						Settings.Default.WorkflowStageColor6,
						Settings.Default.WorkflowStageTextColor6);
			}
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