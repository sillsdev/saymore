using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Palaso.IO;

namespace SayMore.ClearShare
{
	/// <summary>
	/// Serializes/Deserializes a work and all the information ClearShare has about it, as an OLAC record.
	/// </summary>
	public class OlacSystem : IArchivingMetaDataSystem
	{
		public Work LoadWorkFromXml(string xml)
		{
			//TODO: parse the xml as olac. For a first pass, we can ignore anything we don't understand.
			//Eventually, we'll want to round-trip things we don't understand.
			return new Work();
		}


		public string GetXmlForWork( Work work)
		{
			return @"<olac:olac xmlns:olac='http://www.language-archives.org/OLAC/1.1/'
			xmlns:dc='http://purl.org/dc/elements/1.1/'
	  xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
	  xsi:schemaLocation='http://www.language-archives.org/OLAC/1.1/
		 http://www.language-archives.org/OLAC/1.1/olac.xsd'>

//TODO for each contributore, spit out
			 <dc:contributor xsi:type='olac:role' olac:code='compiler' view='Compiler'>Holzknecht, H. K.</dc:contributor>

   </olac:olac>";

			/*            <dc:language xsi:type="olac:language" olac:code="adz"
				  view="Adzera"/>

			  <dc:subject xsi:type="olac:language" olac:code="adz"
				  view="Adzera"/>



	  <dc:title>Language</dc:title>
	  <dc:publisher>New York: Holt</dc:publisher>
			 */

		}

		/// <summary>
		/// Get all the roles in the system's controlled vocabulary
		/// </summary>
		public IEnumerable<Role> GetRoles()
		{
			var path = FileLocator.GetFileDistributedWithApplication("olac", "roles.xml");

			//todo: read that file and extract the roles, then cache them so that we don't have to parse it constantly

			yield return new Role("author", "Author", "The participant contributed original writings to the resource.");
		}


		/// <summary>
		/// Used to look up roles in the system's controlled vocabulary
		/// </summary>
		public bool TryGetRole(string code, out Role role)
		{
			role = GetRoles().FirstOrDefault<Role>(r => r.Code == code);
			return role != null;
		}

		/// <summary>
		/// Used to look up roles in the system's controlled vocabulary
		/// </summary>
		public Role GetRoleOrThrow(string code)
		{
			var role = GetRoles().FirstOrDefault<Role>(r => r.Code == code);
			if(role==null)
				throw new ArgumentOutOfRangeException("This version of OLAC does not contain a role with code '"+code+"'.");
			return role;
		}
	}
}
