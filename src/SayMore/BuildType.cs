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
			Production
		}

		public static VersionType Current
		{
#if DEBUG
			get { return VersionType.Debug; }
#else
			get { return VersionType.Production; }
#endif
		}
	}
}
