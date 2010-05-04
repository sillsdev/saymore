using Sponge2.Model;
using Sponge2.Properties;
using Sponge2.UI.ProjectWindow;

namespace Sponge2.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class PersonListScreen : ConcretePersonListScreen, ISpongeView
	{
		public PersonListScreen(ElementListViewModel<Person> presentationModel)
			: base(presentationModel)
		{
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentEditorsTabControl, _componentFileGrid, _sessionsListPanel);
		}

		/// ------------------------------------------------------------------------------------
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);

			if (firstTime)
			{
				if (Settings.Default.PersonScreenElementListSpiltterPos > 0)
					_elementListSplitter.SplitterDistance = Settings.Default.PersonScreenElementListSpiltterPos;

				if (Settings.Default.PersonScreenComponentsSplitterPos > 0)
					_componentsSplitter.SplitterDistance = Settings.Default.PersonScreenComponentsSplitterPos;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.PersonScreenElementListSpiltterPos = _elementListSplitter.SplitterDistance;
			Settings.Default.PersonScreenComponentsSplitterPos = _componentsSplitter.SplitterDistance;
			base.OnHandleDestroyed(e);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is used to overcome a limitation in the VS 2008 designer:
	/// not only can it not design a generic class, but it cannot even design a class which
	/// directly inhertis from a generic class! So we have this intermediate class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
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
