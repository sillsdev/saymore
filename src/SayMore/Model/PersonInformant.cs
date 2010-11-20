using System;
using System.Collections.Generic;
using System.Linq;

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

		[Obsolete("For mocking only")]
		public PersonInformant(){}

		/// ------------------------------------------------------------------------------------
		public PersonInformant(ElementRepository<Person> peopleRepository)
		{
			_peopleRepository = peopleRepository;
			_peopleRepository.ElementIdChanged += HandlePersonNameChanged;
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
		public virtual IEnumerable<string> GetPeopleNames()
		{
			return _peopleRepository.AllItems.Select(x => x.Id);
		}
	}
}
