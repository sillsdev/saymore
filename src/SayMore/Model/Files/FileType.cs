using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;

namespace SayMore.Model.Files
{
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// It also tells us which commands to offer in, for example, a context menu.
	/// </summary>
	public class FileType
	{
		private readonly Func<string, bool> _isMatchPredicate;

		protected readonly List<EditorProvider> _providers = new List<EditorProvider>();

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
		public bool IsMatch(string path)
		{
			return _isMatchPredicate(path);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			yield return new EditorProvider(new DiagnosticsFileInfoControl(file), "Info");
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

		public virtual bool IsAudioOrVideo
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}

	#region PersonFileType class
	/// ----------------------------------------------------------------------------------------
	public class PersonFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public PersonFileType() : base("Person", p => p.EndsWith(".person"))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			// review: this will create a new editor provider and basic editor for each
			// person. That seems a bit resource intensive. It would be nice to reuse the editor.

			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new PersonBasicEditor(file), "Technical", "Technical"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
			}

			return _providers;
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
			get { return Resources.PersonComponentFileImage; }
		}
	}

	#endregion

	#region SessionFileType class
	/// ----------------------------------------------------------------------------------------
	public class SessionFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public SessionFileType() : base("Session", p => p.EndsWith(".session"))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			// review: this will create a new editor provider and basic editor for each
			// session. That seems a bit resource intensive when the user has a lot of
			// sessions. It would be nice to reuse the editor.

			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new SessionBasicEditor(file), "Technical", "Technical"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
			}

			return _providers;
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
			get { return Resources.SessionComponentFileImage; }
		}
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public AudioFileType() : base("Audio",
			p => Settings.Default.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
		}


		public override bool IsAudioOrVideo
		{
			get { return true; }
		}
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new AudioComponentEditor(file), "Technical", "Technical"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
				_providers.Add(new EditorProvider(new AudioVideoPlayer(file), "Play", "Play"));
			}

			return _providers;
		}


		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get {return Resources.AudioComponentFileImage;}
		}
	}

	#endregion

	#region VideoFileType class
	/// ----------------------------------------------------------------------------------------
	public class VideoFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public VideoFileType() : base("Video",
				p => Settings.Default.VideoFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
		}

		public override bool IsAudioOrVideo
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new VideoComponentEditor(file), "Technical", "Technical"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
				_providers.Add(new EditorProvider(new AudioVideoPlayer(file), "Play", "Play"));
			}

			return _providers;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.VideoComponentFileImage; }
		}
	}

	#endregion

	#region ImageFileType class
	/// ----------------------------------------------------------------------------------------
	public class ImageFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public ImageFileType() : base("Image",
			p => Settings.Default.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<EditorProvider> GetEditorProviders(ComponentFile file)
		{
			if (_providers.Count == 0)
			{
				_providers.Add(new EditorProvider(new ImageViewer(file), "View", "View"));
				_providers.Add(new EditorProvider(new NotesEditor(file), "Notes", "Notes"));
				_providers.Add(new EditorProvider(new ContributorsEditor(file), "Contributors", "Contributors"));
			}

			return _providers;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.ImageComponentFileImage; }
		}
	}

	#endregion
}