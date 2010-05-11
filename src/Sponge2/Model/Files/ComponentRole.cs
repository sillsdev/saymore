using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Sponge2.Properties;

namespace Sponge2.Model.Files
{
	/// <summary>
	/// A session or person folder contains multiple files, each of which is there because
	/// it plays some role in our workflow.  This class is used to define those roles, so
	/// that when a file is identified with 1 or more roles, we can do things like name it
	/// appropriately, know what work remains to be done, and collect statistics on what has
	/// allready been done.
	///	An object of this class
	/// can tell if a given file is elligble for being the one which fullfills that role,
	/// can tell if the session has that role filled already, and
	/// can rename a file to fit the template for this role.
	/// </summary>
	public class ComponentRole
	{
		public enum MeasurementTypes { None, Time, Words }

		public string Id { get; set; }
		private readonly Type _releventElementType;
		private readonly string _englishLabel;
		public MeasurementTypes MeasurementType { get; private set; }
		public Image Icon { get; set; }

		//tells whether this file looks like it *might* be filling this role
		public Func<string, bool> ElligibilityFilter { get; private set; }

		private readonly string _renamingTemplate;

		public ComponentRole(Type releventElementType, string id, string englishLabel,
			MeasurementTypes measurementType, Func<string, bool> elligibilityFilter,
			string renamingTemplate)
		{
			Id = id;
			_releventElementType = releventElementType;
			_englishLabel = englishLabel;
			MeasurementType = measurementType;
			ElligibilityFilter = elligibilityFilter;
			_renamingTemplate = renamingTemplate;
		}


		public string ConvertFileNameToMatchThisRole(string name)
		{
			return name;
		}

		/// <summary>
		/// Is this file marked as having this role?
		/// </summary>
		public bool IsMatch(string path)
		{
			if(!Path.GetFileNameWithoutExtension(path).Contains(partofRenamingTemplateWhichDoesNotDependOnId))
				return false;
			return ElligibilityFilter(path);
		}

		/// <summary>
		/// Does it make sense to offer this role as something the user can assign to this file?
		/// </summary>
		public bool IsPotential(string path)
		{
			return ElligibilityFilter(path);
		}


		public string Name
		{
			get { return _englishLabel; }
		}


		public static bool GetIsAudioVideo(string path)
		{
			return (GetIsAudio(path) || GetIsVideo(path));
		}

		public static bool GetIsAudio(string path)
		{
			var extensions = Settings.Default.AudioFileExtensions;
			return extensions.Contains(Path.GetExtension(path).ToLower());
		}

		public static bool GetIsVideo(string path)
		{
			var extensions = Settings.Default.VideoFileExtensions;
			return extensions.Contains(Path.GetExtension(path).ToLower());
		}


		public string GetCanoncialName(string sessionId, string path)
		{
			var dir = Path.GetDirectoryName(path);
			//var fileName = Path.GetFileNameWithoutExtension(path);
			var name = _renamingTemplate + Path.GetExtension(path);
			name = name.Replace("$ElementId$", sessionId);
			if (string.IsNullOrEmpty(dir))
			{
				return name;
			}

			return Path.Combine(dir, name);
		}

		public bool AtLeastOneFileHasThisRole(string sessionId, string[] paths)
		{
			return paths.Any(p =>
			{
				var name = Path.GetFileNameWithoutExtension(p);
				return ElligibilityFilter(Path.GetExtension(p))
					&& name.ToLower() == _renamingTemplate.Replace("$ElementId$", sessionId).ToLower();
			}
				);
		}
	}
}
