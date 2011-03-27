using System;
using System.Drawing;
using System.Windows.Forms;
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
			PersonGrid.Factory personGridFactory)
			: base(presentationModel)
		{
			_elementsGrid = personGridFactory();
			_elementsGrid.Name = "PersonGrid";
			InitializeComponent();

			if (DesignMode)
				return;

			Initialize(_componentsSplitter.Panel2, _personComponentFileGrid, _peopleListPanel);
			_personComponentFileGrid.InitializeGrid("PersonScreen");

			if (_componentsSplitter.Panel2.Controls.Count > 1)
				_labelHelp.Visible = false;
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
		public override string Text
		{
			get { return "People"; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return Resources.People; }
		}

		/// ------------------------------------------------------------------------------------
		public ToolStripMenuItem MainMenuItem
		{
			get { return null; }
		}

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
		public override void ViewActivated(bool firstTime)
		{
			base.ViewActivated(firstTime);

			if (firstTime)
			{
				if (Settings.Default.PersonScreenElementListSpiltterPos > 0)
					_elementListSplitter.SplitterDistance = Settings.Default.PersonScreenElementListSpiltterPos;

				if (Settings.Default.PersonScreenComponentsSplitterPos > 0)
					_componentsSplitter.SplitterDistance = Settings.Default.PersonScreenComponentsSplitterPos;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
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
