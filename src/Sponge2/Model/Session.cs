using System;
using System.IO;
using System.Xml.Serialization;
using Palaso.Code;

namespace Sponge2.Model
{
	/// <summary>
	/// A session is an event which is recorded, documented, transcribed, etc.
	/// Each sesssion is represented on disk as a single folder, with 1 or more files
	/// related to that even.  The one file it will always have is some meta data about
	/// the event.
	/// </summary>
	public class Session : ProjectChild
	{
		//autofac uses this
		public delegate Session Factory();

		public Session(string desiredOrExistingFolder, ComponentFile.Factory componentFileFactory)
			: base(desiredOrExistingFolder, componentFileFactory)
		{

		}

		protected override string ExtensionWithoutPeriod
		{
			get { return ExtensionWithoutPeriodStatic; }
		}

		protected static string ExtensionWithoutPeriodStatic
		{
			get { return "session"; }
		}
		public string InfoForPrototype
		{
			get
			{
				return Id;
			}
		}
	}
}
