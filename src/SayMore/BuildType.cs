namespace SayMore
{
	public static class BuildType
	{
		public enum VersionType
		{
			// Do not delete "unused" types. The "Current" type can be set using a parameter on TeamCity via
			// Palaso.BuildTasks.UpdateBuildTypeFile.UpdateBuildTypeFile.
			Debug,
			Alpha,
			AlphaTest,
			Beta,
			ReleaseCandidate,
			Release,
		}

		public static VersionType Current
		{
#if DEBUG
			get { return VersionType.Debug; }
#else
			get { return VersionType.Alpha; }
#endif
		}
	}
}
