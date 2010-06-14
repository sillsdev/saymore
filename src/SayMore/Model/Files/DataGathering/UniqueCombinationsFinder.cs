using System.Collections.Generic;
using System.Linq;

namespace SayMore.Model.Files.DataGathering
{
	/// <summary>
	/// Its job is simply to provide 0 or more suggestions for collections of values to assign
	/// to media files.  It does that by looking at the values which have been used so far,
	/// and (eventually) sorting them by frequency.
	/// </summary>
	public class UniqueCombinationsFinder
	{
		private readonly IEnumerable<Dictionary<string, string>> _existingInstances;

		public UniqueCombinationsFinder(IEnumerable<Dictionary<string, string>> existingInstances)
		{
			_existingInstances = existingInstances;
		}

		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetSuggestions()
		{
			return from set in
					(
						from x in _existingInstances
						select x).Distinct(new SetComparer())
				   select new KeyValuePair<string, Dictionary<string, string>>(GetLabelForSet(set), set);

			//enhance: sort by frequency
		}

		private static string GetLabelForSet(Dictionary<string, string> dictionary)
		{
			string label = "";
			foreach (var value in dictionary.Values)
			{
				if(!string.IsNullOrEmpty(value))
				{
					label += value + ", ";
				}
			}
			return label.Trim(new char[] {' ', ',' });
		}

		class SetComparer : IEqualityComparer<Dictionary<string, string>>
		{
			public bool Equals(Dictionary<string, string> x, Dictionary<string, string> y)
			{
				if (x.Count != y.Count)
					return false;

				foreach (var pair in x)
				{
					if (!y.ContainsKey(pair.Key) || y[pair.Key] != pair.Value)
						return false;
				}
				return true;
			}

			public int GetHashCode(Dictionary<string, string> obj)
			{
				return 0;// obj.GetHashCode();// Values.Last().GetHashCode();//hack (sum of them all overflowed... it's just a hash)
			}
		}


	}
}