using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;
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

//			var filesTypes = GetFilesTypes(parentContainer);
//			builder.RegisterInstance(filesTypes).As(typeof(IEnumerable<FileType>));

			//when something needs the list of filetypes, get them from this method
//			builder.Register<IEnumerable<FileType>>(c => GetFilesTypes(c));
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
					ComponentRole(typeof(Event), "original", "Original Recording",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Original", Color.Magenta);//todo... but maybe we dont' show this as a stage?

				yield return
					new ComponentRole(typeof(Event), "carefulSpeech", "Careful Speech",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_Careful", Settings.Default.CarefulSpeechColor);

				yield return
					new ComponentRole(typeof(Event), "oralTranslation", "Oral Translation",
						ComponentRole.MeasurementTypes.Time,
						ComponentRole.GetIsAudioVideo, "$ElementId$_OralTranslation", Settings.Default.TranslationSpeechColor);

				yield return
					new ComponentRole(typeof(Event), "transcription", "Transcription",
						ComponentRole.MeasurementTypes.Words,
						(p => Path.GetExtension(p).ToLower() == ".txt"), "$ElementId$_Transcription", Color.Magenta);//todo

				yield return
					new ComponentRole(typeof (Event), "transcriptionN", "Written Translation",
						ComponentRole.MeasurementTypes.Words,
						(p => Path.GetExtension(p).ToLower() == ".txt"), "$ElementId$_Translation-N", Settings.Default.WrittenTranslationColor);

				yield return
					new ComponentRole(typeof (Person), "consent", "Informed Consent",
						ComponentRole.MeasurementTypes.None, (p => true), "$ElementId$_Consent", Settings.Default.InformedConsentColor);
			}
		}

		public WelcomeDialog CreateWelcomeDialog()
		{
			return _container.Resolve<WelcomeDialog>();
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