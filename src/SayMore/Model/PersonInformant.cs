using System;
using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Files.DataGathering;

namespace SayMore.Model
{
	/// <summary>
	/// I couldn't  think of a good name... the idea is to put stuff for the event in here, so as to keep
	/// knowledge of people somewhat seperate from Event
	/// </summary>
	public class PersonInformant
	{
		public event ElementRepository<Person>.ElementIdChangedHandler PersonNameChanged;

		private readonly ElementRepository<Person> _peopleRepository;
		private readonly AutoCompleteValueGatherer _autoCompleteValueGatherer;

		[Obsolete("For mocking only")]
		public PersonInformant(){}

		/// ------------------------------------------------------------------------------------
		public PersonInformant(ElementRepository<Person> peopleRepository,
			AutoCompleteValueGatherer autoCompleteValueGatherer)
		{
			_peopleRepository = peopleRepository;
			_peopleRepository.ElementIdChanged += HandlePersonNameChanged;
			_autoCompleteValueGatherer = autoCompleteValueGatherer;
		}

		/// ------------------------------------------------------------------------------------
		public int NumberOfPeople
		{
			get { return _peopleRepository.AllItems.Count(); }
		}

		/// ------------------------------------------------------------------------------------
		protected void HandlePersonNameChanged(ProjectElement element, string oldId, string newId)
		{
			if (PersonNameChanged != null)
				PersonNameChanged(element, oldId, newId);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetHasInformedConsent(string personName)
		{
			var person = _peopleRepository.GetById(personName);

			//Review:  if we have this error at runtime, just say false
			return (person == null ? false : person.GetInformedConsentComponentFile() != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets names of all people found, whether or not their is an entry for the person
		/// on the Person tab of the program. This method uses the people names found by the
		/// auto-complete value gatherer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetAllPeopleNames()
		{
			var gathererNames = (_autoCompleteValueGatherer == null ? null :
				_autoCompleteValueGatherer.GetValueLists(false).Where(x => x.Key == "person"));

			if (gathererNames == null || gathererNames.Count() == 0)
			{
				foreach (var name in GetPeopleNamesFromRepository())
					yield return name;
			}
			else
			{
				foreach (var person in gathererNames.SelectMany(kvp => kvp.Value))
					yield return person;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets only those people for whom there is an entry on the Person tab of the program.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<Person> GetPeopleFromRepository()
		{
			return _peopleRepository.AllItems;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets only those people for whom there is an entry on the Person tab of the program.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetPeopleNamesFromRepository()
		{
			return _peopleRepository.AllItems.Select(x => x.Id);
		}

		/// ------------------------------------------------------------------------------------
		public Person GetPersonByName(string name)
		{
			return _peopleRepository.GetById(name);
		}
	}
}
