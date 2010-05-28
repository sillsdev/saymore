using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.UI.Utilities;

namespace SayMore.Model.Files
{
	/// <summary>
	/// Each file corresponds to a single kind of fileType.  The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// It also tells us which commands to offer in, for example, a context menu.
	/// </summary>
	public class FileType
	{
		#region Windows API stuff
#if !MONO
		public const uint SHGFI_DISPLAYNAME = 0x00000200;
		public const uint SHGFI_TYPENAME = 0x400;
		public const uint SHGFI_EXETYPE = 0x2000;
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
		public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint
			dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
#endif
		#endregion

		private readonly Func<string, bool> _isMatchPredicate;

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
			var match = _isMatchPredicate(path);
			if (match)
			{
				Bitmap smallIcon;
				string name;
				GetSmallIconAndFileType(path, out smallIcon, out name);
				SmallIcon = smallIcon;
				Name = Name ?? name;

				// File should only not exist during tests.
				var fi = new FileInfo(path);
				FileSize = (fi.Exists ? GetDisplayableFileSize(fi.Length) : "0 KB");
			}

			return match;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the small icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void GetSmallIconAndFileType(string fullFilePath, out Bitmap smallIcon,
			out string fileType)
		{
#if !MONO
			SHFILEINFO shinfo = new SHFILEINFO();
			SHGetFileInfo(fullFilePath, 0, ref
				shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME |
				SHGFI_SMALLICON | SHGFI_ICON | SHGFI_DISPLAYNAME);

			// This should only be zero during tests.
			smallIcon = (shinfo.hIcon == IntPtr.Zero ?
				null : Icon.FromHandle(shinfo.hIcon).ToBitmap());

			fileType = shinfo.szTypeName;
#else
			// REVIEW: Figure out a better way to get this in Mono.
			Icon icon = Icon.ExtractAssociatedIcon(fullFilePath);
			var largeIcons = new ImageList();
			largeIcons.Images.Add(icon);
			var bmSmall = new Bitmap(16, 16);

			using (var g = Graphics.FromImage(bmSmall))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(LargeIcon, new Rectangle(0, 0, 16, 16),
					new Rectangle(new Point(0, 0), LargeIcon.Size), GraphicsUnit.Pixel);
			}

			smallIcon = bmSmall;
			// TODO: Figure out how to get FileType in Mono.
#endif
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the session file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetDisplayableFileSize(long fileSize)
		{
			if (fileSize < 1000)
				return string.Format("{0} B", fileSize);

			if (fileSize < Math.Pow(1024, 2))
			{
				var size = (int)Math.Round(fileSize / (decimal)1024, 2, MidpointRounding.AwayFromZero);
				if (size < 1)
					size = 1;

				return string.Format("{0} KB", size.ToString("###"));
			}

			double dblSize;
			if (fileSize < Math.Pow(1024, 3))
			{
				dblSize = Math.Round(fileSize / Math.Pow(1024, 2), 2, MidpointRounding.AwayFromZero);
				return string.Format("{0} MB", dblSize.ToString("###.##"));
			}

			dblSize = Math.Round(fileSize / Math.Pow(1024, 3), 2, MidpointRounding.AwayFromZero);
			return string.Format("{0} GB", dblSize.ToString("###,###.##"));
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
		readonly List<EditorProvider> _providers = new List<EditorProvider>();

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
	}

	#endregion

	#region SessionFileType class
	/// ----------------------------------------------------------------------------------------
	public class SessionFileType : FileType
	{
		readonly List<EditorProvider> _providers = new List<EditorProvider>();

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
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : FileType
	{
		readonly List<EditorProvider> _providers = new List<EditorProvider>();

		/// ------------------------------------------------------------------------------------
		public AudioFileType() : base("Audio",
			p => Settings.Default.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
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
		readonly List<EditorProvider> _providers = new List<EditorProvider>();

		/// ------------------------------------------------------------------------------------
		public VideoFileType() : base("Video",
				p => Settings.Default.VideoFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
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
		readonly List<EditorProvider> _providers = new List<EditorProvider>();

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