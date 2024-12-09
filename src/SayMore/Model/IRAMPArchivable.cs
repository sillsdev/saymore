using System.Text;
using SIL.Archiving;

namespace SayMore.Model
{
	internal interface IRAMPArchivable : IArchivable
	{
		string GetFileDescription(string key, string file);

		void CustomFilenameNormalization(string key, string file, StringBuilder bldr);

		void SetAdditionalMetsData(RampArchivingDlgViewModel model);
	}
}
