using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Palaso.Code;

namespace Sponge2.Model
{
	public class Session : ProjectChild
	{
		public Session()
		{
		}

		public static Session CreateAtLocation(string parentDirectoryPath, string id)
		{
			return (Session)InitializeAtLocation(new Session(), parentDirectoryPath, id);
		}

		protected override string ExtensionWithoutPeriod
		{
			get { return "session"; }
		}
	}
}
