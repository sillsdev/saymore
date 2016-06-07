using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Model;
using SayMore.Properties;

namespace SayMore.UI.ElementListScreen
{
	public class PersonGrid : ElementGrid
	{
		public delegate PersonGrid Factory();  //autofac uses this

		public PersonGrid()
		{
			Program.PersonDataChanged += Program_PersonDataChanged;
		}

		/// <summary>Update the list when the display name changes</summary>
		private void Program_PersonDataChanged()
		{
			Application.DoEvents();
			Refresh();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			Program.PersonDataChanged -= Program_PersonDataChanged;
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public override GridSettings GridSettings
		{
			get
			{
				try
				{
					return Settings.Default.PersonListGrid;
				}
				catch
				{
					return new GridSettings();
				}
			}
			set { Settings.Default.PersonListGrid = value; }
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ToolStripMenuItem> GetMenuCommands()
		{
			if (DeleteAction != null)
			{
				var menu = new ToolStripMenuItem(string.Empty, null, (s, e) => DeleteAction());
				menu.Text = LocalizationManager.GetString("MainWindow.DeletePersonMenuText", "&Delete Person...", null, menu);
				yield return menu;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName == "display name")
				return element.UiId;

			if (fieldName != "consent")
				return base.GetValueForField(element, fieldName);

			return ((Person)element).GetInformedConsentImage();
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetSortValueForField(ProjectElement element, string fieldName)
		{
			return (fieldName == "consent" ?
				((Person)element).GetInformedConsentSortKey() :
				base.GetSortValueForField(element, fieldName));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				Columns[e.ColumnIndex].DataPropertyName == "consent")
			{
				var element = Items.ElementAt(e.RowIndex);
				this[e.ColumnIndex, e.RowIndex].ToolTipText =
					((Person)element).GetToolTipForInformedConsentType();
			}
		}
	}
}
