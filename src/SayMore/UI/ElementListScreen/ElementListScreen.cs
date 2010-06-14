using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Properties;
using SIL.Localization;
using SilUtils;
using SayMore.Model;
using SayMore.UI.ComponentEditors;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is the base class for both People and Session screens.
	///
	/// Review for later: Some alternate ways to approach this:
	///
	/// * Separate the 3 main areas of these screens into separate controls, each
	/// with their own view model as needed. This way the two screens could be more
	/// naturally customized as needed.
	///
	/// * Move away from knowing about the generics
	/// at this level, and instead take an IElementListViewModel. Leave it to the DI to
	/// give us the right one.  That might have been an easier approach than what I've
	/// done here.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ElementListScreen<T> : UserControl where T : ProjectElement
	{
		protected readonly ElementListViewModel<T> _model;

		protected TabControl _componentEditorsTabControl;
		protected ListPanel _elementsListPanel;
		private ComponentFileGrid _componentFilesControl;

		/// ------------------------------------------------------------------------------------
		public ElementListScreen(ElementListViewModel<T> presentationModel)
		{
			_model = presentationModel;
		}

		/// ------------------------------------------------------------------------------------
		protected void Initialize(TabControl componentEditorsTabControl,
			ComponentFileGrid componentGrid, ListPanel elementsListPanel)
		{
			_componentEditorsTabControl = componentEditorsTabControl;
			_componentEditorsTabControl.TabPages.Clear();

			var imgList = new ImageList();
			imgList.ColorDepth = ColorDepth.Depth32Bit;
			imgList.ImageSize = Resources.PlayTabImage.Size;
			//imgList.Images.Add("Contributors", Resources.ContributorsTabImage);
			imgList.Images.Add("Notes", Resources.NotesTabImage);
			imgList.Images.Add("Play", Resources.PlayTabImage);
			imgList.Images.Add("Person", Resources.PersonComponentFileImage);
			imgList.Images.Add("Session", Resources.SessionComponentFileImage);
			imgList.Images.Add("Technical", Resources.TechnicalTabImage);
			imgList.Images.Add("View", Resources.ViewTabImage);
			_componentEditorsTabControl.ImageList = imgList;

			_elementsListPanel = elementsListPanel;
			_elementsListPanel.ReSortWhenItemTextChanges = true;

			_componentFilesControl = componentGrid;
			_componentFilesControl.ComponentSelectedCallback = HandleComponentSelectedCallback;
			_componentFilesControl.FilesAdded = HandleFilesAddedToComponentGrid;
			_componentFilesControl.FilesBeingDraggedOverGrid = HandleFilesBeingDraggedOverComponentGrid;
			_componentFilesControl.FilesDroppedOnGrid = HandleFilesAddedToComponentGrid;

			_elementsListPanel.NewButtonClicked += HandleNewElementButtonClicked;
			_elementsListPanel.BeforeItemsDeleted += HandleBeforeElementsDeleted;
			_elementsListPanel.AfterItemsDeleted += HandleElementsDeleted;
			_elementsListPanel.SelectedItemChanged += HandleSelectedElementChanged;

			_componentEditorsTabControl.Selecting += HandleSelectedComponentEditorTabSelecting;

			_componentEditorsTabControl.Font = SystemFonts.IconTitleFont;
			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called by the component file grid when the user chooses a different file
		/// </summary>
		//review: why use index, why not the object?
		// Answser: If the object is used, the caller of this delegate would have to get the object
		// this way: _model.GetComponentFile(index). Using the index here is really just
		// passing off to the model the inevitable job of indexing into the component file list.
		// The grid (i.e. the only object calling this delegate so far) does not keep a
		// reference to each component files that it can pass to this delegate.
		/// ------------------------------------------------------------------------------------
		private void HandleComponentSelectedCallback(int index)
		{
			_model.SetSelectedComponentFile(index);
			UpdateComponentEditors();
		}

		/// ------------------------------------------------------------------------------------
		private DragDropEffects HandleFilesBeingDraggedOverComponentGrid(string[] files)
		{
			var validFiles = _model.SelectedElement.RemoveInvalidFilesFromProspectiveFilesToAdd(files);
			return (validFiles.Count() > 0 ? DragDropEffects.Copy : DragDropEffects.None);
		}

		/// ------------------------------------------------------------------------------------
		private bool HandleFilesAddedToComponentGrid(string[] files)
		{
			if (_model.SelectedElement.AddComponentFiles(files))
			{
				UpdateComponentList();
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadElementList()
		{
			LoadElementList(null);
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadElementList(string itemToSelectAfterLoad)
		{
			_elementsListPanel.Clear();
			var elements = _model.Elements.Cast<object>().ToList();
			_elementsListPanel.AddRange(elements);

			if (elements.Count == 0)
				return;

			if (itemToSelectAfterLoad == null)
				_elementsListPanel.SelectItem(elements[0], true);
			else
				_elementsListPanel.SelectItem(itemToSelectAfterLoad, true);
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateDisplay()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentList()
		{
			var componentsOfSelectedElement = _model.GetComponentsOfSelectedElement();
			_componentFilesControl.UpdateComponentList(componentsOfSelectedElement);
			_model.SetSelectedComponentFile(0);

			foreach (var componentFile in componentsOfSelectedElement)
			{
				componentFile.UiShouldRefresh -= HandleUiShouldRefresh;
				componentFile.UiShouldRefresh += HandleUiShouldRefresh;
				//review: and later, are we wired longer than we want to be?
			}

			_model.SetSelectedComponentFile(0);
			_componentFilesControl.SelectComponent(0);
			UpdateComponentEditors();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// At the moment, this doesn't distinguish between events which would cause
		/// the element list to need updating (e.g., person name column or file duration
		/// column or some such) vs. things that just require that the component list update.
		///
		/// This is called when the Component File raises this event, in response to the user
		/// doing something like changing a person's name, or a session's id
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleUiShouldRefresh(object sender, EventArgs e)
		{
			_elementsListPanel.RefreshTextOfCurrentItem(true);
			UpdateComponentList();
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentEditors()
		{
			Utils.SetWindowRedraw(_componentEditorsTabControl, false);

			_componentEditorsTabControl.Selecting -= HandleSelectedComponentEditorTabSelecting;

			if (_model.SelectedElement == null)
			{
				_componentEditorsTabControl.TabPages.Clear();
				_componentFilesControl.AddButtonEnabled = false;
			}
			else
			{
				// Remove all but one tab page because removing all of them
				// will steal the focus from the active control. Go figure.
				for (int i = _componentEditorsTabControl.TabCount - 1; i > 0; i--)
					_componentEditorsTabControl.TabPages.RemoveAt(i);

				_componentFilesControl.AddButtonEnabled = true;
			}

			int providerCount = 0;

			// At this point, just make tabs and name them. A tab's editor
			// controls will be built only when the user selects the tab.
			foreach (var provider in _model.GetComponentEditorProviders())
			{
				ComponentEditorTabPage page;

				if (providerCount++ == 0 && _componentEditorsTabControl.TabCount > 0)
				{
					page = _componentEditorsTabControl.TabPages[0] as ComponentEditorTabPage;
					page.SetProvider(provider);
					page.LoadEditorControl(_model.SelectedComponentFile);
				}
				else
				{
					page = new ComponentEditorTabPage(provider);
					_componentEditorsTabControl.TabPages.Add(page);
				}
			}

			_componentEditorsTabControl.Selecting += HandleSelectedComponentEditorTabSelecting;
			Utils.SetWindowRedraw(_componentEditorsTabControl, true);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool HandleBeforeElementsDeleted(object sender, IEnumerable<object> itemsToDelete)
		{
			int itemCount = itemsToDelete.Count();

			var msg = (itemCount == 1 ?
				LocalizationManager.LocalizeString("Misc. Messages.DeleteOneItemMsg", "Are you sure you would like to delete 1 item?") :
				LocalizationManager.LocalizeString("Misc. Messages.DeleteMultipleItemsMsg", "Are you sure you would like to delete {0} items?"));

			msg = string.Format(msg, itemCount);
			return (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleElementsDeleted(object sender, IEnumerable<object> itemsToDelete)
		{
			foreach (var item in itemsToDelete)
				_model.Remove(item as T);

			if (_elementsListPanel.CurrentItem != null)
				LoadElementList(_elementsListPanel.CurrentItem.ToString());
			else
			{
				_model.SetSelectedElement(null);
				UpdateComponentList();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual object HandleNewElementButtonClicked(object sender)
		{
			_model.SetSelectedElement(_model.CreateNewElement());
			_elementsListPanel.AddItem(_model.SelectedElement, true, true);
			return null;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleSelectedElementChanged(object sender, object newItem)
		{
			_model.SetSelectedElement(newItem as T);
			UpdateComponentList();
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleSelectedComponentEditorTabSelecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Selecting)
			{
				((ComponentEditorTabPage)e.TabPage).LoadEditorControl(_model.SelectedComponentFile);
				((ComponentEditorTabPage)e.TabPage).EditorProvider.Control.Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ViewActivated(bool firstTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ViewDeactivated()
		{
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}
	}
}
