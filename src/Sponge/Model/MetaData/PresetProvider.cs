using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIL.Sponge.Model.MetaData
{
	//Gets all the metadata settings found in the whole project, for the purpose of automatically making presets
	public class PresetProvider
	{
		private readonly List<Dictionary<string, string>> _instances;
		private UniqueCombinationsFinder _suggestor;

		private PresetProvider(List<Dictionary<string, string>> instances)
		{
			_instances = instances;
		}

		public static PresetProvider CreateFromDirectory(string sessionsDirectoryPath)
		{
			var allMetaDataSets = new List<Dictionary<string, string>>();
			foreach (var sessionDirectoryPath in Directory.GetDirectories(sessionsDirectoryPath))
			{
				foreach (var path in Directory.GetFiles(sessionDirectoryPath))
				{
					var file = SessionFile.Create(path);
					Dictionary<string, string> dictionary = file.GetMetaDataDictionary();
					if(dictionary.Count>0)
						allMetaDataSets.Add(dictionary);
				}
			}
			return new PresetProvider(allMetaDataSets);
		}

		public static PresetProvider CreateFromTestArray(string[] set)
		{
			var instances = new List<Dictionary<string, string>>();
			foreach (var v in set)
			{
				if (v.Trim() == "")
					continue;
				var fields = new Dictionary<string, string>();
				foreach (string field in v.Split(new char[] { ',' }))
				{
					if (field.Trim() == "")
						continue;
					var x = field.Split(new char[] { '=' });
					fields.Add(x[0], x[1]);
				}
				instances.Add(fields);
			}
			return new PresetProvider(instances);
		}

		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetSuggestions()
		{
			if (_suggestor == null)//this could take some time, so don't make it until we have to
				_suggestor = new UniqueCombinationsFinder(_instances);
			return _suggestor.GetSuggestions();
		}



		/*This class was so hard to name! Its job is simply to provide 0 or more suggestions for collections of values to assign
 * to media files.  It does that by looking at the values which have been used so far, and sorting them
 * by frequency.
 */
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
					label += value + ", ";
				}
				return label.Trim(new char[] { ',' });
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
}
