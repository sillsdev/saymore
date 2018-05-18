using System;
using System.Collections.Generic;
using System.Text;
using SayMore.Model.Files;
using SIL.Archiving;
using SIL.Archiving.IMDI;

namespace SayMore.Model
{
	internal interface IRAMPArchivable
	{
		string ArchiveInfoDetails { get; }

		string Title { get; }

		string Id { get; }

		void InitializeModel(RampArchivingDlgViewModel model);

		void SetFilesToArchive(ArchivingDlgViewModel model);

		string GetFileDescription(string key, string file);

		void CustomFilenameNormalization(string key, string file, StringBuilder bldr);

		void DisplayInitialArchiveSummary(IDictionary<string, Tuple<IEnumerable<string>, string>> fileLists,
			ArchivingDlgViewModel model);

		void SetAdditionalMetsData(RampArchivingDlgViewModel model);
	}
}
