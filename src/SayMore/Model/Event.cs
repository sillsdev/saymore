using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
			if (string.IsNullOrEmpty(MetaDataFile.GetStringValue("status", null)))
			{
				// REVIEW: Should we report anything if there's an error message returned?
				string errMsg;
				MetaDataFile.SetValue("status", Status.Incoming.ToString(), out errMsg);
				Save();
			}
		}

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
		public static string GetComponentStageText(ComponentStage stage)
		{
			return stage.ToString().Replace('_', ' ').Replace(',', ';');
		}

		/// ------------------------------------------------------------------------------------
		public static Image GetImageForComponentStage(ComponentStage stage)
		{
			Image img = new Bitmap(Resources.CompletedComponentTemplate);

			foreach (var kvp in GetColorsToReplace())
			{
				var clrNew = GetComponentStageColor(stage & kvp.Key);
				var tmpImg = AppColors.ReplaceColor(img, kvp.Value, clrNew);
				img.Dispose();
				img = tmpImg;
			}

			return img;
		}

		/// ------------------------------------------------------------------------------------
		private static IEnumerable<KeyValuePair<ComponentStage, Color>> GetColorsToReplace()
		{
			yield return new KeyValuePair<ComponentStage, Color>(
				ComponentStage.Informed_Consent, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

			yield return new KeyValuePair<ComponentStage, Color>(
				ComponentStage.Translation_Speech, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFE));

			yield return new KeyValuePair<ComponentStage, Color>(
				ComponentStage.Careful_Speech, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFD));

			yield return new KeyValuePair<ComponentStage, Color>(
				ComponentStage.Written_Translation, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFC));

			yield return new KeyValuePair<ComponentStage, Color>(
				ComponentStage.Written_Transcription, Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFB));
		}

		/// ------------------------------------------------------------------------------------
		public static Color GetComponentStageColor(ComponentStage component)
		{
			switch (component)
			{
				case ComponentStage.Informed_Consent: return Settings.Default.InformedConsentColor;
				case ComponentStage.Translation_Speech: return Settings.Default.TranslationSpeechColor;
				case ComponentStage.Careful_Speech: return Settings.Default.CarefulSpeechColor;
				case ComponentStage.Written_Translation: return Settings.Default.WrittenTranslationColor;
				case ComponentStage.Written_Transcription: return Settings.Default.WrittenTranscriptionColor;
				default: return Settings.Default.IncompleteEventComponentColor;
			}
		}
	}
}
