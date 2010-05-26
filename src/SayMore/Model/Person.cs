using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	public class Person : ProjectElement
	{
		public Person(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory,  FileSerializer fileSerializer)
			:base(parentElementFolder,id, componentFileFactory, fileSerializer, new PersonFileType())
		{
		}

		public override string RootElementName
		{
			get { return "Person"; }
		}

		protected override string ExtensionWithoutPeriod
		{
			get { return Settings.Default.PersonFileExtension.TrimStart('.'); }
		}
	}
}
