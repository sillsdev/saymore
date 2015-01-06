namespace SayMore
{
	public static class BuildType
	{
		public enum VersionType
		{
			Debug,
			Alpha,
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
