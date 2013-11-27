using SIL.Archiving;
using SIL.Archiving.IMDI;

namespace SayMore.Model
{
	internal interface IIMDIArchivable
	{
		string ArchiveInfoDetails { get; }

		string Title { get; }

		string Id { get; }

		void InitializeModel(IMDIArchivingDlgViewModel model);

		void SetFilesToArchive(ArchivingDlgViewModel model);
	}
}
