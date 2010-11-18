using System.Drawing;

namespace SayMore.ClearShare
{
	/// <summary>
	/// describes a single license, under which many works can be licensed for use
	/// </summary>
	public class License
	{
		/// <summary>
		/// a web location describing the license
		/// </summary>
		public string Url { get; private set; }

		public string Name { get; private set; }

		public Image Logo { get; private set; }
	}
}
