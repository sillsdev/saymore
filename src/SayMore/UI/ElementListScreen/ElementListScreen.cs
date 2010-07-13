using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Localization;
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

		protected TabControl _selectedEditorsTabControl;
		protected ListPanel _elementsListPanel;
		protected ComponentFileGrid _componentFilesControl;
		protected Control _tabControlHostControl;
		protected ImageList _tabControlImages;

		protected Dictionary<string, ComponentEditorsTabControl> _tabControls =
			new Dictionary<string, ComponentEditorsTabControl>();

		/// ------------------------------------------------------------------------------------
		public ElementListScreen(ElementListViewModel<T> presentationModel)
		{
			_model = presentationModel;
		}

		/// ------------------------------------------------------------------------------------
		protected void Initialize(Control tabControlHostControl,
			ComponentFileGrid componentGrid, ListPanel elementsListPanel)
		{
			_tabControlHostControl = tabControlHostControl;

			_tabControlImages = new ImageList();
			_tabControlImages.ColorDepth = ColorDepth.Depth32Bit;
			_tabControlImages.ImageSize = Resources.PlayTabImage.Size;
			//imgList.Images.Add("Contributors", Resources.ContributorsTabImage);
			_tabControlImages.Images.Add("Notes", Resources.NotesTabImage);
			_tabControlImages.Images.Add("Play", Resources.PlayTabImage);
			_tabControlImages.Images.Add("Person", Resources.PersonFileImage);
			_tabControlImages.Images.Add("Session", Resources.SessionFileImage);
			_tabControlImages.Images.Add("Image", Resources.ImageFileImage);
			_tabControlImages.Images.Add("Video", Resources.VideoFileImage);
			_tabControlImages.Images.Add("Audio", Resources.AudioFileImage);
			_tabControlImages.Images.Add("View", Resources.ViewTabImage);

			_elementsListPanel = elementsListPanel;
			_elementsListPanel.ReSortWhenItemTextChanges = true;

			_componentFilesControl = componentGrid;
			_componentFilesControl.AfterComponentSelected = HandleAfterComponentSelected;
			_componentFilesControl.AfterContextMenuItemChosen = HandleComponentFileContextMenuItemChosen;
			_componentFilesControl.FilesAdded = HandleFilesAddedToComponentGrid;
			_componentFilesControl.FilesBeingDraggedOverGrid = HandleFilesBeingDraggedOverComponentGrid;
			_componentFilesControl.FilesDroppedOnGrid = HandleFilesAddedToComponentGrid;

			_elementsListPanel.NewButtonClicked += HandleNewElementButtonClicked;
			_elementsListPanel.BeforeItemsDeleted += HandleBeforeElementsDeleted;
			_elementsListPanel.AfterItemsDeleted += HandleElementsDeleted;
			_elementsListPanel.SelectedItemChanged += HandleSelectedElementChanged;

			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called by the component file grid when the user chooses a different file
		/// </summary>
		/// review: why use index, why not the object?
		/// Answser: If the object is used, the caller of this delegate would have to get the object
		/// this way: _model.GetComponentFile(index). Using the index here is really just
		/// passing off to the model the inevitable job of indexing into the component file list.
		/// The grid (i.e. the only object calling this delegate so far) does not keep a
		/// reference to each component files that it can pass to this delegate.
		/// ------------------------------------------------------------------------------------
		private void HandleAfterComponentSelected(int index)
		{
			_model.MakeComponentEditorsGoDormant();
			_model.SetSelectedComponentFile(index);
			ShowSelectedComponentFileEditors();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileContextMenuItemChosen()
		{
			var currFile = _model.SelectedComponentFile.PathToAnnotatedFile;
			_model.RefreshSelectedElementComponentFileList();
			UpdateComponentFileList();
			_componentFilesControl.TrySetComponent(currFile);
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
			if (_model.AddComponentFiles(files))
			{
				UpdateComponentFileList();
				_componentFilesControl.TrySetComponent(files[0]);

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
		protected void UpdateComponentFileList()
		{
			var componentsOfSelectedElement = _model.GetComponentsOfSelectedElement();
			_componentFilesControl.AfterComponentSelected = null;
			_componentFilesControl.UpdateComponentFileList(componentsOfSelectedElement);
			_model.SetSelectedComponentFile(0);

			foreach (var componentFile in componentsOfSelectedElement)
			{
				componentFile.IdChanged -= HandleComponentFileIdChanged;
				componentFile.IdChanged += HandleComponentFileIdChanged;

				componentFile.MetadataValueChanged -= HandleComponentFileMetadataValueChanged;
				componentFile.MetadataValueChanged += HandleComponentFileMetadataValueChanged;

				//componentFile.UiShouldRefresh -= HandleUiShouldRefresh;
				//componentFile.UiShouldRefresh += HandleUiShouldRefresh;
				//review: and later, are we wired longer than we want to be?
			}

			_model.SetSelectedComponentFile(0);

			// Setting the selected component to nothing now will make sure that
			// setting it to zero below will cause a row changed event, thus causing
			// the ComponentSelectedCallback event.
			_componentFilesControl.SelectComponent(-1);
			_componentFilesControl.AfterComponentSelected = HandleAfterComponentSelected;
			_componentFilesControl.SelectComponent(0);
			ShowSelectedComponentFileEditors();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called when the Component File raises this event, in response to the user
		/// changing a person's name, or a session's id
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileIdChanged(ComponentFile file, string oldId, string newId)
		{
			_elementsListPanel.RefreshTextOfCurrentItem(true);
			_model.RefreshAfterIdChanged();
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleComponentFileMetadataValueChanged(ComponentFile file,
			string oldValue, string newValue)
		{
			_componentFilesControl.Refresh();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Each set of editors (i.e. a set being the editors associated with a single
		/// component file type) has its own tab control with a tab page for each editor.
		/// The tab controls are cached in a dictionary whose key is the file type name.
		/// The process of showing the editors for a component file is just a matter of
		/// hiding the tab control containing the previously selected file's editors and
		/// unhiding the tab control containing the selected file's editors.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void ShowSelectedComponentFileEditors()
		{
			var currProviderKey = _model.GetSelectedProviderKey();

			_componentFilesControl.AddButtonEnabled = (currProviderKey != null);
			if (currProviderKey == null)
				return;

			ComponentEditorsTabControl tabCtrl;
			if (!_tabControls.TryGetValue(currProviderKey, out tabCtrl))
			{
				tabCtrl = new ComponentEditorsTabControl(currProviderKey,
					_tabControlImages, _model.GetComponentEditorProviders(),
					ComponentEditorBackgroundColor, ComponentEditorBorderColor);

				tabCtrl.Selecting += HandleSelectedComponentEditorTabSelecting;
				_tabControls[currProviderKey] = tabCtrl;
				_tabControlHostControl.Controls.Add(tabCtrl);
			}

			// Don't do anything if the selected tab control hasn't changed.
			if (_selectedEditorsTabControl != tabCtrl)
			{
				if (_selectedEditorsTabControl != null)
					_selectedEditorsTabControl.Visible = false;

				tabCtrl.SelectedIndex = 0;
				tabCtrl.Visible = true;
				_selectedEditorsTabControl = tabCtrl;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Color ComponentEditorBackgroundColor
		{
			get { return SystemColors.Control; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Color ComponentEditorBorderColor
		{
			get { return SystemColors.ControlDark; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool HandleBeforeElementsDeleted(object sender, IEnumerable<object> itemsToDelete)
		{
			int itemCount = itemsToDelete.Count();

			var msg = (itemCount == 1 ?
				LocalizationManager.LocalizeString("Misc. Messages.DeleteOneItemMsg", "This will permanently remove 1 item?") :
				LocalizationManager.LocalizeString("Misc. Messages.DeleteMultipleItemsMsg", "This will permanently remove {0} items?"));

			msg = string.Format(msg, itemCount);
			return (DeleteMessageBox.Show(this, msg) == DialogResult.OK);
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

				if (_model.Elements.Count() == 0)
				{
					foreach (var tabCtrl in _tabControls.Values)
					{
						_tabControlHostControl.Controls.Remove(tabCtrl);
						tabCtrl.Dispose();
					}

					_tabControls.Clear();
				}

				UpdateComponentFileList();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual object HandleNewElementButtonClicked(object sender)
		{
			_model.SetSelectedElement(_model.CreateNewElement());
			_elementsListPanel.AddItem(_model.SelectedElement, true, true);

			// After a new element is added, then give focus to the editor of the first
			// editor provider. This will assume the first field in the editor is the
			// desired one to give focus.
			var providers = _model.GetComponentEditorProviders().ToArray();
			if (providers.Length > 0)
				providers[0].Control.Focus();

			return null;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleSelectedElementChanged(object sender, object newItem)
		{
			_model.SetSelectedElement(newItem as T);
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleSelectedComponentEditorTabSelecting(object sender,
			TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Selecting)
				((ComponentEditorTabPage)e.TabPage).EditorProvider.Control.Focus();
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
