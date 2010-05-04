using System;
using Sponge2.Model;

namespace Sponge2.UI.ElementListScreen
{
	public partial class PersonListScreen : ConcretePersonListScreen
	{
		public PersonListScreen(ElementListViewModel<Person> presentationModel)
			: base(presentationModel)
		{
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentEditorsTabControl, _componentFileGrid, _sessionsListPanel);
		}
	}

	/// <summary>
	/// This class is used to overcome a limitation in the VS 2008 designer:
	/// not only can it not design a generic class, but it cannot even design a class which
	/// directly inhertis from a generic class! So we have this intermediate class.
	/// </summary>
	public class ConcretePersonListScreen : ElementListScreen<Person>
	{
		//design time only
		private ConcretePersonListScreen()
			: base(null)
		{}

		public ConcretePersonListScreen(ElementListViewModel<Person> presentationModel)
			: base(presentationModel)
		{}
	}
}
