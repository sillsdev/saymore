using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SIL.Localization;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// It also tells us which commands to offer in, for example, a context menu.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FileType
	{
		protected Func<BasicFieldGridEditor.Factory> _basicFieldGridEditorFactoryLazy;
		protected Func<string, bool> _isMatchPredicate;

		protected readonly List<IEditorProvider> _editors = new List<IEditorProvider>();

		public string Name { get; private set; }
		public virtual string TypeDescription { get; protected set; }
		public virtual Image SmallIcon { get; protected set; }
		public virtual string FileSize { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string matchForEndOfFileName)
		{
			return new FileType(name, p => p.EndsWith(matchForEndOfFileName));
		}

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string[] matchesForEndOfFileName)
		{
			return new FileType(name, p => matchesForEndOfFileName.Any(ext => p.EndsWith(ext)));
		}

		/// ------------------------------------------------------------------------------------
		public FileType(string name, Func<string, bool>isMatchPredicate)
		{
			Name = name;
			_isMatchPredicate = isMatchPredicate;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsMatch(string path)
		{
			return _isMatchPredicate(path);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string FieldsGridSettingsName
		{
			get { return "UnknownFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count == 0)
				_editors.Add(new DiagnosticsFileInfoControl(file));
			else
				_editors[0].SetComponentFile(file);

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<FileCommand> Commands
		{
			get
			{
				yield return new FileCommand("Show in File Explorer...", FileCommand.HandleOpenInFileManager_Click);
				yield return new FileCommand("Open in Program Associated with this File ...", FileCommand.HandleOpenInApp_Click);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsForUnknownFileTypes
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsAudioOrVideo
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile + Settings.Default.MetadataFileExtension;
		}
	}

	#region PersonFileType class
	/// ----------------------------------------------------------------------------------------
	public class PersonFileType : FileType
	{
		private readonly Func<PersonBasicEditor.Factory> _personBasicEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="personBasicEditorFactoryLazy">This is to get us around a circular dependency
		/// error in autofac.  NB: when we move to .net 4, this can be replaced by Lazy<Func<PersonBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public PersonFileType(Func<PersonBasicEditor.Factory> personBasicEditorFactoryLazy)
			: base("Person", p => p.EndsWith(".person"))
		{
			_personBasicEditorFactoryLazy = personBasicEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "PersonCustomFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count > 0)
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}
			else
			{
				var text = LocalizationManager.LocalizeString("PersonInfoEditor.PersonTabText", "Person");
				_editors.Add(_personBasicEditorFactoryLazy()(file, text, "Person"));

				text = LocalizationManager.LocalizeString("PersonInfoEditor.NotesTabText", "Notes");
				_editors.Add(new NotesEditor(file, text, "Notes"));

				//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> Commands
		{
			get
			{
				yield return new FileCommand("Show in File Explorer...",
					FileCommand.HandleOpenInFileManager_Click);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.PersonFileImage; }
		}
	}

	#endregion

	#region SessionFileType class
	/// ----------------------------------------------------------------------------------------
	public class SessionFileType : FileType
	{
		private readonly Func<SessionBasicEditor.Factory> _sessionBasicEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="sessionBasicEditorFactoryLazy">This is to get us around a circular dependency
		/// error in autofac.  NB: when we move to .net 4, this can be replaced by Lazy<Func<SessionBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public SessionFileType(Func<SessionBasicEditor.Factory> sessionBasicEditorFactoryLazy)
			: base("Session", p => p.EndsWith(".session"))
		{
			_sessionBasicEditorFactoryLazy = sessionBasicEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "SessionCustomFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count > 0)
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}
			else
			{
				var text = LocalizationManager.LocalizeString("SessionInfoEditor.SessionTabText", "Session");
				_editors.Add(_sessionBasicEditorFactoryLazy()(file, text, "Session"));

				text = LocalizationManager.LocalizeString("SessionInfoEditor.NotesTabText", "Notes");
				_editors.Add(new NotesEditor(file, text, "Notes"));

				//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> Commands
		{
			get
			{
				yield return new FileCommand("Show in File Explorer...",
					FileCommand.HandleOpenInFileManager_Click);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.SessionFileImage; }
		}
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : FileType
	{
		private readonly Func<AudioComponentEditor.Factory> _audioComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AudioFileType(Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy)
			: base("Audio",
			p => Settings.Default.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_audioComponentEditorFactoryLazy = audioComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudioOrVideo
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "AudioFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count > 0)
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}
			else
			{
				var text = LocalizationManager.LocalizeString("AudioFileInfoEditor.PlaybackTabText", "Audio");
				_editors.Add(new AudioVideoPlayer(file, text, "Audio"));

				text = LocalizationManager.LocalizeString("AudioFileInfoEditor.PropertiesTabText", "Properties");
				_editors.Add(_audioComponentEditorFactoryLazy()(file, text, null));

				text = LocalizationManager.LocalizeString("AudioFileInfoEditor.NotesTabText", "Notes");
				_editors.Add(new NotesEditor(file, text, "Notes"));

				//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get {return Resources.AudioFileImage;}
		}
	}

	#endregion

	#region VideoFileType class
	/// ----------------------------------------------------------------------------------------
	public class VideoFileType : FileType
	{
		private readonly Func<VideoComponentEditor.Factory> _videoComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public VideoFileType(Func<VideoComponentEditor.Factory> videoComponentEditorFactoryLazy)
			: base("Video",
				p => Settings.Default.VideoFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_videoComponentEditorFactoryLazy = videoComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudioOrVideo
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "VideoFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count > 0)
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}
			else
			{
				var text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PlaybackTabText", "Video");
				_editors.Add(new AudioVideoPlayer(file, text, "Video"));

				text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PropertiesTabText", "Properties");
				_editors.Add(_videoComponentEditorFactoryLazy()(file, text, null));

				text = LocalizationManager.LocalizeString("VideoFileInfoEditor.NotesTabText", "Notes");
				_editors.Add(new NotesEditor(file, text, "Notes"));

				//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.VideoFileImage; }
		}
	}

	#endregion

	#region ImageFileType class
	/// ----------------------------------------------------------------------------------------
	public class ImageFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public ImageFileType(Func<BasicFieldGridEditor.Factory> basicFieldGridEditorFactoryLazy)
			: base("Image",
			p => Settings.Default.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_basicFieldGridEditorFactoryLazy = basicFieldGridEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "ImageFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<IEditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_editors.Count > 0)
			{
				foreach (var editor in _editors)
					editor.SetComponentFile(file);
			}
			else
			{
				var text = LocalizationManager.LocalizeString("ImageFileInfoEditor.ViewTabText", "Image");
				_editors.Add(new ImageViewer(file, text, "Image"));

				text = LocalizationManager.LocalizeString("ImageFileInfoEditor.PropertiesTabText", "Properties");
				_editors.Add(_basicFieldGridEditorFactoryLazy()(file, text, null));

				text = LocalizationManager.LocalizeString("ImageFileInfoEditor.NotesTabText", "Notes");
				_editors.Add(new NotesEditor(file, text, "Notes"));

				//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
			}

			return _editors;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.ImageFileImage; }
		}
	}

	#endregion
}