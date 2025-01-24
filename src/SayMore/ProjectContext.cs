using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Autofac;
using DesktopAnalytics;
using SayMore.Media.Audio;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;
using SayMore.UI.Overview;
using SayMore.UI.ProjectWindow;
using SIL.IO;
using SIL.Reporting;

namespace SayMore
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// TODO: it might be cleaner to remove this class and just have it all be in a method
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

		public Project Project { get; }
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
				var namesList = new SortedSet<string>();
				var nameRolesList = new SortedSet<string>();
				var contributorLists = new StringBuilder();
				var filesInDir = Directory.GetFiles(sessionFldrPath);
				var sessionFile = filesInDir.FirstOrDefault(f => f.EndsWith(".session"));
				if (sessionFile == null)
					return;

				// SP-2260:We really NEVER want to deal with files that start with ._, but since
				// previous versions of SayMore and certain other ways of creating session files
				// could have resulted in session files that do, we will not exclude ._*.meta
				// files if the session file itself starts with ._
				var doesNotHaveIllegalPrefix = Path.GetFileName(sessionFile).StartsWith(ProjectElement.kMacOsxResourceFilePrefix) ?
					(Func<string, bool>)(fileName => true) :
					fileName => !Path.GetFileName(fileName).StartsWith(ProjectElement.kMacOsxResourceFilePrefix);
				var metaFilesList = filesInDir.Where(f => doesNotHaveIllegalPrefix(f) &&
					f.EndsWith(Settings.Default.MetadataFileExtension) && !f.Contains(Settings.Default.OralAnnotationGeneratedFileSuffix)).ToList();
				var sessionDoc = LoadXmlDocument(sessionFile);
				LoadContributors(sessionDoc, namesList, nameRolesList, contributorLists);
				var root = sessionDoc.DocumentElement;
				
				if (root != null)
				{
					if (root.Attributes.GetNamedItem("version") == null)
					{
						// SP-2303: SayMore 3.5.0 (released 3/22/2023) had a bug whereby it could convert
						// media files to a non-standard (IEEE Float) PCM format that could not be
						// annotated. This code checks for and deletes any such files. Because it takes
						// some time to check this, we only consider files that might have been created
						// after the release date. It would be nice to only do this check if we could know
						// that version 3.5.0 had been installed, but I'm not sure we can reliably and
						// easily detect this. (Maybe by looking for the settings file?). But using this
						// version number, we can at least avoid checking again.
						foreach (var bogusStandardAudioFile in filesInDir.Where(f =>
							         f.EndsWith(Settings.Default.StandardAudioFileSuffix) &&
							         new FileInfo(f).CreationTime >= new DateTime(2023, 3, 21) && 
							         !AudioUtils.GetIsFileStandardPcm(f)))
						{
							Analytics.Track("Delete bogus Standard Audio file");
							RobustFile.Delete(bogusStandardAudioFile);
						}

						// Note: There never was a version 1, but it felt wrong to start with version 1
						// after more than a decade of existence.
						root.SetAttribute("version", "2.0");
					}
				}

				var contributionsNode = root?.SelectSingleNode(SessionFileType.kContributionsFieldName);
				contributionsNode?.ParentNode?.RemoveChild(contributionsNode); //Remove the contributions node
				if (root?.LastChild == null)
					continue;
				foreach (var metaFile in metaFilesList)
				{
					var metaFileDoc = LoadXmlDocument(metaFile);
					LoadContributors(metaFileDoc, namesList, nameRolesList, contributorLists);
				}

				if (!(root.SelectSingleNode("participants") is XmlElement participantsNode))
				{
					participantsNode = sessionDoc.CreateElement("participants");
					participantsNode.SetAttribute("type", "string");
					root.InsertAfter(participantsNode, root.LastChild);
				}
				participantsNode.InnerText = string.Join("; ", namesList);

				var newContributionsNode = sessionDoc.CreateElement(SessionFileType.kContributionsFieldName);
				newContributionsNode.SetAttribute("type", "xml");
				newContributionsNode.InnerXml = contributorLists.ToString();
				root.InsertAfter(newContributionsNode, root.LastChild);
				using (var sessionOutput = XmlWriter.Create(sessionFile, new XmlWriterSettings{Indent = true}))
				{
					sessionDoc.Save(sessionOutput);
				}
			}
		}

		private static XmlDocument LoadXmlDocument(string xmlFile)
		{
			var doc = new XmlDocument();
			using (var reader = XmlReader.Create(xmlFile))
			{
				try
				{
					doc.Load(reader);
				}
				catch (XmlException e)
				{
					Logger.WriteError($"Error loading {xmlFile}", e);
					throw;
				}
			}

			return doc;
		}

		private static void LoadContributors(XmlNode xmlDoc, SortedSet<string> namesList, SortedSet<string> nameRolesList, StringBuilder contributorLists)
		{
			var nodelist = xmlDoc.SelectNodes("//contributor");
			foreach (XmlNode node in nodelist)
			{
				var name = node["name"]?.InnerText;
				var role = node["role"]?.InnerText;
				var item = $@"{name} ({role})";
				if (!nameRolesList.Add(item))
					continue;
				contributorLists.Append(node.OuterXml);
				namesList.Add(name); // Set will avoid duplicates.
			}
			// Check the participants list. Normally this legacy field is derived from contributions.
			// However, if this is a file from an older version of SayMore, it may not have contributions;
			// we need to migrate it. It might also have been edited outside SayMore and have
			// participants that are not known contributors, even though it has a contributions element.
			// So add in any participants we don't already have in some form.
			var participantsNode = xmlDoc.SelectSingleNode("//participants");
			if (participantsNode == null)
				return;
			foreach (var participant in participantsNode.InnerText.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				var name = participant.Trim();

				// SayMore briefly had a state where roles were appended in parens to names in participants.
				// We don't want to create new contributors with names like Joe (consultant).
				// This fix could be unfortunate if someone wants to use a name like "Sally Smith (née Jones)"
				// but we decided the danger of not handling the messed up data files was greater.
				var paren = name.IndexOf("(", StringComparison.Ordinal);
				if (paren >= 0)
					name = name.Substring(0, paren).Trim();
				// If we already have this person, with any role, we won't add again, since we don't have
				// any definite role information in the participants field
				if (name == "" || namesList.Contains(name))
					continue;

				// We have no way of knowing a role. But various code assumes it's not empty.
				// Since we created this contributor on the basis of finding a name in the participants list,
				// it seems a reasonable default to make the role 'participant'.
				// There was no way to specify a date for participants; there is still no way in the
				// UI to specify a contribution date, but there is a field for it in the XML.
				var item = $@"{name} (participant)";
				nameRolesList.Add(item);
				contributorLists.Append(
					$"<contributor><name>{name}</name><role>participant</role><date>0001-01-01</date><notes></notes></contributor>");
				namesList.Add(name); // Set will avoid duplicates.
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
				builder.RegisterType<AnnotationFileWithMissingMediaFileType>().InstancePerLifetimeScope();
				builder.RegisterType<OralAnnotationFileType>().InstancePerLifetimeScope();

				//when something needs the list of filetypes, get them from this method
				builder.Register(GetFilesTypes).InstancePerLifetimeScope();

				//these needed to be done later (as delegates) because of the FileTypes dependency
				//there's maybe something I'm doing wrong that requires me to register this twice like this...
				builder.Register<IProvideAudioVideoFileStatistics>(
					c => new AudioVideoDataGatherer(rootDirectoryPath,
						c.Resolve<IEnumerable<FileType>>())).InstancePerLifetimeScope();

				builder.Register(c => c.Resolve(typeof(IProvideAudioVideoFileStatistics))
						as AudioVideoDataGatherer).InstancePerLifetimeScope();

				// Create a single PresetGatherer and stick it in the container
				// builder.RegisterInstance(parentContainer.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));

				// Using the factory gave stack overflow:
				// builder.Register<PresetGatherer>(c => c.Resolve<PresetGatherer.Factory>()(rootDirectoryPath));
				builder.Register(c => new PresetGatherer(rootDirectoryPath,
					GetDataGatheringFilesTypes(c), c.Resolve<PresetData.Factory>())).InstancePerLifetimeScope();

				builder.Register(
					c => new AutoCompleteValueGatherer(rootDirectoryPath, GetDataGatheringFilesTypes(c),
						c.Resolve<Func<ProjectElement, string, ComponentFile>>())).InstancePerLifetimeScope();

				builder.Register(
					c => new FieldGatherer(rootDirectoryPath, GetDataGatheringFilesTypes(c),
						c.Resolve<FileTypeFields.Factory>())).InstancePerLifetimeScope();

				builder.Register(c => new FieldUpdater(c.Resolve<FieldGatherer>(),
					c.Resolve<IDictionary<string, IXmlFieldSerializer>>())).InstancePerLifetimeScope();

				builder.Register(c => new ComponentFileFactory(
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
				context.Resolve<AnnotationFileWithMissingMediaFileType>(),
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