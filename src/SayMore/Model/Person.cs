using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Person : ProjectElement
	{
		/// ------------------------------------------------------------------------------------
		public Person(string parentElementFolder, string id, PersonFileType personFileType,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory)
			: base(parentElementFolder, id, personFileType, componentFileFactory, fileSerializer, prjElementComponentFileFactory)
		{
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
