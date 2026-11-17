using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Windows.Forms.FileSystem;
using SayMore.Model.Files;
using SayMore.Model;
using SayMore.UI.ComponentEditors;
using SayMore.UI.LowLevelControls;
using static SayMore.ApplicationContainer;

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
	/// give us the right one. That might have been an easier approach than what I've
	/// done here.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ElementListScreen<T> : UserControl where T : ProjectElement
	{
		protected readonly ElementListViewModel<T> _model;
		private readonly ILocalizationManager _localizationManager;
		protected ElementGrid _elementsGrid;
		protected TabControl _selectedEditorsTabControl;
		protected ListPanel _elementsListPanel;
		protected ComponentFileGrid _componentFilesControl;
		protected Control _tabControlHostControl;
		protected ImageList _tabControlImages;
		protected HashSet<string> _metadataSearchMatchingIds;

		protected Dictionary<string, ComponentEditorsTabControl> _tabControls =
			new Dictionary<string, ComponentEditorsTabControl>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle a search request from the list panel by reloading the element list
		/// filtered by the search text coming from the panel.
		/// </summary>
		private void HandleSearchRequested(object sender, EventArgs e)
		{
			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		public ToolStripMenuItem MainMenuItem { get; }

		/// ------------------------------------------------------------------------------------
		public ElementListScreen(ElementListViewModel<T> presentationModel, ILocalizationManager lm)
		{
			_model = presentationModel;
			_localizationManager = lm;
			MainMenuItem = new ToolStripMenuItem();
		}

		/// ------------------------------------------------------------------------------------
		protected void Initialize(Control tabControlHostControl,
			ComponentFileGrid componentGrid, ListPanel elementsListPanel)
		{
			_tabControlHostControl = tabControlHostControl;

			_tabControlImages = new ImageList();
			_tabControlImages.ColorDepth = ColorDepth.Depth32Bit;
			_tabControlImages.ImageSize = ResourceImageCache.PlayTabImage.Size;
			_tabControlImages.Images.Add("Notes", ResourceImageCache.NotesTabImage);
			_tabControlImages.Images.Add("Play", ResourceImageCache.PlayTabImage);
			_tabControlImages.Images.Add("Person", ResourceImageCache.PersonFileImage);
			_tabControlImages.Images.Add("Session", ResourceImageCache.SessionFileImage);
			_tabControlImages.Images.Add("Image", ResourceImageCache.ImageFileImage);
			_tabControlImages.Images.Add("Video", ResourceImageCache.VideoFileImage);
			_tabControlImages.Images.Add("Audio", ResourceImageCache.AudioFileImage);

			_elementsGrid.IsOKToSelectDifferentElement = GetIsOKToLeaveCurrentEditor;
			_elementsGrid.DeleteAction = DeleteSelectedElements;
			_elementsGrid.SelectedElementChanged += HandleSelectedElementChanged;
			_elementsGrid.SetFileType(_model.ElementFileType);

			_elementsListPanel = elementsListPanel;
			_elementsListPanel.NewButtonClicked += HandleAddingNewElement;
			_elementsListPanel.SearchRequested += HandleSearchRequested;
			_elementsListPanel.ListControl = _elementsGrid;

			_componentFilesControl = componentGrid;
			_componentFilesControl.AfterComponentSelectionChanged += HandleAfterComponentFileSelected;
			_componentFilesControl.FilesAdded = HandleFilesAddedToComponentGrid;
			_componentFilesControl.FileDeletionAction = file => _model.DeleteComponentFile(file);
			_componentFilesControl.FilesBeingDraggedOverGrid = HandleFilesBeingDraggedOverComponentGrid;
			_componentFilesControl.FilesDroppedOnGrid = HandleFilesAddedToComponentGrid;
			_componentFilesControl.PostMenuCommandRefreshAction = HandlePostMenuCommandRefresh;
			_componentFilesControl.IsOKToSelectDifferentFile = GetIsOKToLeaveCurrentEditor;
			_componentFilesControl.IsOKToDoFileOperation = GetIsOKToLeaveCurrentEditor;
			_componentFilesControl.PrepareToSelectDifferentFile += PrepareToSelectDifferentFile;

			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			HandleStringsLocalized(null, EventArgs.Empty);
			_localizationManager.UiLanguageChanged += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			_localizationManager.UiLanguageChanged -= HandleStringsLocalized;
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleStringsLocalized(object sender, EventArgs e)
		{
			// Overridden in derived classes
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

			if (_model.FileLoadErrors.Any())
			{
				using var dlg = new FileLoadErrorsReportDlg(_model.FileLoadErrors);
				dlg.ShowDialog(this);
			}

			// Do this in case some of the metadata changed (e.g. audio file was edited)
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
		/// Answer: If the object is used, the caller of this delegate would have to get the object
		/// this way: _model.GetComponentFile(index). Using the index here is really just
		/// passing off to the model the inevitable job of indexing into the component file list.
		/// The grid (i.e. the only object calling this delegate so far) does not keep a
		/// reference to each component file that it can pass to this delegate.
		/// ------------------------------------------------------------------------------------
		private void HandleAfterComponentFileSelected(int index)
		{
			_model.DeactivateComponentEditors();
			if (index < 0)
				return;
			_model.SetSelectedComponentFile(index);
			ShowSelectedComponentFileEditors();
			_model.ActivateComponentEditors();
		}

		/// ------------------------------------------------------------------------------------
		public void HandlePostMenuCommandRefresh(string fileToSelectAfterRefresh)
		{
			_model.SelectedElement.ClearComponentFiles();
			UpdateComponentFileList();
			_componentFilesControl.TrySetComponent(fileToSelectAfterRefresh);
			_componentFilesControl.Invalidate();
			_elementsGrid.Refresh();
		}

		/// ------------------------------------------------------------------------------------
		private DragDropEffects HandleFilesBeingDraggedOverComponentGrid(string[] files)
		{
			var validFiles = _model.SelectedElement.GetValidFilesToCopy(files);
			return validFiles.All(pair => File.Exists(pair.Key)) ?
				DragDropEffects.Copy : DragDropEffects.None;
		}

		/// ------------------------------------------------------------------------------------
		private bool HandleFilesAddedToComponentGrid(string[] files)
		{
			if (files.Any(f => _model.ElementFileType.IsMatch(f)))
			{
				MessageBox.Show(string.Format(LocalizationManager.GetString(
					"CommonToMultipleViews.FileList.AddFiles.CannotAddFilesOfElementType",
					"Additional {0} files cannot be added to an existing {0}.",
					"Parameter is an element type (\"Session\" or \"Person\""),
					_model.ElementFileType.Name));
			}
			if (_model.AddComponentFiles(files.Where(f => !_model.ElementFileType.IsMatch(f))))
			{
				UpdateComponentFileList();
				_componentFilesControl.TrySetComponent(files[0]);
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the grid from the model, filtered by whatever search text is currently in
		/// the search box (if any). If itemToSelectAfterLoad is a ProjectElement that the
		/// active filter would hide, the filter is cleared first (clearing it synchronously
		/// re-enters this method via SearchRequested to reload unfiltered; this call then
		/// continues below and reloads again, this time correctly selecting
		/// itemToSelectAfterLoad) rather than leaving that as something every caller has to
		/// avoid - ElementGrid.SelectElement(ProjectElement) throws an ArgumentException if the
		/// element isn't among the filtered rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void LoadElementList(object itemToSelectAfterLoad = null)
		{
			var searchParam = _elementsListPanel?.SearchText;

			if (!string.IsNullOrWhiteSpace(searchParam) &&
				itemToSelectAfterLoad is ProjectElement elementToSelect &&
				!MatchesSearch(elementToSelect, searchParam))
			{
				_elementsListPanel.SearchText = string.Empty;
				searchParam = string.Empty;
			}

			if (!string.IsNullOrWhiteSpace(searchParam))
				DoAsyncMetadataSearch(searchParam, itemToSelectAfterLoad);

			LoadFilteredElementList(itemToSelectAfterLoad, searchParam);
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadFilteredElementList(object itemToSelectAfterLoad, string searchParam)
		{
			// Start with all elements, ordered by id
			IEnumerable<ProjectElement> items = _model.Elements.OrderBy(x => x.Id);

			// If a search parameter was provided, restrict to elements whose Id
			// contains the search text (case-insensitive) or which match the metadata search.
			if (!string.IsNullOrWhiteSpace(searchParam))
				items = items.Where(x => MatchesSearch(x, searchParam));

			_elementsGrid.Items = items;

			if (_elementsGrid.Items.Any())
			{
				switch (itemToSelectAfterLoad)
				{
					case null:
						_elementsGrid.SelectElement(0);
						break;
					case ProjectElement element:
						_elementsGrid.SelectElement(element);
						break;
					case int index:
						// index was computed against a row count that can be stale by the time
						// this runs (e.g. DeleteSelectedElements computes it before the reload,
						// against the pre-deletion row count, which a search filter can make
						// larger than this reload's actual row count) - clamp rather than
						// trust it, since ElementGrid.SelectElement(int) throws otherwise.
						_elementsGrid.SelectElement(Math.Max(0,
							Math.Min(index, _elementsGrid.RowCount - 1)));
						break;
					case string itemId:
						_elementsGrid.SelectElement(itemId);
						break;
				}
			}

			if (ShouldAutoResizeGridColumns(searchParam))
				_elementsGrid.AutoResizeColumns();

			_elementsGrid.Refresh();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hook for a derived class whose search is backed by more than an ID substring match
		/// (e.g. SessionsListScreen's metadata search). Called with a non-blank searchParam
		/// right before the matching synchronous filter pass runs, so an override can reset
		/// whatever state that filter consults and kick off whatever (potentially expensive,
		/// off-UI-thread) search will eventually refine it by calling LoadFilteredElementList
		/// again. searchParam is pinned to this call rather than meant to be re-read live
		/// later, so a result that arrives after the user has typed something else doesn't get
		/// paired with a newer, different filter text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void DoAsyncMetadataSearch(string searchParam, object itemToSelectAfterLoad)
		{
			// No-op by default; overridden in SessionsListScreen.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether element matches the given (non-blank) search text: either by
		/// metadata search (if one was previously run and recorded matching ids), or by a
		/// case-insensitive id substring match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool MatchesSearch(ProjectElement element, string searchParam)
		{
			return (_metadataSearchMatchingIds?.Contains(element.Id) ?? false) ||
				element.Id.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Decides whether to auto-size the grid's columns to fit their content: only when the
		/// user hasn't customized them (no saved GridSettings) AND this load isn't a reload
		/// triggered by an active search. Without the latter check, a user who hasn't
		/// persisted column widths would get a full column-width remeasurement pass over just
		/// the currently filtered rows on every debounced search keystroke.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool ShouldAutoResizeGridColumns(string searchParam)
		{
			return _elementsGrid.GridSettings == null && string.IsNullOrWhiteSpace(searchParam);
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentFileList()
		{
			var componentsOfSelectedElement = _model.GetComponentsOfSelectedElement().ToArray();
			_componentFilesControl.AfterComponentSelectionChanged -= HandleAfterComponentFileSelected;
			_componentFilesControl.UpdateComponentFileList(componentsOfSelectedElement);

			foreach (var componentFile in componentsOfSelectedElement)
			{
				componentFile.IdChanged -= HandleComponentFileIdChanged;
				componentFile.IdChanged += HandleComponentFileIdChanged;
				componentFile.MetadataValueChanged -= HandleComponentFileMetadataValueChanged;
				componentFile.MetadataValueChanged += HandleComponentFileMetadataValueChanged;
				componentFile.AfterSave -= HandleComponentFileSaved;
				componentFile.AfterSave += HandleComponentFileSaved;
			}

			if (!componentsOfSelectedElement.Any())
			{
				_selectedEditorsTabControl = null;
				_componentFilesControl.AddButtonEnabled = false;
			}
			else
			{
				_componentFilesControl.AfterComponentSelectionChanged += HandleAfterComponentFileSelected;
				_componentFilesControl.SelectComponent(0);
				ShowSelectedComponentFileEditors();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called when the Component File raises this event, in response to the user
		/// changing a person's name, or a session's id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleComponentFileIdChanged(ComponentFile file,
			string fieldId, object oldId, object newId)
		{
			_elementsGrid.Refresh();
			_elementsGrid.SelectedElementChanged -= HandleSelectedElementChanged;
			_elementsGrid.SelectElement(_model.SelectedElement);
			_elementsGrid.SelectedElementChanged += HandleSelectedElementChanged;
			_model.SelectedElement.ClearComponentFiles();
			UpdateComponentFileList();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleComponentFileMetadataValueChanged(ComponentFile file,
			string fieldId, object oldValue, object newValue)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleComponentFileSaved(object sender, EventArgs e)
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
		/// showing the tab control containing the selected file's editors.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void ShowSelectedComponentFileEditors()
		{
			var currProviderKey = _model.GetSelectedProviderKey();

			_componentFilesControl.AddButtonEnabled = (currProviderKey != null);

			if (currProviderKey == null)
				return;

			var editorProviders = _model.GetComponentEditorProviders().ToArray();

			// Check if editors for the current file type have been shown yet. If not then
			// load a new tab control for this file type containing the appropriate editors.
			if (!_tabControls.TryGetValue(currProviderKey, out var tabCtrl))
			{
				tabCtrl = new ComponentEditorsTabControl(currProviderKey, _tabControlImages,
					editorProviders, ComponentEditorBackgroundColor, ComponentEditorBorderColor);

				tabCtrl.Selecting += HandleSelectedComponentEditorTabSelecting;
				_tabControls[currProviderKey] = tabCtrl;
				_tabControlHostControl.Controls.Add(tabCtrl);

				foreach (var editor in editorProviders)
					editor.ComponentFileListRefreshAction = ComponentFileListRefreshFromEditor;
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
		private void ComponentFileListRefreshFromEditor(string fileToSelectAfterRefresh,
			Type componentEditorTypeToSelect)
		{
			_model.SelectedElement.ClearComponentFiles();
			UpdateComponentFileList();

			if (fileToSelectAfterRefresh != null &&
				_componentFilesControl.TrySetComponent(fileToSelectAfterRefresh))
			{
				SelectedComponentEditorsTabControl?.TrySelectEditorOfType(componentEditorTypeToSelect);
			}
		}

		/// ------------------------------------------------------------------------------------
		public ComponentEditorsTabControl SelectedComponentEditorsTabControl =>
			_selectedEditorsTabControl as ComponentEditorsTabControl;

		/// ------------------------------------------------------------------------------------
		protected virtual Color ComponentEditorBackgroundColor => SystemColors.Control;

		/// ------------------------------------------------------------------------------------
		protected virtual Color ComponentEditorBorderColor => SystemColors.ControlDark;

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleAddingNewElement(object sender, EventArgs e)
		{
			if (_elementsGrid.IsOKToSelectDifferentElement())
				AfterNewItemAdded(_model.CreateNewElement());
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void AfterNewItemAdded(T newItem)
		{
			_model.SetSelectedElement(newItem);
			LoadElementList(newItem);

			// This doesn't feel completely like the right thing to do. This fixes SP-339,
			// which is a crash that happens when clicking the New button when the current
			// component editor is not the one for an .session or .person file. SP-339 was
			// introduced with the fix for the reentrant call problem (e.g. SP-333). That
			// fix now causes a lag between when a component file is selected and when all
			// the editors for that component file are loaded into the view. It is during
			// that lag that the code below gets executed (i.e., the code to get the first
			// component file editor). But if the editors for the new item's metadata file
			// (i.e., .session or .person) are not yet loaded, then the first editor gotten
			// is one left over from those associated with the previous component file.
			_model.SetSelectedComponentFile(0);

			// After a new element is added, then give focus to the first editor. This will
			// assume the first field in the editor is the desired one to give focus.
			var firstEditor = _model.GetComponentEditorProviders().ElementAtOrDefault(0);
			if (firstEditor == null)
				return;

			// When the index in the list of the new element is the same as the index of
			// the previously selected element, then the component editor won't get
			// updated with the new element's component file because the element grid's
			// row enter event doesn't get fired because the row index hasn't changed.
			// Therefore, we check to see if the first component editor's file is the
			// new one. If not then set it to the new file.
			var componentFiles = _model.SelectedElement.GetComponentFiles();
			if (componentFiles.Length == 0)
				return; // SayMore is probably shutting down.

			var newComponentFile = componentFiles[0];
			if (newComponentFile != firstEditor.ComponentFile)
				firstEditor.SetComponentFile(newComponentFile);

			firstEditor.Control.Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool DoesUserConfirmDeletingSelectedElements()
		{
			int itemCount = _elementsGrid.SelectedRows.Count;

			var msg = LocalizationManager.GetString("MainWindow.DeleteOneItemMsg", "{0}", "For deleting sessions and people items");

			msg = itemCount > 1 ? string.Format(msg, itemCount) :
				string.Format(msg, _elementsGrid.GetCurrentElement().Id);

			return ConfirmRecycleDialog.JustConfirm(msg, itemCount > 1, kSayMoreLocalizationId);
		}

		/// ------------------------------------------------------------------------------------
		private void DeleteSelectedElements()
		{
			if (_elementsGrid.RowCount == 0)
				return;

			if (!DoesUserConfirmDeletingSelectedElements())
				return;

			var currElementIndex = _elementsGrid.CurrentCellAddress.Y;

			foreach (var item in _elementsGrid.GetSelectedElements())
			{
				if (!_model.Remove(item as T))
					break; //bail after the first 'cancel'
			}

			if (_model.Elements.Any())
			{
				// LoadElementList clamps this to a valid row regardless of how many rows are
				// actually displayed after the reload (e.g. fewer than before, if a search
				// filter is active), so passing the pre-deletion row index selects whatever
				// is now at (or nearest to) that same position.
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
		protected virtual void HandleSelectedElementChanged(object sender, ProjectElement newItem)
		{
			PrepareToDeactivateCurrentEditor();
			
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
			// A pre-existing WinForms quirk (ViewDeactivated below disables this control, and
			// disabling a control that has focus silently steals focus away without raising
			// the normal Leave event) can leave the search box's placeholder stuck hidden even
			// though nothing has focus after reactivation. Re-check and correct that here.
			_elementsListPanel?.RefreshSearchPromptIfNeeded();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void ViewDeactivated()
		{
			PrepareToDeactivateCurrentEditor();
		}
		
		/// ------------------------------------------------------------------------------------
		public virtual void PrepareToDeactivateCurrentEditor()
		{
			if (_selectedEditorsTabControl?.SelectedTab is ComponentEditorTabPage componentEditorTab)
				componentEditorTab.EditorProvider?.PrepareToDeactivate();
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
			return editor == null || editor.IsOKToLeaveEditor;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PrepareToSelectDifferentFile(object sender, EventArgs args)
		{
			SelectedComponentEditorsTabControl?.CurrentEditor?.PrepareToDeactivate();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components?.Dispose();

				_elementsGrid.SelectedElementChanged -= HandleSelectedElementChanged;
				_elementsListPanel.NewButtonClicked -= HandleAddingNewElement;
				_elementsListPanel.SearchRequested -= HandleSearchRequested;

				var frm = FindForm();
				if (frm != null)
					frm.Activated -= HandleParentFormActivated;
			}
			base.Dispose(disposing);
		}
	}
}
