using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sponge2.Persistence;

namespace Sponge2.Model
{
	public class Person : ProjectElement
	{
		public Person(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory,  FileSerializer fileSerializer)
			:base(parentElementFolder,id, componentFileFactory, fileSerializer)
		{
		}

		public override string RootElementName
		{
			get { return "Person"; }
		}
//
//		public Person(string desiredOrExistingFolder, ComponentFile.Factory componentFileFactory)
//			: base(desiredOrExistingFolder, componentFileFactory)
//		{
//
//		}

		protected override string ExtensionWithoutPeriod
		{
			get { return "person"; }
		}
	}
}
