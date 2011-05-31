using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.UI.WindowsForms.FileSystem;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Model;
using SayMore.UI.ComponentEditors;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is the base class for both People and Event screens.
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
		public Action<ComponentFile> AfterComponentFileSelected;

		protected readonly ElementListViewModel<T> _model;
		protected ElementGrid _elementsGrid;
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
			_tabControlImages.Images.Add("Notes", Resources.NotesTabImage);
			_tabControlImages.Images.Add("Play", Resources.PlayTabImage);
			_tabControlImages.Images.Add("Person", Resources.PersonFileImage);
			_tabControlImages.Images.Add("Event", Resources.EventFileImage);
			_tabControlImages.Images.Add("Image", Resources.ImageFileImage);
			_tabControlImages.Images.Add("Video", Resources.VideoFileImage);
			_tabControlImages.Images.Add("Audio", Resources.AudioFileImage);

			_elementsGrid.IsOKToSelectDifferentElement = GetIsOKToLeaveCurrentEditor;
			_elementsGrid.DeleteAction = DeleteSelectedElements;
			_elementsGrid.SelectedElementChanged += HandleSelectedElementChanged;
			_elementsGrid.SetFileType(_model.ElementFileType);

			_elementsListPanel = elementsListPanel;
			_elementsListPanel.NewButtonClicked += HandleAddingNewElement;
			_elementsListPanel.DeleteButtonClicked += HandleDeletingSelectedElements;
			_elementsListPanel.ListControl = _elementsGrid;

			_componentFilesControl = componentGrid;
			_componentFilesControl.AfterComponentSelected = HandleAfterComponentFileSelected;
			_componentFilesControl.FilesAdded = HandleFilesAddedToComponentGrid;
			_componentFilesControl.FileDeletionAction = HandleFileDeletedFromComponentGrid;
			_componentFilesControl.FilesBeingDraggedOverGrid = HandleFilesBeingDraggedOverComponentGrid;
			_componentFilesControl.FilesDroppedOnGrid = HandleFilesAddedToComponentGrid;
			_componentFilesControl.PostMenuCommandRefreshAction = HandlePostMenuCommandRefresh;
			_componentFilesControl.IsOKToSelectDifferentFile = GetIsOKToLeaveCurrentEditor;
			_componentFilesControl.IsOKToDoFileOperation = GetIsOKToLeaveCurrentEditor;

			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			var frm = FindForm();
			if (frm != null)
				frm.Activated += HandleParentFormActivated;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleParentFormActivated(object sender, EventArgs e)
		{
			var frm = FindForm();
			if (frm != null)
				frm.Activated -= HandleParentFormActivated;

			if (!_model.VerifyAllElementsStillExist())
				LoadElementList();

			// Do this in case some of the meta data changed (e.g. audio file was edited)
			// while the program was deactivated.
			Refresh();

			if (frm != null)
				frm.Activated += HandleParentFormActivated;
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
		/// reference to each component file that it can pass to this delegate.
		/// ------------------------------------------------------------------------------------
		private void HandleAfterComponentFileSelected(int index)
		{
			_model.DeactivateComponentEditors();
			_model.SetSelectedComponentFile(index);
			ShowSelectedComponentFileEditors();

			if (AfterComponentFileSelected != null)
				AfterComponentFileSelected(_model.SelectedComponentFile);
		}

		/// ------------------------------------------------------------------------------------
		public void HandlePostMenuCommandRefresh()
		{
			var currFile = _model.SelectedComponentFile.PathToAnnotatedFile;
			_model.RefreshSelectedElementComponentFileList();
			UpdateComponentFileList();
			_componentFilesControl.TrySetComponent(currFile);
			_componentFilesControl.Invalidate();
			_elementsGrid.Refresh();
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
		private bool HandleFileDeletedFromComponentGrid(ComponentFile file)
		{
			return _model.DeleteComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void LoadElementList()
		{
			LoadElementList(null);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void LoadElementList(object itemToSelectAfterLoad)
		{
			_elementsGrid.Items = _model.Elements.OrderBy(x => x.Id);

			if (_model.Elements.Count() > 0)
			{
				if (itemToSelectAfterLoad == null)
					_elementsGrid.SelectElement(0);
				else if (itemToSelectAfterLoad is ProjectElement)
					_elementsGrid.SelectElement((ProjectElement)itemToSelectAfterLoad);
				else if (itemToSelectAfterLoad.GetType() == typeof(int))
					_elementsGrid.SelectElement((int)itemToSelectAfterLoad);
				else if (itemToSelectAfterLoad == typeof(string))
					_elementsGrid.SelectElement((string)itemToSelectAfterLoad);
			}

			if (_elementsGrid.GridSettings == null)
				_elementsGrid.AutoResizeColumns();

			_elementsGrid.Refresh();
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentFileList()
		{
			var componentsOfSelectedElement = _model.GetComponentsOfSelectedElement();
			_componentFilesControl.AfterComponentSelected = null;
			_componentFilesControl.UpdateComponentFileList(componentsOfSelectedElement);

			foreach (var componentFile in componentsOfSelectedElement)
			{
				componentFile.IdChanged -= HandleComponentFileIdChanged;
				componentFile.IdChanged += HandleComponentFileIdChanged;
				componentFile.MetadataValueChanged -= HandleComponentFileMetadataValueChanged;
				componentFile.MetadataValueChanged += HandleComponentFileMetadataValueChanged;
			}

			if (componentsOfSelectedElement.Count() == 0)
			{
				_selectedEditorsTabControl = null;
				_componentFilesControl.AddButtonEnabled = false;
			}
			else
			{
				// Setting the selected component to nothing now will make sure that
				// setting it to zero below will cause a row changed event, thus causing
				// the ComponentSelectedCallback event.
				_componentFilesControl.SelectComponent(-1);
				_componentFilesControl.AfterComponentSelected = HandleAfterComponentFileSelected;
				_componentFilesControl.SelectComponent(0);
				ShowSelectedComponentFileEditors();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called when the Component File raises this event, in response to the user
		/// changing a person's name, or an event's id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleComponentFileIdChanged(ComponentFile file,
			string fieldId, object oldId, object newId)
		{
			_elementsGrid.Refresh();
			_elementsGrid.SelectedElementChanged -= HandleSelectedElementChanged;
			_elementsGrid.SelectElement(_model.SelectedElement);
			_elementsGrid.SelectedElementChanged += HandleSelectedElementChanged;
			_model.RefreshAfterIdChanged();
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleComponentFileMetadataValueChanged(ComponentFile file,
			string fieldId, object oldValue, object newValue)
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

			var editorProviders = _model.GetComponentEditorProviders();
			ComponentEditorsTabControl tabCtrl;

			if (!_tabControls.TryGetValue(currProviderKey, out tabCtrl))
			{
				tabCtrl = new ComponentEditorsTabControl(currProviderKey, _tabControlImages,
					editorProviders, ComponentEditorBackgroundColor, ComponentEditorBorderColor);

				tabCtrl.Selecting += HandleSelectedComponentEditorTabSelecting;
				_tabControls[currProviderKey] = tabCtrl;
				_tabControlHostControl.Controls.Add(tabCtrl);

				foreach (var editor in editorProviders)
					editor.ComponentFileListRefreshAction = (() => ComponentFileListRefreshFromEditor());
			}

			// Don't do anything if the selected tab control hasn't changed.
			if (_selectedEditorsTabControl != tabCtrl)
			{
				if (_selectedEditorsTabControl != null)
					_selectedEditorsTabControl.Visible = false;

				tabCtrl.Visible = true;
				_selectedEditorsTabControl = tabCtrl;
			}

			tabCtrl.MakeAppropriateEditorsVisible();
		}

		/// ------------------------------------------------------------------------------------
		private void ComponentFileListRefreshFromEditor()
		{
			_model.RefreshSelectedElementComponentFileList();
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		public ComponentEditorsTabControl SelectedComponentEditorsTabControl
		{
			get { return _selectedEditorsTabControl as ComponentEditorsTabControl; }
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
		protected virtual void HandleAddingNewElement(object sender, EventArgs e)
		{
			var newItem = _model.CreateNewElement();
			_model.SetSelectedElement(newItem);
			LoadElementList(newItem);

			// After a new element is added, then give focus to the first editor. This will
			// assume the first field in the editor is the desired one to give focus.
			var firstEditor = _model.GetComponentEditorProviders().ElementAtOrDefault(0);
			if (firstEditor != null)
				firstEditor.Control.Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool DoesUserConfirmDeletingSelectedElements()
		{
			int itemCount = _elementsGrid.SelectedRows.Count;

			var msg = (itemCount == 1 ?
				LocalizationManager.LocalizeString("Misc. Messages.DeleteOneItemMsg", "{0}") :
				LocalizationManager.LocalizeString("Misc. Messages.DeleteMultipleItemsMsg", "{0} items"));

			msg = (itemCount > 1 ? string.Format(msg, itemCount) :
				string.Format(msg, _elementsGrid.GetCurrentElement().Id));

			return ConfirmRecycleDialog.JustConfirm(msg);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleDeletingSelectedElements(object sender, EventArgs e)
		{
			DeleteSelectedElements();
		}

		/// ------------------------------------------------------------------------------------
		private void DeleteSelectedElements()
		{
			if (!DoesUserConfirmDeletingSelectedElements())
				return;

			var currElementIndex = _elementsGrid.CurrentCellAddress.Y;

			foreach (var item in _elementsGrid.GetSelectedElements())
			{
				if (!_model.Remove(item as T))
					break; //bail after the first 'cancel'
			}

			int newElementCount = _model.Elements.Count();
			while (currElementIndex >= newElementCount)
				currElementIndex--;

			if (currElementIndex >= 0)
			{
				LoadElementList(currElementIndex);
				_model.SetSelectedElement(_elementsGrid.GetCurrentElement() as T);
				UpdateComponentFileList();
				return;
			}

			// At this point, we know that we just deleted all the elements.
			_model.SetSelectedElement(null);
			LoadElementList();
			foreach (var tabCtrl in _tabControls.Values)
			{
				_tabControlHostControl.Controls.Remove(tabCtrl);
				tabCtrl.Selecting -= HandleSelectedComponentEditorTabSelecting;
				tabCtrl.Controls.Clear();
				tabCtrl.Dispose();
			}

			_tabControls.Clear();
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleSelectedElementChanged(object sender,
			ProjectElement oldItem, ProjectElement newItem)
		{
			_model.SetSelectedElement(newItem as T);
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleSelectedComponentEditorTabSelecting(object sender,
			TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Selecting && e.TabPage != null)
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
			return GetIsOKToLeaveCurrentEditor();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetIsOKToLeaveCurrentEditor()
		{
			if (SelectedComponentEditorsTabControl == null)
				return true;

			var editor = SelectedComponentEditorsTabControl.CurrentEditor;
			return (editor == null ? true : editor.IsOKSToLeaveEditor);
		}
	}
}
