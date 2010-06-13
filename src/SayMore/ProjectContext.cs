using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
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
//			_scope = parentContainer.BeginLifetimeScope(builder =>
//        	{
//			var rootDirectoryPath = Path.GetDirectoryName(projectSettingsPath);
//				builder.RegisterType<ElementRepository<Session>>().InstancePerLifetimeScope();
//				builder.RegisterType<ElementRepository<Person>>().InstancePerLifetimeScope();
//				builder.RegisterType<ElementListViewModel<Session>>().InstancePerLifetimeScope();
//				builder.RegisterType<ElementListViewModel<Person>>().InstancePerLifetimeScope();
//				builder.RegisterType<AudioVideoDataGatherer>().InstancePerLifetimeScope();
//
				//there's maybe something I'm doing wrong that requires me to register this twice like this...
//				var audioVideoDataGatherer = new AudioVideoDataGatherer(
//					rootDirectoryPath,
//					parentContainer.Resolve<IEnumerable<FileType>>());//enhance... use a factory to make this
//        		builder.RegisterInstance(audioVideoDataGatherer).As<IProvideAudioVideoFileStatistics>();
//				builder.RegisterInstance(audioVideoDataGatherer).As<AudioVideoDataGatherer>();
//
				//create a single PresetGatherer and stick in the container
				//builder.RegisterInstance(parentContainer.Resolve<PresetGatherer.Factory>(rootDirectoryPath));
//        	});
			var rootDirectoryPath = Path.GetDirectoryName(projectSettingsPath);
			BuildSubContainerForThisProject(rootDirectoryPath, parentContainer);

			Project = _scope.Resolve<Func<string, Project>>()(projectSettingsPath);

			var sessionRepoFactory = _scope.Resolve<ElementRepository<Session>.Factory>();
			sessionRepoFactory(rootDirectoryPath, "Sessions");

			var peopleRepoFactory = _scope.Resolve<ElementRepository<Person>.Factory>();
			peopleRepoFactory(rootDirectoryPath, "People");

			//Start up the background operations
			((AudioVideoDataGatherer)_scope.Resolve<IProvideAudioVideoFileStatistics>()).Start();
			_scope.Resolve<PresetGatherer>().Start();

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

				//there's maybe something I'm doing wrong that requires me to register this twice like this...
				var audioVideoDataGatherer = new AudioVideoDataGatherer(
					rootDirectoryPath,
					parentContainer.Resolve<IEnumerable<FileType>>());//enhance... use a factory to make this
				builder.RegisterInstance(audioVideoDataGatherer).As<IProvideAudioVideoFileStatistics>();
				builder.RegisterInstance(audioVideoDataGatherer).As<AudioVideoDataGatherer>();

				//create a single PresetGatherer and stick it in the container
				builder.RegisterInstance(parentContainer.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));
			});
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