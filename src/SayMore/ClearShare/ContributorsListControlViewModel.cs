using System;
using System.Collections.Generic;

namespace SayMore.ClearShare
{
	public class ContributorsListControlViewModel
	{
		public event EventHandler NewContributionListAvailable;

		private readonly OlacSystem _olacSystem = new OlacSystem();
		private Work _work;
		private List<Contribution> _contributions;

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Contribution> Contributions
		{
			get { return _contributions; }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Role> OlacRoles
		{
			get { return _olacSystem.GetRoles(); }
		}

		/// ------------------------------------------------------------------------------------
		public void SetWorkFromXML(string xml)
		{
			_work = new Work();
			if (!string.IsNullOrEmpty(xml))
				_olacSystem.LoadWorkFromXml(_work, xml);

			_contributions = _work.Contributions;

			if (NewContributionListAvailable != null)
				NewContributionListAvailable(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public string GetXMLFromWork()
		{
			return _olacSystem.GetXmlForWork(_work);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetCanDeleteContribution(int index)
		{
			return (_contributions != null && index >= 0 && index < _contributions.Count);
		}

		/// ------------------------------------------------------------------------------------
		public Contribution GetContributionCopy(int index)
		{
			return (index < 0 || index >= _contributions.Count ?
				null : _contributions[index].Clone());
		}

		/// ------------------------------------------------------------------------------------
		public object GetContributionValue(int index, string valueName)
		{
			if (index >= 0 && index < _contributions.Count)
			{
				switch (valueName)
				{
					default: return null;
					case "name": return _contributions[index].ContributorName;
					case "notes": return _contributions[index].Notes;

					case "role":
						if (_contributions[index].Role != null)
							return _contributions[index].Role.Name;
						break;

					case "date":
						DateTime date;
						if (DateTime.TryParse(_contributions[index].Date, out date))
							return date;
						break;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public void SetContributionValue(int index, string valueName, object value)
		{
			if (index < 0 || index > _contributions.Count)
				return;

			if (index == _contributions.Count)
				_contributions.Add(new Contribution());

			switch (valueName)
			{
				default: break;
				case "name": _contributions[index].ContributorName = value as string; break;
				case "notes": _contributions[index].Notes = value as string; break;

				case "role":
					Role role;
					if (_olacSystem.TryGetRoleByName(value as string, out role))
						_contributions[index].Role = role;
					break;

				case "date":
					if (value != null && value.GetType() == typeof(DateTime))
						_contributions[index].Date = ((DateTime)value).ToShortDateString();
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool DeleteContribution(int index)
		{
			if (index < 0 || index >= _contributions.Count)
				return false;

			_contributions.RemoveAt(index);
			return true;
		}
	}
}
