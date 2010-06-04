using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class Person : ProjectElement
	{
		/// ------------------------------------------------------------------------------------
		public Person(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer)
			:base(parentElementFolder,id, componentFileFactory, fileSerializer, new PersonFileType())
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
	}
}
