using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Localization;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.Utilities;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An event is recorded, documented, transcribed, etc.
	/// Each event is represented on disk as a single folder, with 1 or more files
	/// related to that even.  The one file it will always have is some meta data about
	/// the event.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Event : ProjectElement
	{
		public enum Status
		{
			Incoming = 0,
			In_Progress,
			Finished,
			Skipped
		}


		[Flags]
		public enum ComponentStage
		{
			None = 0,
			Informed_Consent = 1,
			Translation_Speech = 2,
			Careful_Speech = 4,
			Written_Translation = 8,
			Written_Transcription = 16
		}

		//autofac uses this
		public delegate Event Factory(string parentElementFolder, string id);

		/// ------------------------------------------------------------------------------------
		public Event(string parentElementFolder, string id, EventFileType eventFileType,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory)
			: base(parentElementFolder, id, eventFileType, componentFileFactory, fileSerializer, prjElementComponentFileFactory)
		{
		}
		/// ------------------------------------------------------------------------------------
		//public Event(string parentElementFolder, string id, EventFileType eventFileType,
		//    ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer,
		//    EventComponentFile.Factory prjElementComponentFileFactory)
		//    : base(parentElementFolder, id, eventFileType, componentFileFactory, fileSerializer, prjElementComponentFileFactory)

		/// ------------------------------------------------------------------------------------
		protected override string ExtensionWithoutPeriod
		{
			get { return ExtensionWithoutPeriodStatic; }
		}

		/// ------------------------------------------------------------------------------------
		public override string RootElementName
		{
			get { return "Event"; }
		}

		/// ------------------------------------------------------------------------------------
		protected static string ExtensionWithoutPeriodStatic
		{
			get { return Settings.Default.EventFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix
		{
			get
			{
				return LocalizationManager.LocalizeString(
					"EventsView.NewEventNamePrefix", "New Event");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return "You must specify a event id."; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get { return "Could not rename from {0} to {1} because there is already a event by that name."; }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultStatusValue
		{
			get { return Status.Incoming.ToString(); }
		}

		#region Static methods
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetStatusNames()
		{
			return Enum.GetNames(typeof(Status)).Select(x => x.ToString().Replace('_', ' '));
		}


		#endregion
	}
}
