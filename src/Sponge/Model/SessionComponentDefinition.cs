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
		public enum MeasurementTypes {None, Time, Words}

		public string Id { get; set; }
		private readonly string _englishLabel;
		public MeasurementTypes MeasurementType{ get; private set;}

		//tells whether this file looks like it *might* be filling this role
		public Func<string, bool> ElligibilityFilter { get; private set; }


		private readonly string _renamingTemplate;

		public string Name
		{
			get { return _englishLabel; }
		}

		public static IEnumerable<SessionComponentDefinition> CreateHardCodedDefinitions()
		{
			yield return new SessionComponentDefinition("original", "Original Recording", MeasurementTypes.Time, SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_Original");
			yield return new SessionComponentDefinition("carefulSpeech", "Careful Speech", MeasurementTypes.Time, SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_Careful");
			yield return new SessionComponentDefinition("oralTranslation", "Oral Translation", MeasurementTypes.Time, SessionComponentDefinition.GetIsAudioVideo, "$SessionId$_OralTranslation");
			yield return new SessionComponentDefinition("transcription", "Transcription", MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Transcription");
			yield return new SessionComponentDefinition("transcriptionN", "Written Translation", MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Translation-N");
		}

		public SessionComponentDefinition(string id, string englishLabel, MeasurementTypes measurementType, Func<string, bool> elligibilityFilter, string renamingTemplate)
		{
			Id = id;
			_englishLabel = englishLabel;
			MeasurementType = measurementType;
			ElligibilityFilter = elligibilityFilter;
			_renamingTemplate = renamingTemplate;
		}

		//tells whether this file has been identified as filling this role (i.e., it is named appropriately)
		public Func<string, bool> MatchFilter
		{
			get
			{
				return path => ElligibilityFilter(path) &&
					(Path.GetFileNameWithoutExtension(path).Contains(_renamingTemplate.Replace("$SessionId$","")));
			}
		}


		public static bool GetIsAudioVideo(string path)
		{
			return new [] {".mov", ".avi", ".mp3", ".ogg", ".wav"}.Contains(Path.GetExtension(path).ToLower());
		}

		public bool GetFileIsElligible(string path)
		{
			return ElligibilityFilter(path);
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
							  return ElligibilityFilter(Path.GetExtension(p))
								  && name.ToLower() == _renamingTemplate.Replace("$SessionId$", sessionId).ToLower() ;
						  }
				);
		}
	}
}
