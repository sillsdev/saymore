using System;
using Sponge2.Model;

namespace Sponge2.UI.ElementListScreen
{
	public partial class SessionScreen : ConcreteSessionScreen
	{
		public SessionScreen(ElementListViewModel<Session> presentationModel)
			: base(presentationModel)
		{
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentEditorsTabControl, _componentGrid, _sessionsListPanel);
		}
	}

	/// <summary>
	/// This class is used to overcome a limitation in the VS 2008 designer:
	/// not only can it not design a generic class, but it cannot even design a class which
	/// directly inhertis from a generic class! So we have this intermediate class.
	/// </summary>
	public class ConcreteSessionScreen : ElementListScreen<Session>
	{
		//design time only
		private ConcreteSessionScreen()
			: base(null)
		{}

		public ConcreteSessionScreen(ElementListViewModel<Session> presentationModel)
			: base(presentationModel)
		{}
	}
}
