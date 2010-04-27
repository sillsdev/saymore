using System;
using System.IO;
using System.Xml.Serialization;

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

		[Obsolete("Only for use by the deserializer")]
		public Session()
		{
		}

		public Session(ComponentFile.Factory componentFileFactory)
			: base(componentFileFactory)
		{
		}

		public static Session CreateAtLocation(string parentDirectoryPath, string id, ComponentFile.Factory componentFileFactory)
		{
			return (Session)InitializeAtLocation(new Session(componentFileFactory), parentDirectoryPath, id);
		}

		public static Session LoadFromFolder(string sessionFolder, Func<Session, Session> propertyInjectionMethod)
		{
			string sessionFileName = Path.GetFileName(sessionFolder) + "." + ExtensionWithoutPeriodStatic;
			var sessionFilePath = Path.Combine(sessionFolder,sessionFileName);
			if (!File.Exists(sessionFilePath))
			{
				throw new FileNotFoundException(sessionFilePath);
			}
			using (var reader = File.OpenRead(sessionFilePath))
			{
				var session= (Session) (new XmlSerializer(typeof (Session))).Deserialize(reader);
				return propertyInjectionMethod(session);
			}
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
