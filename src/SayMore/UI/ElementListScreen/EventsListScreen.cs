using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Media;
using SayMore.Model;
using SayMore.Properties;
using SayMore.UI.EventRecording;
using SayMore.UI.NewEventsFromFiles;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventsListScreen : ConcreteEventScreen, ISayMoreView
	{
		private readonly NewEventsFromFileDlgViewModel.Factory _newEventsFromFileDlgViewModel;

		/// ------------------------------------------------------------------------------------
		public EventsListScreen(ElementListViewModel<Event> presentationModel,
			NewEventsFromFileDlgViewModel.Factory newEventsFromFileDlgViewModel,
			EventsGrid.Factory eventGridFactory)
			: base(presentationModel)
		{
			_elementsGrid = eventGridFactory();
			_elementsGrid.Name = "EventsGrid";
			_newEventsFromFileDlgViewModel = newEventsFromFileDlgViewModel;
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentsSplitter.Panel2, _eventComponentFileGrid, _eventsListPanel);
			_eventComponentFileGrid.InitializeGrid("EventScreen",
				LocalizationManager.GetString("EventsView.FileList.AddEventsButtonToolTip", "Add Files to the Event"));

			_elementsListPanel.InsertButton(1, _buttonNewFromFiles);
			_elementsListPanel.InsertButton(2, _buttonNewFromRecording);

			InitializeMenus();

			if (_componentsSplitter.Panel2.Controls.Count > 1)
				_labelClickNewHelpPrompt.Visible = false;
			else
				_componentsSplitter.Panel2.ControlAdded += HandleFirstSetOfComponentEditorsAdded;

			_componentsSplitter.Panel2.ControlRemoved += HandleLastSetOfComponentEditorsRemoved;

			_elementsListPanel.ButtonPanelBackColor1 = Settings.Default.EventEditorsButtonBackgroundColor1;
			_elementsListPanel.ButtonPanelBackColor2 = Settings.Default.EventEditorsButtonBackgroundColor2;
			_elementsListPanel.ButtonPanelTopBorderColor = Settings.Default.EventEditorsBorderColor;

			_elementsListPanel.HeaderPanelBackColor1 = Settings.Default.EventEditorsButtonBackgroundColor2;
			_elementsListPanel.HeaderPanelBackColor2 = Settings.Default.EventEditorsButtonBackgroundColor1;
			_elementsListPanel.HeaderPanelBottomBorderColor = Settings.Default.EventEditorsBorderColor;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeMenus()
		{
			MainMenuItem = new ToolStripMenuItem();
			MainMenuItem.Text = LocalizationManager.GetString(
				"EventsView.EventsMainMenu.TopLevelMenuText", "&Event", null, MainMenuItem);

			var menu = new ToolStripMenuItem();
			MainMenuItem.DropDownItems.Add(menu);
			menu.Click += HandleAddingNewElement;
			menu.Text = LocalizationManager.GetString(
				"EventsView.EventsMainMenu.NewMenuText", "&New", null, menu);

			menu = new ToolStripMenuItem();
			MainMenuItem.DropDownItems.Add(menu);
			menu.Click += HandleButtonNewFromFilesClick;
			menu.Text = LocalizationManager.GetString(
				"EventsView.EventsMainMenu.NewFromDeviceMenuText", "New From De&vice...", null, menu);

			menu = new ToolStripMenuItem();
			MainMenuItem.DropDownItems.Add(menu);
			menu.Click += HandleButtonNewFromRecordingsClick;
			menu.Text = LocalizationManager.GetString(
				"EventsView.EventsMainMenu.NewFromRecordingMenuText", "New From &Recording...", null, menu);

			MainMenuItem.DropDownItems.Add(new ToolStripSeparator());

			foreach (var eventMenuItem in _elementsGrid.GetMenuCommands())
				MainMenuItem.DropDownItems.Add(eventMenuItem);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFirstSetOfComponentEditorsAdded(object sender, ControlEventArgs e)
		{
			_componentsSplitter.Panel2.ControlAdded -= HandleFirstSetOfComponentEditorsAdded;
			_labelClickNewHelpPrompt.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		void HandleLastSetOfComponentEditorsRemoved(object sender, ControlEventArgs e)
		{
			if (_componentsSplitter.Panel2.Controls.Count == 1)
			{
				_labelClickNewHelpPrompt.Visible = true;
				_componentsSplitter.Panel2.ControlAdded += HandleFirstSetOfComponentEditorsAdded;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleComponentFileSaved(object sender, EventArgs e)
		{
			base.HandleComponentFileSaved(sender, e);
			_elementsGrid.RefreshCurrentRow();
		}

		/// ------------------------------------------------------------------------------------
		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			var tab = viewTabGroup.AddTab(this);
			tab.Name = "EventsViewTab"; // for tests
			tab.Text = LocalizationManager.GetString("EventsView.ViewTabText", "Events", null, "Events View", null, tab);
			Text = tab.Text;
		}

		/// ------------------------------------------------------------------------------------
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);

			if (firstTime)
			{
				if (Settings.Default.EventScreenElementsListSplitterPos > 0)
					_elementListSplitter.SplitterDistance = Settings.Default.EventScreenElementsListSplitterPos;

				if (Settings.Default.EventScreenComponentsSplitterPos > 0)
					_componentsSplitter.SplitterDistance = Settings.Default.EventScreenComponentsSplitterPos;
			}
		}

		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return Resources.Events; }
		}

		/// ------------------------------------------------------------------------------------
		public ToolStripMenuItem MainMenuItem { get; private set; }

		/// ------------------------------------------------------------------------------------
		protected override Color ComponentEditorBackgroundColor
		{
			get { return Settings.Default.EventEditorsBackgroundColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ComponentEditorBorderColor
		{
			get { return Settings.Default.EventEditorsBorderColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Settings.Default.EventScreenElementsListSplitterPos = _elementListSplitter.SplitterDistance;
			Settings.Default.EventScreenComponentsSplitterPos = _componentsSplitter.SplitterDistance;
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonNewFromFilesClick(object sender, EventArgs e)
		{
			using (var viewModel = _newEventsFromFileDlgViewModel(_model))
			using (var dlg = new NewEventsFromFilesDlg(viewModel))
			{
				viewModel.Dialog = dlg;

				if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
					LoadElementList(viewModel.FirstNewEventAdded);

				FindForm().Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonNewFromRecordingsClick(object sender, EventArgs e)
		{
			if (!AudioUtils.WarnUserIfOSCannotRecord())
				return;

			using (var viewModel = new EventRecorderDlgViewModel())
			using (var dlg = new EventRecorderDlg(viewModel))
			{
				if (dlg.ShowDialog(FindForm()) != DialogResult.OK)
					return;

				var newEvent = _model.CreateNewElement();
				viewModel.MoveRecordingToEventFolder(newEvent);
				LoadElementList(newEvent);
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
	public class ConcreteEventScreen : ElementListScreen<Event>
	{
		//design time only
		private ConcreteEventScreen()
			: base(null)
		{}

		public ConcreteEventScreen(ElementListViewModel<Event> presentationModel)
			: base(presentationModel)
		{}
	}
}
