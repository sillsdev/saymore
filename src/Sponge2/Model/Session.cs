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
		//autofac uses this
		public delegate Session Factory();

		[Obsolete("Only for use by the deserializer")]
		public Session()
		{
		}

		public Session(ComponentFile.Factory componentFileFactory): base(componentFileFactory)
		{
		}

		public static Session CreateAtLocation(string parentDirectoryPath, string id, ComponentFile.Factory componentFileFactory)
		{
			return (Session)InitializeAtLocation(new Session(componentFileFactory), parentDirectoryPath, id);
		}

		protected override string ExtensionWithoutPeriod
		{
			get { return "session"; }
		}
	}
}
