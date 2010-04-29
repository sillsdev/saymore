using System.Collections.Generic;
using Sponge2.Model;

namespace Sponge2.UI
{
	public class SessionsViewModel
	{
		private readonly ElementRepository<Session> _repository;

		public SessionsViewModel(ElementRepository<Session> repository)
		{
			_repository = repository;
		}

		public IEnumerable<Session> Sessions
		{
			get
			{
				return _repository.AllItems;
			}
		}
	}
}