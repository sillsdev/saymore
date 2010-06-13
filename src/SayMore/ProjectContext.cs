using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.ComponentEditors;
using SayMore.UI.ElementListScreen;
using SayMore.UI.ProjectWindow;

namespace SayMore
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// TODO: it might be cleaner to remove this class and just have it all be in method
	/// on applicationContext
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectContext : IDisposable
	{
		/// <summary>
		/// Any resources which belong only to this project will be tracked by this,
		/// and disposed of along with this ProjectContext class
		/// </summary>
		private ILifetimeScope _scope;

		public Project Project { get; private set; }
		public ProjectWindow ProjectWindow { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ProjectContext(string projectSettingsPath, IContainer parentContainer)
		{
			var rootDirectoryPath = Path.GetDirectoryName(projectSettingsPath);
			BuildSubContainerForThisProject(rootDirectoryPath, parentContainer);

			Project = _scope.Resolve<Func<string, Project>>()(projectSettingsPath);

			var sessionRepoFactory = _scope.Resolve<ElementRepository<Session>.Factory>();
			sessionRepoFactory(rootDirectoryPath, "Sessions");

			var peopleRepoFactory = _scope.Resolve<ElementRepository<Person>.Factory>();
			peopleRepoFactory(rootDirectoryPath, "People");

			//Start up the background operations
			_scope.Resolve<AudioVideoDataGatherer>().Start();
			_scope.Resolve<PresetGatherer>().Start();

//			var test = _scope.Resolve<AudioFileType>();
//			var test2 = _scope.Resolve<AudioComponentEditor.Factory>();
//			test2(new ComponentFile("x", null, null, null));
			ProjectWindow = _scope.Resolve<ProjectWindow.Factory>()(projectSettingsPath);
		}

		protected void BuildSubContainerForThisProject(string rootDirectoryPath, IContainer parentContainer)
		{
			_scope = parentContainer.BeginLifetimeScope(builder =>
			{
				builder.RegisterType<ElementRepository<Session>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementRepository<Person>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementListViewModel<Session>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementListViewModel<Person>>().InstancePerLifetimeScope();
				builder.RegisterType<AudioVideoDataGatherer>().InstancePerLifetimeScope();
				builder.RegisterType<IEnumerable<FileType>>().InstancePerLifetimeScope();

				//when something needs the list of filetypes, get them from this method
				builder.Register<IEnumerable<FileType>>(c => GetFilesTypes(c)).InstancePerLifetimeScope();

				//these needed to be done later (as delegates) because of the FileTypes dependency
				//there's maybe something I'm doing wrong that requires me to register this twice like this...
				builder.Register<IProvideAudioVideoFileStatistics>(c => new AudioVideoDataGatherer(rootDirectoryPath,
																								   c.Resolve<IEnumerable<FileType>>()))
					.InstancePerLifetimeScope();
					;
				//.As<IProvideAudioVideoFileStatistics>().As<AudioVideoDataGatherer>();
				builder.Register<AudioVideoDataGatherer>(c => c.Resolve(typeof (IProvideAudioVideoFileStatistics))
																		as AudioVideoDataGatherer).InstancePerLifetimeScope();
				;
				//create a single PresetGatherer and stick it in the container
				//builder.RegisterInstance(parentContainer.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));

				//using the factory gave stack overflow: builder.Register<PresetGatherer>(c => c.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));
				builder.Register<PresetGatherer>(c => new PresetGatherer(rootDirectoryPath, c.Resolve<IEnumerable<FileType>>(), c.Resolve<PresetData.Factory>())).InstancePerLifetimeScope();
			});


		}


		public IEnumerable<FileType> GetFilesTypes(IComponentContext context)
		{
			yield return new SessionFileType();
			yield return new PersonFileType();
			yield return context.Resolve<AudioFileType>();
			yield return new VideoFileType();
			yield return new ImageFileType();

		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_scope.Dispose();
			_scope = null;
		}

		/// ------------------------------------------------------------------------------------
		public T ResolveForTests<T>() where T: class
		{
			return _scope.Resolve<T>();
		}
	}
}