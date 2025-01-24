
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
using DatePicker = SayMore.UI.LowLevelControls.DatePicker;

namespace SayMore.Utilities
{
	internal static class ExtensionMethods
	{
		public static bool IsValidBirthYear(this string birthYear)
		{
			var val = birthYear.Trim();

			// year must be a 4 digit integer
			return string.IsNullOrEmpty(val) || (val.Length == 4 && int.TryParse(val, out _));
		}

		/// <summary>
		/// When the user presses Enter, should we treat it as a Tab press?
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static bool ShouldTreatEnterAsTab(this Control ctrl)
		{
			if (ctrl is ComboBox || ctrl is DatePicker)
				return true;

			// Enter is valid input for a multi-line text box
			if (ctrl is TextBox txt)
				return !txt.Multiline;

			return false;
		}

		/// <summary>
		/// We only want to jump to controls which we can type into
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static bool ShouldTabToMe(this Control ctrl)
		{
			return ctrl is ComboBox || 
			       ctrl is DatePicker || 
			       ctrl is TextBox || 
			       ctrl is DataGridView;
		}

		/// <summary>
		/// Gets whether the media object contains a (or multiple) moving video streams. Note that media
		/// can have a "video" stream that is really just a thumbnail image and not a "motion picture".
		/// ENHANCE: This should be implemented in <see cref="SIL.Media.MediaInfo"/> instead of here.
		/// </summary>
		public static bool IsMotionPicture(this SIL.Media.MediaInfo silMediaInfo)
		{
			var silVideoInfo = silMediaInfo.Video;
			if (silVideoInfo == null)
				return false;
			// Unfortunately, just checking for a Video stream is not enough because ffProbe
			// treats an embedded thumbnail jpeg as "video" (which is technically true).
			return silMediaInfo.AnalysisData.PrimaryVideoStream != null &&
				(silMediaInfo.AnalysisData.PrimaryVideoStream.BitRate > 0 || 
					silMediaInfo.AnalysisData.PrimaryVideoStream.AvgFrameRate > 0 || 
					silMediaInfo.AnalysisData.PrimaryVideoStream.FrameRate > 0);
		}

		public static XElement GetXElement(this XmlNode node)
		{
			XDocument xDoc = new XDocument();
			using (XmlWriter xmlWriter = xDoc.CreateWriter())
				node.WriteTo(xmlWriter);
			return xDoc.Root;
		}
	}
}
