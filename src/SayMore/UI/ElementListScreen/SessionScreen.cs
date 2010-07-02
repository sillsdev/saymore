using System;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Properties;
using SayMore.UI.NewSessionsFromFiles;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class SessionScreen : ConcreteSessionScreen, ISayMoreView
	{
		private readonly NewSessionsFromFileDlgViewModel.Factory _newSessionsFromFileDlgViewModel;

		/// ------------------------------------------------------------------------------------
		public SessionScreen(ElementListViewModel<Session> presentationModel,
			NewSessionsFromFileDlgViewModel.Factory newSessionsFromFileDlgViewModel)
			: base(presentationModel)
		{
			_newSessionsFromFileDlgViewModel = newSessionsFromFileDlgViewModel;
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentsSplitter.Panel2, _componentFileGrid, _sessionsListPanel);
			_componentFileGrid.InitializeGrid("SessionScreen");

			_sessionsListPanel.InsertButton(1, _buttonNewFromFiles);
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
		public override string Text
		{
			get { return "Sessions"; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public System.Drawing.Image Image
		{
			get { return Resources.Sessions; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Settings.Default.SessionScreenElementsListSplitterPos = _elementListSplitter.SplitterDistance;
			Settings.Default.SessionScreenComponentsSplitterPos = _componentsSplitter.SplitterDistance;
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonNewFromFilesClick(object sender, EventArgs e)
		{
			using (var viewModel = _newSessionsFromFileDlgViewModel(_model))
			using (var dlg = new NewSessionsFromFilesDlg(viewModel))
			{
				viewModel.Dialog = dlg;

				if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
					LoadElementList(viewModel.FirstNewSessionAdded);

				FindForm().Focus();
			}
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
