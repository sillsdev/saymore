using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using SilUtils;
using Sponge2.Model;
using Sponge2.Model.Files;
using Sponge2.UI.LowLevelControls;

namespace Sponge2.UI.ElementListScreen
{
	/// <summary>
	/// This is the base class for both People and Session screens.
	///
	/// Review for later: Some alternate ways to approach this:
	/// * separate the 3 main areas of these screens into separate controls, each
	/// with their own view model as needed.  This way the two screens could be more
	/// naturally customized as needed.
	/// *Move away from knowing about the generics at this level, and instead take an
	/// IElementListViewModel. Leave it to the DI to give us the right one.  That might
	/// have been an easier approach than what I've done here.
	/// </summary>
	public partial class ElementListScreen<T> : UserControl where T : ProjectElement
	{
		protected readonly ElementListViewModel<T> _model;

		protected TabControl _componentEditorsTabControl;
		protected ListPanel _elementsListPanel;
		protected SilGrid _componentGrid;

		/// ------------------------------------------------------------------------------------
		public ElementListScreen(ElementListViewModel<T> presentationModel)
		{
			_model = presentationModel;
			//	InitializeComponent();

			if (DesignMode)
				return;
		}

		/// ------------------------------------------------------------------------------------
		protected void Initialize(TabControl componentEditorsTabControl,
			SilGrid componentGrid, ListPanel elementsListPanel)
		{
			_componentEditorsTabControl = componentEditorsTabControl;
			_componentEditorsTabControl.TabPages.Clear();
			_elementsListPanel = elementsListPanel;
			_componentGrid = componentGrid;
			_componentGrid.CellValueNeeded += HandleComponentFileGridCellValueNeeded;
			_componentGrid.RowEnter += HandleComponentFileGridRowEnter;

			_elementsListPanel.NewButtonClicked += HandleNewElementButtonClicked;
			_elementsListPanel.SelectedItemChanged += HandleSelectedElementChanged;

			_componentEditorsTabControl.Selecting += HandleSelectedComponentEditorTabSelecting;

			_componentEditorsTabControl.Font = SystemFonts.IconTitleFont;
			_componentGrid.Font = SystemFonts.IconTitleFont;
			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadElementList()
		{
			var elements = _model.Elements.Cast<object>().ToList();

			_elementsListPanel.AddRange(elements);

			if (elements.Count > 0)
				_elementsListPanel.SelectItem(elements[0], true);
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateDisplay()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentList()
		{
			// I think there's a bug in the grid that fires the cell value needed event in
			// the process of changing the row count but it fires it for rows that are
			// no longer supposed to exist. This tends to happen when the row count was
			// previously higher than the new value.
			_componentGrid.CellValueNeeded -= HandleComponentFileGridCellValueNeeded;
			var componentsOfSelectedElement = _model.ComponentsOfSelectedElement;
			_componentGrid.RowCount = componentsOfSelectedElement.Count();
			_componentGrid.CellValueNeeded += HandleComponentFileGridCellValueNeeded;

			foreach (var componentFile in componentsOfSelectedElement)
			{
				componentFile.UiShouldRefresh -=HandleUiShouldRefresh;
				componentFile.UiShouldRefresh += HandleUiShouldRefresh;
				//review: and later, are we wired longer than we want to be?
			}
			_componentGrid.Invalidate();
			UpdateComponentEditors();

			//TODO: editor tab (for now, just the first page) isn't currently
			//being displayed, even though the first component shows as highlighted.
		}

		/// <summary>
		/// At the moment, this doesn't distinguish between events which would cause
		/// the element list to need updating (e.g., person name column or file duration
		/// column or some such) vs. things that just require that the component list update.
		///
		/// This is called when the Component File raises this even, in response to the user
		/// doing something like changing a person's name, or a session's id
		/// </summary>
		void HandleUiShouldRefresh(object sender, System.EventArgs e)
		{
			_elementsListPanel.Invalidate(); //TODO: David, I couldn't get this to work. What I was wanting was to make the element list update what it has for this element
			_componentGrid.Invalidate();
			UpdateComponentEditors();
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentEditors()
		{
			Utils.SetWindowRedraw(_componentEditorsTabControl, false);

			_componentEditorsTabControl.Selecting -= HandleSelectedComponentEditorTabSelecting;

			// Remove all but one tab page because removing all of them will
			// steal the focus from the active control. Go figure.
			for (int i = _componentEditorsTabControl.TabCount - 1; i > 0; i--)
				_componentEditorsTabControl.TabPages.RemoveAt(i);

			// At this point, just make tabs and name them. A tab's editor
			// controls will be built only when the user selects the tab.
			foreach (var provider in _model.GetComponentEditorProviders())
			{
				ComponentEditorTabPage page;

				if (_componentEditorsTabControl.TabPages.Count != 1)
				{
					page = new ComponentEditorTabPage(provider);
					_componentEditorsTabControl.TabPages.Add(page);
				}
				else
				{
					page = _componentEditorsTabControl.TabPages[0] as ComponentEditorTabPage;
					page.SetProvider(provider);
				}

				if (_componentEditorsTabControl.TabPages.Count == 1)
					page.LoadEditorControl(_model.SelectedComponentFile);
			}

			_componentEditorsTabControl.Selecting += HandleSelectedComponentEditorTabSelecting;
			Utils.SetWindowRedraw(_componentEditorsTabControl, true);
		}

		/// ------------------------------------------------------------------------------------
		protected object HandleNewElementButtonClicked(object sender)
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
		protected void HandleComponentFileGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			// This event is fired even when the grid gains focus without the row actually
			// changing, therefore we should just ignore the event when the row hasn't changed.
			if (e.RowIndex != _componentGrid.CurrentCellAddress.Y)
			{
				_model.SetSelectedComponentFile(e.RowIndex);
				UpdateComponentEditors();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleComponentFileGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var dataPropName = _componentGrid.Columns[e.ColumnIndex].DataPropertyName;
			var currElementFile = _model.GetComponentFile(e.RowIndex);

			e.Value = (currElementFile == null ? null :
				ReflectionHelper.GetProperty(currElementFile, dataPropName));
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleSelectedComponentEditorTabSelecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.Action == TabControlAction.Selecting)
				((ComponentEditorTabPage)e.TabPage).LoadEditorControl(_model.SelectedComponentFile);
		}
	}
}
