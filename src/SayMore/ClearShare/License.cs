using System;
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

		//TODO: support the full six options at http://creativecommons.org/licenses/, plus public domain

		/// ------------------------------------------------------------------------------------
		public static License CreativeCommons_Attribution_ShareAlike
		{
			get
			{
				return new License
				{
					Name = "Creative Commons. Attribution-ShareAlike 3.0",
					Url = "http://creativecommons.org/licenses/by-sa/3.0/"
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		public static License CreativeCommons_Attribution
		{
			get
			{
				return new License
				{
					Name = "Creative Commons. Attribution 3.0",
					Url = "http://creativecommons.org/licenses/by/3.0/"
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if the contents of this License are the same as those of the
		/// specified License.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreContentsEqual(License other)
		{
			// TODO: compare logo images.
			return (other != null && Name == other.Name && Url == other.Url);
		}
	}
}
