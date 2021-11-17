using System;
using System.Linq;
using System.Xml.Linq;
using SayMore.Model.Files;
using SIL.Extensions;
using SIL.Reporting;
using SIL.Windows.Forms.ClearShare;

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
		public ContributionSerializer() : base(SessionFileType.kContributionsFieldName)
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
				_olacSystem.TryGetRoleByCode(e.Element("role").Value, out var role);
				var contrib = new Contribution(e.Element("name").Value, role);
				// We have this permissive business because we released versions of SayMore (prior to 1.1.120) which used the local
				// format, rather than a universal one.
				var when = e.Element("date").Value;
				try
				{
					contrib.Date = DateTimeExtensions.ParseDateTimePermissivelyWithException(when);
				}
				catch (Exception exception)
				{
					Logger.WriteEvent("Handled exception in ContributionSerializer.Deserialize:\r\n{0}", exception.ToString());
					contrib.Date = DateTime.MinValue;
					// looked like it would take hours to change scores of methods to propagate a progress thing (e.g. ErrorCollector) down this far. Sigh...  progress.WriteError("SayMore had trouble understanding the date '{0}', on a contribution by {1}. For now, it was replaced by {2}", d, contrib.ContributorName, contrib.Date.ToString(CultureInfo.CurrentCulture));
				}
				contrib.Comments = e.Element("notes")?.Value;
				return contrib;
			}).ToArray();

			return (contributionCollection.Any()) ? new ContributionCollection(contributionCollection) : null;
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
					e.Add(new XElement("date", c.Date.ToISO8601TimeFormatDateOnlyString()));
					e.Add(new XElement("notes", c.Comments));
					element.Add(e);
				}

				return element;
			});
		}
	}
}
