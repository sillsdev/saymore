using System.Linq;
using System.Xml.Linq;
using SayMore.ClearShare;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to serialize and deserialize contribution collections to
	/// SayMore meta data files. When we figure out how to store notes and dates at the
	/// contributor level in OLAC, this could be modified to read and write an OLAC record.
	/// For now, it reads and writes a ContributionCollection.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ContributionSerializer : FieldSerializer
	{
		private readonly OlacSystem _olacSystem = new OlacSystem();

		/// ------------------------------------------------------------------------------------
		public ContributionSerializer() : base("contributions")
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the specified chunk of XML into a list of contributions
		/// (i.e. ContributionCollection).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override object Deserialize(string xmlBlob)
		{
			// TODO: Deal with approved license objects of contributions.

			var contributionCollection = GetElementFromXml(xmlBlob).Elements("contributor").Select(e =>
			{
				Role role;
				_olacSystem.TryGetRoleByCode(e.Element("role").Value, out role);
				var contrib = new Contribution(e.Element("name").Value, role);
				contrib.Date = e.Element("date").Value;
				contrib.Notes = e.Element("notes").Value;
				return contrib;
			});

			return new ContributionCollection(contributionCollection);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts a list of contributions into an XElement with sub elements. It's assumed
		/// the obj argument is of type ContributionCollection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override XElement Serialize(object obj)
		{
			// TODO: Deal with approved license objects in contributions.

			return InternalSerialize(obj, typeof(ContributionCollection), element =>
			{
				foreach (var c in (ContributionCollection)obj)
				{
					var e = new XElement("contributor");
					e.Add(new XElement("name", c.ContributorName));
					e.Add(new XElement("role", c.Role.Code));
					e.Add(new XElement("date", c.Date));
					e.Add(new XElement("notes", c.Notes));
					element.Add(e);
				}

				return element;
			});
		}
	}
}
