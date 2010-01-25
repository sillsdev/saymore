using System.Drawing;
using System.Xml.Serialization;

namespace SIL.Sponge.Model
{
	public enum Privacy
	{
		Public,
		Private,
		Unknown
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information about a person (i.e. contributor)
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("person")]
	public class Person
	{
		[XmlElement("fullName")]
		public string FullName { get; set; }

		[XmlElement("shortName")]
		public string ShortName { get; set; }

		[XmlElement("pseudonym")]
		public string Pseudonym { get; set; }

		[XmlElement("privacy")]
		public Privacy Privacy { get; set; }

		[XmlElement("birthYear")]
		public int BirthYear { get; set; }

		[XmlElement("deathYear")]
		public int DeathYear { get; set; }

		[XmlElement("primaryResidence")]
		public string PrimaryResidence { get; set; }

		[XmlElement("otherResidence")]
		public string OtherResidence { get; set; }

		[XmlElement("lang1")]
		public string Language1 { get; set; }

		[XmlElement("lang2")]
		public string Language2 { get; set; }

		[XmlElement("lang3")]
		public string Language3 { get; set; }

		[XmlElement("primaryOccupation")]
		public string PrimaryOccupation { get; set; }

		[XmlElement("otherOccupation")]
		public string OtherOccupation { get; set; }

		[XmlElement("biographicalSketch")]
		public string BiographicalSketch { get; set; }

		[XmlElement("contactInfo")]
		public string ContactInfo { get; set; }

		[XmlElement("comment")]
		public string Comment { get; set; }

		[XmlElement("picture")]
		public Image Picture { get; set; }
	}
}
