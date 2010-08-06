using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A session is an event which is recorded, documented, transcribed, etc.
	/// Each sesssion is represented on disk as a single folder, with 1 or more files
	/// related to that even.  The one file it will always have is some meta data about
	/// the event.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Session : ProjectElement
	{
		//autofac uses this
		public delegate Session Factory(string parentElementFolder, string id);

		/// ------------------------------------------------------------------------------------
		public Session(string parentElementFolder, string id, SessionFileType sessionFileType,
			ComponentFile.Factory componentFileFactory,  FileSerializer fileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory)
			: base(parentElementFolder, id, sessionFileType, componentFileFactory, fileSerializer, prjElementComponentFileFactory)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override string ExtensionWithoutPeriod
		{
			get { return ExtensionWithoutPeriodStatic; }
		}

		/// ------------------------------------------------------------------------------------
		public override string RootElementName
		{
			get { return "Session"; }
		}

		/// ------------------------------------------------------------------------------------
		protected static string ExtensionWithoutPeriodStatic
		{
			get { return Settings.Default.SessionFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix
		{
			get
			{
				return LocalizationManager.LocalizeString(
					"SessionsView.NewSessionNamePrefix", "New Session");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return "You must specify a session id."; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get { return "Could not rename from {0} to {1} because there is already a session by that name."; }
		}
	}
}
