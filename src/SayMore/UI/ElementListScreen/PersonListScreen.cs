using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SayMore.Model;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class PersonListScreen : ConcretePersonListScreen, ISayMoreView
	{
		/// ------------------------------------------------------------------------------------
		public PersonListScreen(ElementListViewModel<Person> presentationModel,
			PersonGrid.Factory personGridFactory) : base(presentationModel)
		{
			Logger.WriteEvent("PersonListScreen constructor");

			_elementsGrid = personGridFactory();
			_elementsGrid.Name = "PersonGrid";
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentsSplitter.Panel2, _personComponentFileGrid, _peopleListPanel);
			_personComponentFileGrid.InitializeGrid("PersonScreen");

			InitializeMenus();

			if (_componentsSplitter.Panel2.Controls.Count > 1)
				_labelClickNewHelpPrompt.Visible = false;
			else
				_componentsSplitter.Panel2.ControlAdded += HandleFirstSetOfComponentEditorsAdded;

			_componentsSplitter.Panel2.ControlRemoved += HandleLastSetOfComponentEditorsRemoved;

			_elementsListPanel.ButtonPanelBackColor1 = Settings.Default.PersonEditorsButtonBackgroundColor1;
			_elementsListPanel.ButtonPanelBackColor2 = Settings.Default.PersonEditorsButtonBackgroundColor2;
			_elementsListPanel.ButtonPanelTopBorderColor = Settings.Default.PersonEditorsBorderColor;

			_elementsListPanel.HeaderPanelBackColor1 = Settings.Default.PersonEditorsButtonBackgroundColor2;
			_elementsListPanel.HeaderPanelBackColor2 = Settings.Default.PersonEditorsButtonBackgroundColor1;
			_elementsListPanel.HeaderPanelBottomBorderColor = Settings.Default.PersonEditorsBorderColor;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			_personComponentFileGrid.AddFileButtonTooltipText =
				LocalizationManager.GetString("PeopleView.FileList.AddPersonButtonToolTip", "Add Files for the Person");
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeMenus()
		{
			MainMenuItem.Text = LocalizationManager.GetString(
				"PeopleView.PeopleMainMenu.TopLevelMenuText", "&Person", null, MainMenuItem);

			var menu = new ToolStripMenuItem();
			MainMenuItem.DropDownItems.Add(menu);
			menu.Click += HandleAddingNewElement;
			menu.Text = LocalizationManager.GetString(
				"PeopleView.PeopleMainMenu.NewMenuText", "&New", null, menu);

			MainMenuItem.DropDownItems.Add(new ToolStripSeparator());

			foreach (var menuItem in _elementsGrid.GetMenuCommands())
				MainMenuItem.DropDownItems.Add(menuItem);
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
		public Image Image
		{
			get { return ResourceImageCache.People; }
		}

		/// ------------------------------------------------------------------------------------
		public string NameForUsageReporting
		{
			get { return "People"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ComponentEditorBackgroundColor
		{
			get { return Settings.Default.PersonEditorsBackgroundColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ComponentEditorBorderColor
		{
			get { return Settings.Default.PersonEditorsBorderColor; }
		}

		/// ------------------------------------------------------------------------------------
		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			var tab = viewTabGroup.AddTab(this);
			tab.Name = "PeopleViewTab"; // for tests
			tab.Text = LocalizationManager.GetString("PeopleView.ViewTabText", "People", null, "People View", null, tab);
			Text = tab.Text;
		}

		/// ------------------------------------------------------------------------------------
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);
			Enabled = true;

			if (firstTime)
			{
				if (Settings.Default.PersonScreenElementListSpiltterPos > 0)
					_elementListSplitter.SplitterDistance = Settings.Default.PersonScreenElementListSpiltterPos;

				if (Settings.Default.PersonScreenComponentsSplitterPos > 0)
					_componentsSplitter.SplitterDistance = Settings.Default.PersonScreenComponentsSplitterPos;
			}
		}

		public override void ViewDeactivated()
		{
			base.ViewDeactivated();
			Enabled = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Settings.Default.PersonScreenElementListSpiltterPos = _elementListSplitter.SplitterDistance;
			Settings.Default.PersonScreenComponentsSplitterPos = _componentsSplitter.SplitterDistance;

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
	public class ConcretePersonListScreen : ElementListScreen<Person>
	{
		//design time only
		private ConcretePersonListScreen()
			: base(null)
		{}

		public ConcretePersonListScreen(ElementListViewModel<Person> presentationModel)
			: base(presentationModel)
		{}
	}
}
