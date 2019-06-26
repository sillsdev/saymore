using System.Threading;
using NUnit.Framework;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class FileSyncHelperTests
	{
		[Test][Category("SkipOnTeamCity")]
		public void IsSynched_Dropbox_ReturnsDropbox()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.Dropbox))
				Assert.Ignore("Dropbox is not running. Ignoring.");

			var result =
				FileSyncHelper.IsSynched(@"C:\Users\TestUser\Dropbox\SayMore\Projects\TestProject\TestProject.proj");

			Assert.AreEqual(FileSyncHelper.SyncClient.Dropbox, result);
		}

		[Test][Category("SkipOnTeamCity")]
		public void IsSynched_GoogleDrive_ReturnsGoogleDrive()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.GoogleDrive))
				Assert.Ignore("Google Drive Sync is not running. Ignoring.");

			var result =
				FileSyncHelper.IsSynched(@"C:\Users\TestUser\Google Drive\SayMore\Projects\TestProject\TestProject.proj");

			Assert.AreEqual(FileSyncHelper.SyncClient.GoogleDrive, result);
		}

		[Test][Category("SkipOnTeamCity")]
		public void IsSynched_OneDrive_ReturnsOneDrive()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.OneDrive))
				Assert.Ignore("OneDrive is not running. Ignoring.");

			var result =
				FileSyncHelper.IsSynched(@"C:\Users\TestUser\OneDrive\SayMore\Projects\TestProject\TestProject.proj");

			Assert.AreEqual(FileSyncHelper.SyncClient.OneDrive, result);
		}

		[Test][Category("SkipOnTeamCity")]
		public void StopClient_Dropbox_KillAndStart()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.Dropbox))
				Assert.Ignore("Dropbox is not running. Ignoring.");

			FileSyncHelper.StopClient(FileSyncHelper.SyncClient.Dropbox);

			Thread.Sleep(2000);

			Assert.False(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.Dropbox));

			FileSyncHelper.RestartAllStoppedClients();

			Thread.Sleep(2000);

			Assert.True(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.Dropbox));
		}

		[Test][Category("SkipOnTeamCity")]
		public void StopClient_GoogleDrive_KillAndStart()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.GoogleDrive))
				Assert.Ignore("Google Drive Sync is not running. Ignoring.");

			FileSyncHelper.StopClient(FileSyncHelper.SyncClient.GoogleDrive);

			Thread.Sleep(2000);

			Assert.False(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.GoogleDrive));

			FileSyncHelper.RestartAllStoppedClients();

			Thread.Sleep(2000);

			Assert.True(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.GoogleDrive));
		}

		[Test][Category("SkipOnTeamCity")]
		public void StopClient_OneDrive_KillAndStart()
		{
			if (!FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.OneDrive))
				Assert.Ignore("OneDrive is not running. Ignoring.");

			FileSyncHelper.StopClient(FileSyncHelper.SyncClient.OneDrive);

			Thread.Sleep(2000);

			Assert.False(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.OneDrive));

			FileSyncHelper.RestartAllStoppedClients();

			Thread.Sleep(2000);

			Assert.True(FileSyncHelper.ClientIsRunning(FileSyncHelper.SyncClient.OneDrive));
		}
	}
}
