using System;
using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.Model
{
	/// <summary>
	/// I couldn't  think of a good name... the idea is to put stuff for the session in here, so as to keep
	/// knowledge of people somewhat separate from Session
	/// </summary>
	public class PersonInformant
	{
		public event EventHandler<ElementIdChangedArgs> PersonNameChanged;
		public event EventHandler<ElementIdChangedArgs> PersonUiIdChanged;

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
		protected void HandlePersonNameChanged(object sender, ElementIdChangedArgs args)
		{
			// If the new "ID" is the real underlying ID of a person, then this was an actual
			// name change (or the "code"was removed and this can be treated as a legitimate name
			// change).
			var person = _peopleRepository.GetById(args.NewId);
			if (person != null)
			{
				if (PersonNameChanged != null)
					PersonNameChanged(this, args);
			}

			if (PersonUiIdChanged != null)
				PersonUiIdChanged(this, args);
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
				_autoCompleteValueGatherer.GetValueLists(false).Where(x => x.Key == "person").ToArray());

			if (gathererNames == null || gathererNames.Length == 0)
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
			// if the code value is present, use it instead of the full name
			//return _peopleRepository.AllItems.Select(x => x.Id);

			// SP-796: Session.People not updating correctly when Person.Code changes
			//return _peopleRepository.AllItems.Select(x => ((x.MetaDataFile != null) ? x.MetaDataFile.GetStringValue(PersonFileType.kCode, null) : null) ?? x.Id);
			var names = new List<string>();
			foreach (var person in _peopleRepository.AllItems)
			{
				if (person.MetaDataFile == null)
				{
					names.Add(person.Id);
				}
				else
				{
					var n = person.MetaDataFile.GetStringValue(PersonFileType.kCode, null);
					names.Add(!string.IsNullOrEmpty(n) ? n : person.Id);
				}
			}

			return names;
		}

		/// ------------------------------------------------------------------------------------
		public virtual Person GetPersonByNameOrCode(string nameOrCode)
		{
			return _peopleRepository.GetById(nameOrCode) ?? _peopleRepository.GetByField(PersonFileType.kCode, nameOrCode);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetUiIdByName(string name)
		{
			var person = _peopleRepository.GetById(name);
			return person != null ? person.UiId : name;
		}
	}
}
