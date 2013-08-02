using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Model.Files.DataGathering;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is used to create the different types of component files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ComponentFileFactory
	{
		private readonly IEnumerable<FileType> _fileTypes;
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly XmlFileSerializer _xmlFileSerializer;
		private readonly IProvideAudioVideoFileStatistics _statisticsProvider;
		private readonly PresetGatherer _presetProvider;
		private readonly FieldUpdater _fieldUpdater;

		/// ------------------------------------------------------------------------------------
		public ComponentFileFactory(IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles, XmlFileSerializer xmlFileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider, FieldUpdater fieldUpdater)
		{
			_fileTypes = fileTypes;
			_componentRoles = componentRoles;
			_xmlFileSerializer = xmlFileSerializer;
			_statisticsProvider = statisticsProvider;
			_presetProvider = presetProvider;
			_fieldUpdater = fieldUpdater;
		}

		/// ------------------------------------------------------------------------------------
		public virtual ComponentFile Create(ProjectElement parentElement, string pathToAnnotatedFile)
		{
			var newComponentFile = new ComponentFile(parentElement, pathToAnnotatedFile, _fileTypes,
			   _componentRoles, _xmlFileSerializer, _statisticsProvider, _presetProvider, _fieldUpdater);

			var annotationFilePath = newComponentFile.GetSuggestedPathToAnnotationFile();
			if (File.Exists(annotationFilePath))
			{
				newComponentFile.SetAnnotationFile(new AnnotationComponentFile(parentElement,
					annotationFilePath, newComponentFile, _fileTypes.ToList(), _componentRoles));
			}

			return newComponentFile;
		}
	}
}
