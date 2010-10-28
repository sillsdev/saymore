using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.NewEventsFromFiles;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventScreen : ConcreteEventScreen, ISayMoreView
	{
		private AudioVideoDataGatherer _audioVideoDataGatherer;
		private readonly NewEventsFromFileDlgViewModel.Factory _newEventsFromFileDlgViewModel;

		/// ------------------------------------------------------------------------------------
		public EventScreen(ElementListViewModel<Event> presentationModel,
			NewEventsFromFileDlgViewModel.Factory newEventsFromFileDlgViewModel,
			EventsGrid.Factory eventGridFactory, AudioVideoDataGatherer avGatherer)
			: base(presentationModel)
		{
			_audioVideoDataGatherer = avGatherer;
			_elementsGrid = eventGridFactory();
			_elementsGrid.Name = "EventsGrid";
			_newEventsFromFileDlgViewModel = newEventsFromFileDlgViewModel;
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentsSplitter.Panel2, _eventComponentFileGrid, _eventsListPanel);
			_eventComponentFileGrid.InitializeGrid("EventScreen");

			_elementsListPanel.InsertButton(1, _buttonNewFromFiles);

			if (_componentsSplitter.Panel2.Controls.Count > 1)
				_labelHelp.Visible = false;
			else
				_componentsSplitter.Panel2.ControlAdded += HandleFirstSetOfComponentEditorsAdded;

			_componentsSplitter.Panel2.ControlRemoved += HandleLastSetOfComponentEditorsRemoved;

			_elementsListPanel.ButtonPanelBackColor1 = Settings.Default.EventEditorsButtonBackgroundColor1;
			_elementsListPanel.ButtonPanelBackColor2 = Settings.Default.EventEditorsButtonBackgroundColor2;
			_elementsListPanel.ButtonPanelTopBorderColor = Settings.Default.EventEditorsBorderColor;

			_elementsListPanel.HeaderPanelBackColor1 = Settings.Default.EventEditorsButtonBackgroundColor1;
			_elementsListPanel.HeaderPanelBackColor2 = Settings.Default.EventEditorsButtonBackgroundColor2;
			_elementsListPanel.HeaderPanelBottomBorderColor = Settings.Default.EventEditorsBorderColor;
		}

		/// ------------------------------------------------------------------------------------
		void HandleFirstSetOfComponentEditorsAdded(object sender, ControlEventArgs e)
		{
			_componentsSplitter.Panel2.ControlAdded -= HandleFirstSetOfComponentEditorsAdded;
			_labelHelp.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		void HandleLastSetOfComponentEditorsRemoved(object sender, ControlEventArgs e)
		{
			if (_componentsSplitter.Panel2.Controls.Count == 1)
			{
				_labelHelp.Visible = true;
				_componentsSplitter.Panel2.ControlAdded += HandleFirstSetOfComponentEditorsAdded;
			}
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

				// This code makes sure that the file info. gathering is at least finished for
				// the media files in the current event's folder so the grid has all the info.
				// it needs right away.
				_audioVideoDataGatherer.SuspendProcessing();

				if (_model.SelectedElement != null)
					_audioVideoDataGatherer.ProcessAllFilesInFolder(_model.SelectedElement.FolderPath);

				_audioVideoDataGatherer.ResumeProcessing(true);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return "Events"; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return Resources.Events; }
		}

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
