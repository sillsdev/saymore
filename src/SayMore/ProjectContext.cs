using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Navigation;
using System.Xml;
using Autofac;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using SayMore.UI.Overview;
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

		private readonly AudioVideoDataGatherer _audioVideoDataGatherer;
		private readonly PresetGatherer _presetGatherer;
		private readonly AutoCompleteValueGatherer _autoCompleteValueGatherer;
		private readonly FieldGatherer _fieldGatherer;

		/// ------------------------------------------------------------------------------------
		public ProjectContext(string projectSettingsPath, IContainer parentContainer)
		{
			var rootDirectoryPath = Path.GetDirectoryName(projectSettingsPath);
			BuildSubContainerForThisProject(rootDirectoryPath, parentContainer);

			Project = _scope.Resolve<Func<string, Project>>()(projectSettingsPath);

			SetContributorsListToSession(Project.SessionsFolder);

			var peopleRepoFactory = _scope.Resolve<ElementRepository<Person>.Factory>();
			peopleRepoFactory(rootDirectoryPath, Person.kFolderName, _scope.Resolve<PersonFileType>());

			var sessionRepoFactory = _scope.Resolve<ElementRepository<Session>.Factory>();
			sessionRepoFactory(rootDirectoryPath, Session.kFolderName, _scope.Resolve<SessionFileType>());

			// Create background operations
			_presetGatherer = _scope.Resolve<PresetGatherer>();
			_autoCompleteValueGatherer = _scope.Resolve<AutoCompleteValueGatherer>();
			_audioVideoDataGatherer = _scope.Resolve<AudioVideoDataGatherer>();
			_fieldGatherer = _scope.Resolve<FieldGatherer>();

			// Start background operations
			_presetGatherer.Start();
			_autoCompleteValueGatherer.Start();
			_audioVideoDataGatherer.Start();
			_fieldGatherer.Start();

			var view1 = _scope.Resolve<ProjectScreen>();
			var view2 = _scope.Resolve<SessionsListScreen>();
			var view3 = _scope.Resolve<PersonListScreen>();

			var views = new ISayMoreView[]
			{
				view1,
				view2,
				view3
			};

			ProjectWindow = _scope.Resolve<ProjectWindow.Factory>()(projectSettingsPath, views);
		}

		///-------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the contributor list to the session file from the metafiles
		/// </summary>
		/// <param name="sessionsFolder">Session folder path</param>
		///-------------------------------------------------------------------------------------------------------
		public static void SetContributorsListToSession(string sessionsFolder)
		{
			if (!Directory.Exists(sessionsFolder) || Path.GetFileName(sessionsFolder).ToLower() != "sessions")
			{
				return;
			}
			var dirLists = Directory.GetDirectories(sessionsFolder);
			foreach (var sessionFldrPath in dirLists)
			{
				var sessionDoc = new XmlDocument();
				List<KeyValuePair<string, string>> namesList = new List<KeyValuePair<string, string>>();
				List<string> contributorLists = new List<string>();
				var filesInDir = Directory.GetFiles(sessionFldrPath);
				var sessionFile = filesInDir.Where(x => x.Contains(".session")).FirstOrDefault();
				if (sessionFile == null) return;
				var metaFilesList = filesInDir.Where(f => f.Contains(Settings.Default.MetadataFileExtension)).ToList();
				sessionDoc.Load(sessionFile);
				XmlNode root = sessionDoc.DocumentElement;
				XmlNode contributionsNode = root.SelectSingleNode("contributions");
				contributionsNode?.ParentNode?.RemoveChild(contributionsNode); //Remove the contributions node
				if (root.LastChild != null)
				{
					foreach (var metaFile in metaFilesList)
					{
						var metaFileDoc = new XmlDocument();
						metaFileDoc.Load(metaFile);
						XmlNodeList nodelist = metaFileDoc.SelectNodes("//contributor");
						if (nodelist != null)
						{
							foreach (XmlNode node in nodelist)
							{
								var name = node["name"]?.InnerText;
								var role = node["role"]?.InnerText;
								if (!namesList.Contains(new KeyValuePair<string, string>(name, role)))
								{
									namesList.Add(new KeyValuePair<string, string>(name, role));
									contributorLists.Add(node.OuterXml);
								}
							}
						}
					}

					string participants = string.Empty;
					var newContributionsNode = sessionDoc.CreateElement("contributions");
					newContributionsNode.SetAttribute("type", "xml");
					foreach (string contributor in contributorLists)
					{
						newContributionsNode.InnerXml += contributor;
					}

					XmlNode participantsNode = root.SelectSingleNode("participants");
					foreach (KeyValuePair<string, string> name in namesList)
					{
						participants += name.Key + " (" + name.Value + ");";
					}

					if (participantsNode != null && participants.Length > 0)
					{
						participantsNode.InnerText = participants;
					}

					root.InsertAfter(newContributionsNode, root.LastChild);
					sessionDoc.Save(sessionFile);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
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

				builder.RegisterType<Project>().InstancePerLifetimeScope();

				builder.RegisterType<SessionFileType>().InstancePerLifetimeScope();
				builder.RegisterType<PersonFileType>().InstancePerLifetimeScope();
				builder.RegisterType<AnnotationFileType>().InstancePerLifetimeScope();
				builder.RegisterType<AnnotationFileWithMisingMediaFileType>().InstancePerLifetimeScope();
				builder.RegisterType<OralAnnotationFileType>().InstancePerLifetimeScope();

				//when something needs the list of filetypes, get them from this method
				builder.Register<IEnumerable<FileType>>(GetFilesTypes).InstancePerLifetimeScope();

				//these needed to be done later (as delegates) because of the FileTypes dependency
				//there's maybe something I'm doing wrong that requires me to register this twice like this...
				builder.Register<IProvideAudioVideoFileStatistics>(
					c => new AudioVideoDataGatherer(rootDirectoryPath,
						c.Resolve<IEnumerable<FileType>>())).InstancePerLifetimeScope();

				builder.Register<AudioVideoDataGatherer>(c => c.Resolve(typeof(IProvideAudioVideoFileStatistics))
						as AudioVideoDataGatherer).InstancePerLifetimeScope();

				//create a single PresetGatherer and stick it in the container
				//builder.RegisterInstance(parentContainer.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));

				//using the factory gave stack overflow: builder.Register<PresetGatherer>(c => c.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));
				builder.Register<PresetGatherer>(c => new PresetGatherer(rootDirectoryPath,
					GetDataGatheringFilesTypes(c), c.Resolve<PresetData.Factory>())).InstancePerLifetimeScope();

				builder.Register<AutoCompleteValueGatherer>(
					c => new AutoCompleteValueGatherer(rootDirectoryPath, GetDataGatheringFilesTypes(c),
						c.Resolve<Func<ProjectElement, string, ComponentFile>>())).InstancePerLifetimeScope();

				builder.Register<FieldGatherer>(
					c => new FieldGatherer(rootDirectoryPath, GetDataGatheringFilesTypes(c),
						c.Resolve<FileTypeFields.Factory>())).InstancePerLifetimeScope();

				builder.Register<FieldUpdater>(c => new FieldUpdater(c.Resolve<FieldGatherer>(),
					c.Resolve<IDictionary<string, IXmlFieldSerializer>>())).InstancePerLifetimeScope();

				builder.Register<ComponentFileFactory>(c => new ComponentFileFactory(
					c.Resolve<IEnumerable<FileType>>(),
					c.Resolve<IEnumerable<ComponentRole>>(),
					c.Resolve<XmlFileSerializer>(),
					c.Resolve<IProvideAudioVideoFileStatistics>(),
					c.Resolve<PresetGatherer>(),
					c.Resolve<FieldUpdater>()));

				// This replaces the ComponentFile.Factory that was expected in various constructors.
				builder.Register<Func<ProjectElement, string, ComponentFile>>(c => c.Resolve<ComponentFileFactory>().Create);

				//make a lazy factory-getter to get around a mysterious circular dependency problem
				//NB: when we move to .net 4, we can remove this and instead use Lazy<Func<PersonBasicEditor.Factory> in the PersonFileType constructor
				//builder.Register<Func<PersonBasicEditor.Factory>>(c => () => c.Resolve<PersonBasicEditor.Factory>());
				//builder.Register<Func<SessionBasicEditor.Factory>>(c => () => c.Resolve<SessionBasicEditor.Factory>());
			});
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<FileType> GetFilesTypes(IComponentContext context)
		{
			return new List<FileType>(new FileType[]
			{
				context.Resolve<SessionFileType>(),
				context.Resolve<PersonFileType>(),
				context.Resolve<AnnotationFileType>(),
				context.Resolve<OralAnnotationFileType>(),	// This must come before AudioFileType.
				context.Resolve<AudioFileType>(),
				context.Resolve<VideoFileType>(),
				context.Resolve<ImageFileType>(),
				context.Resolve<AnnotationFileWithMisingMediaFileType>(),
				context.Resolve<UnknownFileType>(),
			});
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<FileType> GetDataGatheringFilesTypes(IComponentContext context)
		{
			return new List<FileType>(new FileType[]
			{
				context.Resolve<SessionFileType>(),
				context.Resolve<PersonFileType>(),
				// REVIEW: Should this be a gathered type?
				// context.Resolve<OralAnnotationFileType>(),
				context.Resolve<AudioFileType>(),
				context.Resolve<VideoFileType>(),
				context.Resolve<ImageFileType>(),
				context.Resolve<UnknownFileType>(),
			});
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_audioVideoDataGatherer.Dispose();
			_presetGatherer.Dispose();
			_autoCompleteValueGatherer.Dispose();
			_fieldGatherer.Dispose();

			if (ProjectWindow != null)
			{
				ProjectWindow.Dispose();
				ProjectWindow = null;
			}

			_scope.Dispose();
			_scope = null;
			GC.Collect();
		}

		/// ------------------------------------------------------------------------------------
		public void SuspendAudioVideoBackgroundProcesses()
		{
			if (_audioVideoDataGatherer != null)
				_audioVideoDataGatherer.SuspendProcessing();
		}

		/// ------------------------------------------------------------------------------------
		public void ResumeAudioVideoBackgroundProcesses(bool processAllPendingEventsNow)
		{
			if (_audioVideoDataGatherer != null)
				_audioVideoDataGatherer.ResumeProcessing(processAllPendingEventsNow);
		}

		/// ------------------------------------------------------------------------------------
		public void SuspendBackgroundProcesses()
		{
			if (_audioVideoDataGatherer != null)
				_audioVideoDataGatherer.SuspendProcessing();

			if (_autoCompleteValueGatherer != null)
				_autoCompleteValueGatherer.SuspendProcessing();

			if (_fieldGatherer != null)
				_fieldGatherer.SuspendProcessing();

			if (_presetGatherer != null)
				_presetGatherer.SuspendProcessing();
		}

		/// ------------------------------------------------------------------------------------
		public void ResumeBackgroundProcesses(bool processAllPendingEventsNow)
		{
			if (_audioVideoDataGatherer != null)
				_audioVideoDataGatherer.ResumeProcessing(processAllPendingEventsNow);

			if (_autoCompleteValueGatherer != null)
				_autoCompleteValueGatherer.ResumeProcessing(processAllPendingEventsNow);

			if (_fieldGatherer != null)
				_fieldGatherer.ResumeProcessing(processAllPendingEventsNow);

			if (_presetGatherer != null)
				_presetGatherer.ResumeProcessing(processAllPendingEventsNow);
		}

		/// ------------------------------------------------------------------------------------
		public T ResolveForTests<T>() where T: class
		{
			return _scope.Resolve<T>();
		}
	}
}