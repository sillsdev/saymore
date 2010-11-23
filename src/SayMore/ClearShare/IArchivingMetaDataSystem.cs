using System.Collections.Generic;

namespace SayMore.ClearShare
{
	public interface IArchivingMetaDataSystem
	{
		Work LoadWorkFromXml(string xml);
		string GetXmlForWork( Work work);
		IEnumerable<Role> GetRoles();
	}
}