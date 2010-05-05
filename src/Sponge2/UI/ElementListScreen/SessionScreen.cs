using System;
using Sponge2.Model;
using Sponge2.Properties;
using Sponge2.UI.ProjectWindow;

namespace Sponge2.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionScreen : ConcreteSessionScreen, ISpongeView
	{
		public SessionScreen(ElementListViewModel<Session> presentationModel)
			: base(presentationModel)
		{
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_tabComponentEditors, _componentFileGrid, _sessionsListPanel);
			_componentFileGrid.InitializeColumnWidths("SessionScreen");
		}

		/// ------------------------------------------------------------------------------------
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);

			if (firstTime)
			{
				if (Settings.Default.SessionScreenElementsListSplitterPos > 0)
					_elementListSplitter.SplitterDistance = Settings.Default.SessionScreenElementsListSplitterPos;

				if (Settings.Default.SessionScreenComponentsSplitterPos > 0)
					_componentsSplitter.SplitterDistance = Settings.Default.SessionScreenComponentsSplitterPos;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Settings.Default.SessionScreenElementsListSplitterPos = _elementListSplitter.SplitterDistance;
			Settings.Default.SessionScreenComponentsSplitterPos = _componentsSplitter.SplitterDistance;
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
