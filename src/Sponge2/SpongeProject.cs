/*using System;
using System.IO;
using System.Xml.Serialization;
using SilUtils;

namespace Sponge2
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("spongeProject")]
	public class SpongeProject
	{

		public const string ProjectFileExtention = "sprj";

//		public delegate SpongeProject Factory(string path);//autofac uses this

		/// ------------------------------------------------------------------------------------
		public static bool CreateInFileSystem(string path)
		{
			try
			{
				// It's assumed the path is a full path to a file name.
				var folder = Path.GetDirectoryName(path);

				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				XmlSerializationHelper.SerializeToFile(path, new SpongeProject());
				return true;
			}
			catch (Exception e)
			{
				Palaso.Reporting.ErrorReport.ReportNonFatalException(e);
				return false;
			}
		}

		///// ------------------------------------------------------------------------------------
		//public static SpongeProject Load(string path)
		//{
		//    try
		//    {
		//        return XmlSerializationHelper.DeserializeFromFile<SpongeProject>(path);
		//    }
		//    catch (Exception e)
		//    {
		//        Palaso.Reporting.ErrorReport.ReportNonFatalException(e);
		//        return null;
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		public SpongeProject()
		{
		}

		/// ------------------------------------------------------------------------------------
		public SpongeProject(string path)
		{
			Folder = path;
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Name
		{
			get { return Path.GetFileName(Folder); }
		}


	}
}*/