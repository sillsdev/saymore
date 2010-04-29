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
	public class Session : ProjectElement
	{
		//autofac uses this
		public delegate Session Factory();

		/// <summary>
		/// Use this for creating new elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		/// <param name="componentFileFactory"></param>
		public Session(string parentElementFolder, string id, ComponentFile.Factory componentFileFactory)
			:base(parentElementFolder, id, componentFileFactory)
		{
		}

		/// <summary>
		/// Use this constructor for existing elements which just need to be read off disk
		/// </summary>
		/// <param name="existingElementFolder">E.g. "c:/MyProject/Sessions/ETR007"</param>
		/// <param name="componentFileFactory"></param>
		public Session(string existingElementFolder, ComponentFile.Factory componentFileFactory)
			:base(existingElementFolder,componentFileFactory)
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
