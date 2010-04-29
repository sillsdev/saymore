using System;
using System.Collections.Generic;
using System.Linq;
using Sponge2.Model;

namespace Sponge2.UI
{
	public class SessionsViewModel
	{
		private readonly ElementRepository<Session> _repository;

		public Session SelectedSession { get; private set; }
		public ComponentFile SelectedComponentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public SessionsViewModel(ElementRepository<Session> repository)
		{
			_repository = repository;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Session> Sessions
		{
			get
			{
				return _repository.AllItems;
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentFile> ComponentsOfSelectedSession
		{
			get
			{
				if (SelectedSession == null)
				{
					return new List<ComponentFile>(0);
				}

				return SelectedSession.GetComponentFiles();
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool SetSelectedSession(Session session)
		{
			if (SelectedSession == session)
				return false;

			SelectedSession = session;
			SetSelectedComponentFile(0);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetSelectedComponentFile(int index)
		{
			var componentFiles = SelectedSession.GetComponentFiles();

			if (index < 0 || index >= componentFiles.Length)
				return false;

			SelectedComponentFile = componentFiles[index];
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFile GetComponentFile(int index)
		{
			// We probably don't want to do this everytime because this method will be called
			// many times as the grid in the UI is being displayed.
			var componentFiles = SelectedSession.GetComponentFiles();
			return componentFiles[index];
		}

		/// ------------------------------------------------------------------------------------
		public Session CreateNewSession()
		{
			var id = "XYZ-" + _repository.AllItems.Count();
			return _repository.CreateNew(id);
		}
	}
}