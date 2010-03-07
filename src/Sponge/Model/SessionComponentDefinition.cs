using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIL.Sponge.Model
{
	/// <summary>
	/// Defines a role a file might play in documenting a session
	/// (e.g. the original recording, a transcription, etc.).  An object of this class
	/// can tell if a given file is elligble for being the one which fullfills that role,
	/// can tell if the session has that role filled already, and
	/// can rename a file to fit the template for this role.
	/// </summary>
	public class SessionComponentDefinition
	{
		private readonly string _englishLabel;
		private readonly Func<string, bool> _elligibilityFilter;
		private readonly string _renamingTemplate;

		public string Name
		{
			get { return _englishLabel; }
		}

		public static IEnumerable<SessionComponentDefinition> CreateHardCodedDefinitions()
		{
			yield return new SessionComponentDefinition("original", "Original Recording", SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_Original");
			yield return new SessionComponentDefinition("carefulSpeech", "Careful Speech", SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_Careful");
			yield return new SessionComponentDefinition("oralTranslation", "Oral Translation", SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_OralTranslation");
			yield return new SessionComponentDefinition("transcription", "Transcription", (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Transcription");
			yield return new SessionComponentDefinition("transcriptionN", "Written Translation", (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Translation-N");
		}

		public SessionComponentDefinition(string id, string englishLabel, Func<string, bool> elligibilityFilter, string renamingTemplate)
		{
			_englishLabel = englishLabel;
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

		public bool SessionHasThisComponent(string sessionId, string[] paths)
		{
			return paths.Any(p =>
						  {
							  var name = Path.GetFileNameWithoutExtension(p);
							  return _elligibilityFilter(Path.GetExtension(p))
								  && name.ToLower() == _renamingTemplate.Replace("$SessionId$", sessionId).ToLower() ;
						  }
				);
		}
	}
}
