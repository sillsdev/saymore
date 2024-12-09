using System.Threading;
using SIL.Archiving;

namespace SayMore.Model
{
	internal interface IArchivable
	{
		string ArchiveInfoDetails { get; }

		string Title { get; }

		string Id { get; }

		void InitializeModel(ArchivingDlgViewModel model);

		void SetFilesToArchive(ArchivingDlgViewModel model, CancellationToken cancellationToken);
	}
}
