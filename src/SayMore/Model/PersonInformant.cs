using System;
using System.Collections.Generic;
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
	}
}
