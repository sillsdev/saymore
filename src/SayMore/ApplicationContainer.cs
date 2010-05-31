using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ProjectChoosingAndCreating;

namespace SayMore
{
	/// <summary>
	/// This is sortof a wrapper around the DI container. I'm not thrilled with the name I've
	/// used (jh).
	/// </summary>
	public class ApplicationContainer:IDisposable
	{
		private IContainer _container;

		public ApplicationContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

			//this one overrides the above, for commands, so that anything requiring
			//IEnumerable<ICommand> will get a list of *instances*, one each, of each command.
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				.As<ICommand>()
				.Where(t => t.GetInterfaces().Contains(typeof(ICommand)));

			builder.RegisterInstance(FilesTypes).As(typeof(IEnumerable<FileType>));
			builder.RegisterInstance(ComponentRoles).As(typeof(IEnumerable<ComponentRole>));

			_container = builder.Build();
		}

		/// <summary>
		/// Someday, we may put this under user control. For now, they are hard-coded.
		/// </summary>
		protected IEnumerable<ComponentRole> ComponentRoles
		{
			get
			{
				yield return new
					ComponentRole(typeof (Session), "original", "Original Recording",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Original");

				yield return
					new ComponentRole(typeof (Session), "carefulSpeech", "Careful Speech",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Careful");

				yield return
					new ComponentRole(typeof (Session), "oralTranslation", "Oral Translation",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_OralTranslation");

				yield return
					new ComponentRole(typeof (Session), "transcription", "Transcription",
						ComponentRole.MeasurementTypes.Words,
						(p => Path.GetExtension(p).ToLower() == ".txt"), "$ElementId$_Transcription");

				yield return
					new ComponentRole(typeof (Session), "transcriptionN", "Written Translation",
						ComponentRole.MeasurementTypes.Words,
						(p => Path.GetExtension(p).ToLower() == ".txt"), "$ElementId$_Translation-N");

				yield return
					new ComponentRole(typeof (Person), "consent", "Informed Consent",
						ComponentRole.MeasurementTypes.None, (p => true), "$ElementId$_Consent");
			}
		}

		public WelcomeDialog CreateWelcomeDialog()
		{
			return _container.Resolve<WelcomeDialog>();
		}

		private static IEnumerable<FileType> FilesTypes
		{
			get
			{
				yield return new SessionFileType();
				yield return new PersonFileType();
				yield return new AudioFileType();
				yield return new VideoFileType();
				yield return new ImageFileType();
			}
		}

		public void Dispose()
		{
			_container.Dispose();
			_container = null;
		}

		public ProjectContext CreateProjectContext(string projectSettingsPath)
		{
			return new ProjectContext(projectSettingsPath, _container);
		}
	}
}