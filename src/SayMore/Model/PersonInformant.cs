using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SayMore.Model
{
	/// <summary>
	/// I couldn't  think of a good name... the idea is to put stuff for the event in here, so as to keep
	/// knowledge of people somewhat seperate from Event
	/// </summary>
	public class PersonInformant
	{
		private readonly ElementRepository<Person> _peopleRepository;

		public PersonInformant(ElementRepository<Person> peopleRepository)
		{
			_peopleRepository = peopleRepository;
		}

		[Obsolete("FOr mocking only")]
		public PersonInformant(){}

		public virtual bool GetHasInformedConsent(string personName)
		{
			var person = _peopleRepository.GetById(personName);
			//Review:  if we have this error at runtime, just say false
			if(person==null)
			{
				return false;
			}
			return person.GetInformedConsentComponentFile() != null;
		}
	}
}
