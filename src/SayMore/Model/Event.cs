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

		/// ------------------------------------------------------------------------------------
		public static string GetComponentStageText(ComponentStage stage)
		{
			return stage.ToString().Replace('_', ' ').Replace(',', ';');
		}

		/// ------------------------------------------------------------------------------------
		public static Image GetComponentStageColorBlock(ComponentStage stage)
		{
			var clrNew = GetComponentStageColor(stage);

			return AppColors.ReplaceColor(Resources.ComponentStageColorBlockTemplate,
				Color.FromArgb(0xFF, Color.White), clrNew);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an image representing the specified stages. Colors representing the stages
		/// not found in the specified stage are absent (i.e. filled with some whitish color).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Image GetImageForComponentStage(ComponentStage stage)
		{
			var sz = Resources.ComponentStageColorBlockTemplate.Size;
			var stages = Enum.GetValues(typeof(ComponentStage)) as ComponentStage[];

			// Subtract 1 from the number of stages so the value 'None' is not included.
			var bmp = new Bitmap((sz.Width - 1) * (stages.Length - 1) + 1, sz.Height);

			// Now create a single image by combining the blocks for each stage
			// that is not the 'None' stage.
			using (var g = Graphics.FromImage(bmp))
			{
				int dx = 0;

				foreach (var cs in stages)
				{
					if (cs != ComponentStage.None)
					{
						using (var block = GetComponentStageColorBlock(cs & stage))
							g.DrawImageUnscaled(block, dx, 0);

						dx += (sz.Width - 1);
					}
				}
			}

			return bmp;
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

		#endregion
	}
}
