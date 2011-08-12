using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;

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
			var path = pathToAnnotatedFile.ToLower();

			if (path.EndsWith(Settings.Default.OralAnnotationGeneratedFileAffix.ToLower()))
			{
				return new OralAnnotationComponentFile(parentElement, pathToAnnotatedFile, _fileTypes,
					_componentRoles, _fileSerializer, _statisticsProvider, _presetProvider, _fieldUpdater);
			}

			if (path.EndsWith(".eaf"))
			{
				return new AnnotationComponentFile(parentElement, pathToAnnotatedFile,
					_fileTypes.Single(t => t is AnnotationFileType), _componentRoles);
			}

			return new ComponentFile(parentElement, pathToAnnotatedFile, _fileTypes,
				_componentRoles, _fileSerializer, _statisticsProvider, _presetProvider, _fieldUpdater);
		}
	}
}
