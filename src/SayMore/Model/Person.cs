using Localization;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Person : ProjectElement
	{
		public enum Status
		{
			Incoming = 0,
			Finished,
		}

		/// ------------------------------------------------------------------------------------
		public Person(string parentElementFolder, string id, PersonFileType personFileType,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory)
			: base(parentElementFolder, id, personFileType, componentFileFactory, fileSerializer, prjElementComponentFileFactory)
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
			get
			{
				return LocalizationManager.LocalizeString(
					"PersonView.NewPersonNamePrefix", "New Person");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return "You must specify a name."; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get { return "Could not rename from {0} to {1} because there is already a person by that name."; }
		}
	}
}
