using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sponge2.Model
{
	public class Person : ProjectChild
	{
		public Person(ComponentFile.Factory componentFileFactory): base(componentFileFactory)
		{
		}

		protected override string ExtensionWithoutPeriod
		{
			get { return "person"; }
		}
	}
}
