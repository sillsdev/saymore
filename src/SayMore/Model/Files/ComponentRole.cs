using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SayMore.Utilities;

namespace SayMore.Model.Files
{
	/// <summary>
	/// A session or person folder contains multiple files, each of which is there because
	/// it plays some role in our workflow.  This class is used to define those roles, so
	/// that when a file is identified with 1 or more roles, we can do things like name it
	/// appropriately, know what work remains to be done, and collect statistics on what has
	/// allready been done.
	///	An object of this class can tell if a given file is elligble for being the one which
	///	fullfills that role, can tell if the session has that role filled already, and can
	///	rename a file to fit the template for this role.
	/// </summary>
	public class ComponentRole
	{
		public const string kElementIdToken = "$ElementId$";
		public const string kFileSuffixSeparator = "_";
		public const string kSourceComponentRoleId = "source";
		public const string kConsentComponentRoleId = "consent";
		public const string kCarefulSpeechComponentRoleId = "carefulSpeech";
		public const string kOralTranslationComponentRoleId = "oralTranslation";
		public const string kTranscriptionComponentRoleId = "transcription";
		public const string kFreeTranslationComponentRoleId = "transcriptionN";

		private readonly string _englishLabel;

		public enum MeasurementTypes { None, Time, Words }

		public Type RelevantElementType { get; private set; }
		public MeasurementTypes MeasurementType { get; private set; }
		public string Id { get; private set; }
		public Color Color { get; private set; }
		public Color TextColor { get; private set; }

		//tells whether this file looks like it *might* be filling this role
		private readonly Func<string, bool> _elligibilityFilter;
		private readonly string _renamingTemplate;

		/// ------------------------------------------------------------------------------------
		public ComponentRole(Type relevantElementType, string id, string englishLabel,
			MeasurementTypes measurementType, Func<string, bool> elligibilityFilter,
			string renamingTemplate, Color color, Color textColor)
		{
			Id = id;
			RelevantElementType = relevantElementType;
			_englishLabel = englishLabel;
			MeasurementType = measurementType;
			_elligibilityFilter = elligibilityFilter;
			_renamingTemplate = renamingTemplate;
			Color = color;
			TextColor = textColor;
		}

		/// ------------------------------------------------------------------------------------
		public string GetRenamingTemplateSuffix(bool includeLeadingUnderscore = false)
		{
			return _renamingTemplate.Replace(includeLeadingUnderscore ?
				kElementIdToken : kElementIdToken + kFileSuffixSeparator, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Is this file marked as having this role?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsMatch(string path)
		{
			var nameWithoutExtension = Path.GetFileNameWithoutExtension(path);

			return (_elligibilityFilter(path) && nameWithoutExtension != null &&
				(nameWithoutExtension.Contains(GetRenamingTemplateSuffix(true)) ||
					(Id == kSourceComponentRoleId && nameWithoutExtension.Contains("_Original"))));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Does it make sense to offer this role as something the user can assign to this file?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPotential(string path)
		{
			return _elligibilityFilter(path);
		}

		/// ------------------------------------------------------------------------------------
		public string Name
		{
			get { return _englishLabel; }
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanHaveTranscriptionRole(string path)
		{
			return (FileSystemUtils.GetIsAudioVideo(path) || AnnotationFileType.GetIsAnAnnotationFile(path));
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetCanHaveWrittenTranslationRole(string path)
		{
			return (FileSystemUtils.GetIsText(path) || AnnotationFileType.GetIsAnAnnotationFile(path));
		}

		/// ------------------------------------------------------------------------------------
		public string GetCanoncialName(string sessionId, string path)
		{
			var dir = Path.GetDirectoryName(path);
			var name = _renamingTemplate.Replace(kElementIdToken, sessionId) + Path.GetExtension(path);
			return (string.IsNullOrEmpty(dir) ? name : Path.Combine(dir, name));
		}

		///// ------------------------------------------------------------------------------------
		//public bool AtLeastOneFileHasThisRole(string sessionId, string[] paths)
		//{
		//    return paths.Any(p =>
		//    {
		//        var name = Path.GetFileNameWithoutExtension(p).ToLower();
		//        return _elligibilityFilter(Path.GetExtension(p)) &&
		//            (name == GetRenamingTemplateSuffix().ToLower() || name == (sessionId + "_Original").ToLower());
		//    });
		//}

		/// ------------------------------------------------------------------------------------
		public bool IsContainedIn(IEnumerable<ComponentRole> roleSet)
		{
			//todo (jh, jh): why didn't this work? Are there multiple instance being made somewhere?
			//      if (roleSet.Contains(this))
			return roleSet.Any(r => r.Name == Name);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name + ", " + Id + ", Type: " + RelevantElementType;
		}
	}
}