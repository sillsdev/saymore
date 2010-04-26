using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sponge2.Model
{
	class Person : ProjectChild
	{
		protected override string ExtensionWithoutPeriod
		{
			get { return "person"; }
		}
	}
}
