using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using L10NSharp;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Person : ProjectElement
	{
		public static string kFolderName = "People";

		public enum Status
		{
			Incoming = 0,
			Finished,
		}

		/// ------------------------------------------------------------------------------------
		public override string UiId
		{
			get
			{
				var code = MetaDataFile.GetStringValue(PersonFileType.kCode, Id);
				return string.IsNullOrEmpty(code) ? Id : code;
			}
		}

		/// ------------------------------------------------------------------------------------
		public Person(string parentElementFolder, string id,
			Action<ProjectElement, string, string> idChangedNotificationReceiver,
			PersonFileType personFileType,
			Func<ProjectElement, string,
			ComponentFile> componentFileFactory,
			XmlFileSerializer xmlFileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory,
			IEnumerable<ComponentRole> componentRoles)
			: base(parentElementFolder, id, idChangedNotificationReceiver, personFileType,
				componentFileFactory, xmlFileSerializer, prjElementComponentFileFactory, componentRoles)
		{
		}

		[Obsolete("For Mocking Only")]
		public Person(){}

		/// ------------------------------------------------------------------------------------
		public override string RootElementName
		{
			get { return "Person"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string ExtensionWithoutPeriod
		{
			get { return Settings.Default.PersonFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix
		{
			get { return LocalizationManager.GetString("PeopleView.Miscellaneous.NewPersonNamePrefix", "New Person"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return LocalizationManager.GetString("PeopleView.Miscellaneous.NoIdSaveFailureMessage", "You must specify a name."); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get
			{
				return LocalizationManager.GetString("PeopleView.Miscellaneous.AlreadyExistsSaveFailureMessage",
					"Could not rename from {0} to {1} because there is already a person by that name.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultStatusValue
		{
			get { return Status.Incoming.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		public Image GetInformedConsentImage()
		{
			var componentFile = GetInformedConsentComponentFile();
			if (componentFile == null)
				return ResourceImageCache.NoInformedConsent;

			if (componentFile.FileType.IsAudio)
				return ResourceImageCache.AudioInformedConsent;

			if (componentFile.FileType.IsVideo)
				return ResourceImageCache.VideoInformedConsent;

			return ResourceImageCache.WrittenInformedConsent;
		}

		/// ------------------------------------------------------------------------------------
		public int GetInformedConsentSortKey()
		{
			var componentFile = GetInformedConsentComponentFile();

			if (componentFile == null)
				return 0;

			if (componentFile.FileType.IsAudio)
				return 1;

			if (componentFile.FileType.IsVideo)
				return 2;

			return 3;
		}

		/// ------------------------------------------------------------------------------------
		public string GetToolTipForInformedConsentType()
		{
			var componentFile = GetInformedConsentComponentFile();

			if (componentFile == null)
				return LocalizationManager.GetString("PeopleView.InformedConsentTypes.None", "No Informed Consent");

			if (componentFile.FileType.IsAudio)
				return LocalizationManager.GetString("PeopleView.InformedConsentTypes.Audio", "Informed Consent is Audio File");

			if (componentFile.FileType.IsVideo)
				return LocalizationManager.GetString("PeopleView.InformedConsentTypes.Video", "Informed Consent is Video File");

			return LocalizationManager.GetString("PeopleView.InformedConsentTypes.Written", "Informed Consent is Written");
		}

		/// ------------------------------------------------------------------------------------
		public virtual ComponentFile GetInformedConsentComponentFile()
		{
			foreach (var component in GetComponentFiles())
			{
				if (component.GetAssignedRoles().FirstOrDefault(x => x.Id == ComponentRole.kConsentComponentRoleId) != null)
					return component;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public override void NotifyOtherElementsOfUiIdChange(string oldUiId)
		{
			if (IdChangedNotificationReceiver != null)
				IdChangedNotificationReceiver(this, oldUiId, UiId);
		}
	}
}
