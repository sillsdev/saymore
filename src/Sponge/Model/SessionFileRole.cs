using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIL.Sponge.Model
{
	/// <summary>
	/// Defines a role a file might play in documenting a session
	/// (e.g. the original recording, a transcription, etc.)
	/// </summary>
	public class SessionFileRole
	{
		private readonly Func<string, bool> _elligibilityFilter;
		private readonly string _renamingTemplate;

		public static IEnumerable<SessionFileRole> CreateStandardRoleSet()
		{
			yield return new SessionFileRole("original", "Original Recording", SessionFileRole.GetIsAudioVideo, "$SessionId$_Original");
			yield return new SessionFileRole("carefulSpeech", "Careful Speech", SessionFileRole.GetIsAudioVideo, "$SessionId$_Careful");
			yield return new SessionFileRole("oralTranslation", "Oral Translation", SessionFileRole.GetIsAudioVideo, "$SessionId$_OralTranslation");
			yield return new SessionFileRole("transcription", "Transcription", (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Transcription");
			yield return new SessionFileRole("transcriptionN", "Written Translation", (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Translation-N");
		}

		public SessionFileRole(string id, string englishLabel, Func<string, bool> elligibilityFilter, string renamingTemplate)
		{
			_elligibilityFilter = elligibilityFilter;
			_renamingTemplate = renamingTemplate;
		}

		public static bool GetIsAudioVideo(string path)
		{
			return new [] {".mov", ".avi", ".mp3", ".ogg", ".wav"}.Contains(Path.GetExtension(path).ToLower());
		}

		public bool GetFileIsElligible(string path)
		{
			return _elligibilityFilter(path);
		}

		public string GetCanoncialName(string sessionId, string path)
		{
			var dir = Path.GetDirectoryName(path);
			//var fileName = Path.GetFileNameWithoutExtension(path);
			var name = _renamingTemplate + Path.GetExtension(path);
			name = name.Replace("$SessionId$", sessionId);
			if (string.IsNullOrEmpty(dir))
			{
				return name;
			}
			else
			{
				return Path.Combine(dir, name);
			}

		}

		public bool GetSomeFileForThisRoleExists(string sessionId, string[] paths)
		{
			return false;
		}
	}
}
