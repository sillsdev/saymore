using System;
using System.Collections.Generic;
using System.Linq;
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

		public Session SelectedSession { get; set; }

		public IEnumerable<ComponentFile> ComponentsOfCurrentSession
		{
			get
			{
				if(SelectedSession == null)
				{
					return new List<ComponentFile>();
				}

				return SelectedSession.GetComponentFiles();
			}
		}

		public void CreateNewSession()
		{
			var id = "XYZ-"+_repository.AllItems.Count().ToString();
			_repository.CreateNew(id);
		}
	}
}