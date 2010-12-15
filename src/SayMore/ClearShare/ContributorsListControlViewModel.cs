using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;

namespace SayMore.ClearShare
{
	public class ContributorsListControlViewModel
	{
		public event EventHandler NewContributionListAvailable;

		private readonly OlacSystem _olacSystem = new OlacSystem();
		private readonly AutoCompleteValueGatherer _autoCompleteProvider;

		public ContributionCollection Contributions { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public ContributorsListControlViewModel(AutoCompleteValueGatherer autoCompleteProvider)
		{
			_autoCompleteProvider = autoCompleteProvider;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Role> OlacRoles
		{
			get { return _olacSystem.GetRoles(); }
		}

		/// ------------------------------------------------------------------------------------
		public void SetContributionList(ContributionCollection list)
		{
			Contributions = (list ?? new ContributionCollection());

			if (NewContributionListAvailable != null)
				NewContributionListAvailable(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteStringCollection GetAutoCompleteNames()
		{
			var list = (_autoCompleteProvider != null ?
				_autoCompleteProvider.GetValuesForKey("person") : new List<string>(0));

			var autoCompleteValues = new AutoCompleteStringCollection();
			autoCompleteValues.AddRange(list.ToArray());
			return autoCompleteValues;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetCanDeleteContribution(int index)
		{
			return (Contributions != null && index >= 0 && index < Contributions.Count);
		}

		/// ------------------------------------------------------------------------------------
		public Contribution GetContributionCopy(int index)
		{
			return (index < 0 || index >= Contributions.Count ?
				null : Contributions[index].Clone() as Contribution);
		}

		/// ------------------------------------------------------------------------------------
		public object GetContributionValue(int index, string valueName)
		{
			if (index >= 0 && index < Contributions.Count)
			{
				switch (valueName)
				{
					default: return null;
					case "name": return Contributions[index].ContributorName;
					case "comments": return Contributions[index].Comments;

					case "role":
						if (Contributions[index].Role != null)
							return Contributions[index].Role.Name;
						break;

					case "date":
						DateTime date;
						if (DateTime.TryParse(Contributions[index].Date, out date))
							return date;
						break;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public void SetContributionValue(int index, string valueName, object value)
		{
			if (index < 0 || index > Contributions.Count)
				return;

			if (index == Contributions.Count)
				Contributions.Add(new Contribution());

			switch (valueName)
			{
				default: break;
				case "name": Contributions[index].ContributorName = value as string; break;
				case "comments": Contributions[index].Comments = value as string; break;

				case "role":
					Role role;
					if (_olacSystem.TryGetRoleByName(value as string, out role))
						Contributions[index].Role = role;
					break;

				case "date":
					if (value != null && value.GetType() == typeof(DateTime))
						Contributions[index].Date = ((DateTime)value).ToShortDateString();
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool DeleteContribution(int index)
		{
			if (index < 0 || index >= Contributions.Count)
				return false;

			Contributions.RemoveAt(index);
			return true;
		}
	}
}
