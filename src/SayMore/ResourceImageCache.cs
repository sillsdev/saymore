using System.Collections.Generic;
using System.Drawing;
using SayMore.Properties;

namespace SayMore
{
	internal static class ResourceImageCache
	{
		private static readonly Dictionary<string, Bitmap> s_bitmaps = new Dictionary<string, Bitmap>();

		internal static Bitmap GetBitmap(string name)
		{
			Bitmap obj;
			if (!s_bitmaps.TryGetValue(name, out obj))
			{
				obj = (Bitmap) Resources.ResourceManager.GetObject(name, Resources.Culture);
				s_bitmaps[name] = obj;
			}
			return obj;
		}

		internal static Bitmap AudioFileImage
		{
			get { return GetBitmap("AudioFileImage"); }
		}

		internal static Bitmap AudioInformedConsent
		{
			get { return GetBitmap("AudioInformedConsent"); }
		}

		internal static Bitmap CheckedBox
		{
			get { return GetBitmap("CheckedBox"); }
		}

		internal static Bitmap ColumnChooser
		{
			get { return GetBitmap("ColumnChooser"); }
		}

		internal static Bitmap Copy
		{
			get { return GetBitmap("Copy"); }
		}

		internal static Bitmap DropDownArrow
		{
			get { return GetBitmap("DropDownArrow"); }
		}

		internal static Bitmap ElanIcon
		{
			get { return GetBitmap("ElanIcon"); }
		}

		internal static Bitmap Green_check
		{
			get { return GetBitmap("Green_check"); }
		}

		internal static Bitmap HSliderThumb
		{
			get { return GetBitmap("HSliderThumb"); }
		}

		internal static Bitmap HSliderThumbDisabled
		{
			get { return GetBitmap("HSliderThumbDisabled"); }
		}

		internal static Bitmap HSliderThumbMousePressed
		{
			get { return GetBitmap("HSliderThumbMousePressed"); }
		}

		internal static Bitmap ImageFileImage
		{
			get { return GetBitmap("ImageFileImage"); }
		}

		internal static Bitmap Information_blue
		{
			get { return GetBitmap("Information_blue"); }
		}

		internal static Bitmap Information_red
		{
			get { return GetBitmap("Information_red"); }
		}

		internal static Bitmap kimidChangePicture
		{
			get { return GetBitmap("kimidChangePicture"); }
		}

		internal static Bitmap kimidFemale_NotSelected
		{
			get { return GetBitmap("kimidFemale_NotSelected"); }
		}

		internal static Bitmap kimidFemale_Selected
		{
			get { return GetBitmap("kimidFemale_Selected"); }
		}

		internal static Bitmap kimidMale_NotSelected
		{
			get { return GetBitmap("kimidMale_NotSelected"); }
		}

		internal static Bitmap kimidMale_Selected
		{
			get { return GetBitmap("kimidMale_Selected"); }
		}

		internal static Bitmap kimidNoPhoto
		{
			get { return GetBitmap("kimidNoPhoto"); }
		}

		internal static Bitmap kimidSendReceive
		{
			get { return GetBitmap("kimidSendReceive"); }
		}

		internal static Bitmap kimidWarning
		{
			get { return GetBitmap("kimidWarning"); }
		}

		internal static Bitmap LargeSayMoreLogo
		{
			get { return GetBitmap("LargeSayMoreLogo"); }
		}

		internal static Bitmap ListenToOriginalRecording
		{
			get { return GetBitmap("ListenToOriginalRecording"); }
		}

		internal static Bitmap ListenToOriginalRecordingDown
		{
			get { return GetBitmap("ListenToOriginalRecordingDown"); }
		}

		internal static Bitmap ListenToSegment
		{
			get { return GetBitmap("ListenToSegment"); }
		}

		internal static Bitmap MuteVolume
		{
			get { return GetBitmap("MuteVolume"); }
		}

		internal static Bitmap NoInformedConsent
		{
			get { return GetBitmap("NoInformedConsent"); }
		}

		internal static Bitmap NotesTabImage
		{
			get { return GetBitmap("NotesTabImage"); }
		}

		internal static Bitmap People
		{
			get { return GetBitmap("People"); }
		}

		internal static Bitmap PersonFileImage
		{
			get { return GetBitmap("PersonFileImage"); }
		}


		internal static Bitmap PlaySegment
		{
			get { return GetBitmap("PlaySegment"); }
		}

		internal static Bitmap PlayTabImage
		{
			get { return GetBitmap("PlayTabImage"); }
		}

		internal static Bitmap Print
		{
			get { return GetBitmap("Print"); }
		}

		internal static Bitmap Progress
		{
			get { return GetBitmap("Progress"); }
		}

		internal static Bitmap project
		{
			get { return GetBitmap("project"); }
		}

		internal static Bitmap RampIcon
		{
			get { return GetBitmap("RampIcon"); }
		}

		internal static Bitmap RecordingOralAnnotationInProgress
		{
			get { return GetBitmap("RecordingOralAnnotationInProgress"); }
		}

		internal static Bitmap RecordOralAnnotation
		{
			get { return GetBitmap("RecordOralAnnotation"); }
		}

		internal static Bitmap RerecordOralAnnotation
		{
			get { return GetBitmap("RerecordOralAnnotation"); }
		}

		internal static Bitmap Save
		{
			get { return GetBitmap("Save"); }
		}

		internal static Bitmap SayMoreText
		{
			get { return GetBitmap("SayMoreText"); }
		}

		internal static Bitmap SessionFileImage
		{
			get { return GetBitmap("SessionFileImage"); }
		}

		internal static Bitmap Sessions
		{
			get { return GetBitmap("Sessions"); }
		}


		internal static Bitmap StatusFinished
		{
			get { return GetBitmap("StatusFinished"); }
		}

		internal static Bitmap StatusIn_Progress
		{
			get { return GetBitmap("StatusIn_Progress"); }
		}

		internal static Bitmap StatusIncoming
		{
			get { return GetBitmap("StatusIncoming"); }
		}

		internal static Bitmap StatusSkipped
		{
			get { return GetBitmap("StatusSkipped"); }
		}

		internal static Bitmap StopSegment
		{
			get { return GetBitmap("StopSegment"); }
		}

		internal static Bitmap UncheckedBox
		{
			get { return GetBitmap("UncheckedBox"); }
		}

		internal static Bitmap VideoFileImage
		{
			get { return GetBitmap("VideoFileImage"); }
		}

		internal static Bitmap VideoInformedConsent
		{
			get { return GetBitmap("VideoInformedConsent"); }
		}

		internal static Bitmap Volume
		{
			get { return GetBitmap("Volume"); }
		}

		internal static Bitmap VSliderThumb
		{
			get { return GetBitmap("VSliderThumb"); }
		}

		internal static Bitmap VSliderThumbDisabled
		{
			get { return GetBitmap("VSliderThumbDisabled"); }
		}

		internal static Bitmap VSliderThumbPressed
		{
			get { return GetBitmap("VSliderThumbPressed"); }
		}

		internal static Bitmap WrittenInformedConsent
		{
			get { return GetBitmap("WrittenInformedConsent"); }
		}
	}
}
