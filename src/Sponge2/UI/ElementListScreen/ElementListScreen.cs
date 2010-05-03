using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using SilUtils;
using Sponge2.Model;
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
	/// <typeparam name="T"></typeparam>
	public partial class ElementListScreen<T> : UserControl where T : ProjectElement
	{
		protected readonly ElementListViewModel<T> _model;

		protected System.Windows.Forms.TabControl _componentEditorsTabControl;
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

		protected void Initialize(			TabControl componentEditorsTabControl,
			SilGrid componentGrid,
			ListPanel elementsListPanel
		)
		{
			_componentEditorsTabControl = componentEditorsTabControl;
			_elementsListPanel = elementsListPanel;
			_componentGrid = componentGrid;
			_componentGrid.CellValueNeeded += HandleComponentFileGridCellValueNeeded;
			_componentGrid.RowEnter+= HandleComponentFileGridRowEnter;


			_elementsListPanel.NewButtonClicked += HandleNewElementButtonClicked;
			_elementsListPanel.SelectedItemChanged += HandleSelectedElementChanged;

			_componentEditorsTabControl.Selecting+= HandleSelectedComponentEditorTabSelecting;

			_componentEditorsTabControl.Font = SystemFonts.IconTitleFont;
			_componentGrid.Font = SystemFonts.IconTitleFont;
			LoadElementList();
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadElementList()
		{
			var Elements = _model.Elements.Cast<object>().ToList();

			_elementsListPanel.AddRange(Elements);

			if (Elements.Count > 0)
				_elementsListPanel.SelectItem(Elements[0], true);
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
			_componentGrid.RowCount = _model.ComponentsOfSelectedElement.Count();
			_componentGrid.CellValueNeeded += HandleComponentFileGridCellValueNeeded;

			_componentGrid.Invalidate();
			UpdateComponentEditors();


			//TODO: editor tab (for now, just the first page) isn't currently
			//being displayed, even though the first component shows as highlighted.
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateComponentEditors()
		{
			Utils.SetWindowRedraw(_componentEditorsTabControl, false);

			// GRRR!!! This turns out to steal focus, giving it to the tab control. I think
			// what we're going to have to do is reuse at least one page because removing
			// all but one tab page prevents the problem.

			_componentEditorsTabControl.Selecting -= HandleSelectedComponentEditorTabSelecting;
			_componentEditorsTabControl.TabPages.Clear();

			// At this point, we're just making the tabs and naming them,
			//	so that the entire controls
			// don't need to be build until the user actually tabs to them

			foreach (var provider in _model.GetComponentEditorProviders())
			{
				var page = new ComponentEditorTabPage(provider);
				_componentEditorsTabControl.TabPages.Add(page);

				if (_componentEditorsTabControl.TabPages.Count == 1)
					LoadComponentEditorsForTabPage(page);
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
				LoadComponentEditorsForTabPage(e.TabPage as ComponentEditorTabPage);
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadComponentEditorsForTabPage(ComponentEditorTabPage page)
		{
			if (page != null && !page.AreEditorControlsLoaded)
			{
				var control = page.EditorProvider.GetEditor(_model.SelectedComponentFile);
				control.Dock = DockStyle.Fill;
				_componentEditorsTabControl.SelectedTab.Controls.Add(control);
				page.AreEditorControlsLoaded = true;
			}
		}
	}
}
