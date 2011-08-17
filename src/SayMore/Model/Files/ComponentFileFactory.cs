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
		private readonly FileSerializer _fileSerializer;
		private readonly IProvideAudioVideoFileStatistics _statisticsProvider;
		private readonly PresetGatherer _presetProvider;
		private readonly FieldUpdater _fieldUpdater;

		/// ------------------------------------------------------------------------------------
		public ComponentFileFactory(IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles, FileSerializer fileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider, FieldUpdater fieldUpdater)
		{
			_fileTypes = fileTypes;
			_componentRoles = componentRoles;
			_fileSerializer = fileSerializer;
			_statisticsProvider = statisticsProvider;
			_presetProvider = presetProvider;
			_fieldUpdater = fieldUpdater;
		}

		/// ------------------------------------------------------------------------------------
		public virtual ComponentFile Create(ProjectElement parentElement, string pathToAnnotatedFile)
		{
			var newComponentFile = new ComponentFile(parentElement, pathToAnnotatedFile, _fileTypes,
			   _componentRoles, _fileSerializer, _statisticsProvider, _presetProvider, _fieldUpdater);

			var annotationFilePath = newComponentFile.GetSuggestedPathToAnnotationFile();
			if (File.Exists(annotationFilePath))
			{
				newComponentFile.SetAnnotationFile(new AnnotationComponentFile(parentElement,
					annotationFilePath, newComponentFile,
					_fileTypes.Single(t => t is AnnotationFileType), _componentRoles));
			}

			annotationFilePath = newComponentFile.GetSuggestedPathToOralAnnotationFile();
			if (File.Exists(annotationFilePath))
			{
				newComponentFile.SetOralAnnotationFile(new OralAnnotationComponentFile(parentElement,
					annotationFilePath, newComponentFile, _fileTypes, _componentRoles,
					_fileSerializer, _statisticsProvider, _presetProvider, _fieldUpdater));
			}

			return newComponentFile;
		}
	}
}