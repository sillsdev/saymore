using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using Palaso.UI.WindowsForms;
using SayMore.Model.Files;
using SayMore.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public interface IEditorProvider
	{
		Control Control { get; }
		string TabText { get; }
		string ImageKey { get; }
		void Initialize(string tabText, string imageKey);
		void SetComponentFile(ComponentFile file);
		bool ComponentFileDeletionInitiated(ComponentFile file);
		Action<string, Type> ComponentFileListRefreshAction { set; }
		void Deactivated();
		void Activated();
		bool IsOKToLeaveEditor { get; }
		bool IsOKToShow { get; }
		event Action<string> TabTextChanged;
		IEnumerable<Control> ChildControls { get; }
		ComponentFile ComponentFile { get; }
		void PrepareToDeactivate();
	}

	/// ----------------------------------------------------------------------------------------
	public class EditorBase : UserControl, IEditorProvider
	{
		private BindingHelper _binder;
		protected ComponentFile _file;
		protected string _tabText;
		protected HashSet<Control> _childControls = new HashSet<Control>();

		public event Action<string> TabTextChanged;
		public string ImageKey { get; protected set; }
		public Action<string, Type> ComponentFileListRefreshAction { protected get; set; }

		/// ------------------------------------------------------------------------------------
		public EditorBase()
		{
			DoubleBuffered = true;
			BackColor = AppColors.DataEntryPanelBegin;
			Padding = new Padding(7);
			AutoScroll = true;
			Name = "EditorBase";

			ControlAdded += HandleControlAdded;
			ControlRemoved += HandleControlRemoved;
			Layout += HandleLayout;

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		public EditorBase(ComponentFile file, string tabText, string imageKey) : this()
		{
			_file = file;
			Initialize(tabText, imageKey);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(string tabText, string imageKey)
		{
			TabText = tabText ?? TabText;
			ImageKey = imageKey;
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshComponentFiles(string fileToSelectAfterRefresh,
			Type componentEditorTypeToSelect)
		{
			if (ComponentFileListRefreshAction != null)
				ComponentFileListRefreshAction(fileToSelectAfterRefresh, componentEditorTypeToSelect);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool ComponentFileDeletionInitiated(ComponentFile file)
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLayout(object sender, LayoutEventArgs e)
		{
			var frm = FindForm();
			if (frm != null)
			{
				frm.Deactivate += delegate { OnFormLostFocus(); };
				Layout -= HandleLayout;
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetComponentFile(ComponentFile file)
		{
			_file = file;

			if (_binder != null)
				_binder.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ideally, the binding helper should be declared in this class and marked protected,
		/// for exposure to subclasses, but I found that when made protected or public, it
		/// still didn't show up in the forms designer, which means individual controls on
		/// a subclassed editor couldn't be bound in the designer. Therefore, each subclass
		/// has to have its own binding helper dropped on the form in the designer. Argh!
		/// This method is provided so subclasses can give the base class a reference to
		/// their binding helper.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void SetBindingHelper(BindingHelper binder)
		{
			_binder = binder;

			if (_binder != null && _file != null)
				_binder.SetComponentFile(_file);
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFile ComponentFile
		{
			get { return _file; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual void PrepareToDeactivate()
		{
		}

		/// ------------------------------------------------------------------------------------
		public Control Control
		{
			get { return this; }
		}

		/// ------------------------------------------------------------------------------------
		public string TabText
		{
			get { return _tabText; }
			protected set
			{
				if (_tabText != value)
				{
					_tabText = value;
					if (TabTextChanged != null)
						TabTextChanged(_tabText);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			SetLabelFonts(this, FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold));
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			var parent = Parent;
			while (parent != null)
			{
				if (parent is TabControl)
				{
					parent.VisibleChanged += (sender, args) => OnParentTabControlVisibleChanged();
					break;
				}
				parent = parent.Parent;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleStringsLocalized()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected string GetPropertiesTabText()
		{
			return LocalizationManager.GetString("CommonToMultipleViews.PropertiesEditor.TabText", "Properties");
		}

		/// ------------------------------------------------------------------------------------
		private static void SetLabelFonts(Control parent, Font fnt)
		{
			foreach (Control ctrl in parent.Controls)
			{
				if (ctrl.Name.StartsWith("_label"))
					ctrl.Font = fnt;
				else
					SetLabelFonts(ctrl, fnt);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToLeaveEditor
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToShow
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Activated()
		{
			// Allows subclasses to do whatever they need to when an editor gets activated
			// (i.e. when the tab control the editor is on becomes visible).
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Deactivated()
		{
			// Allows subclasses to do whatever they need to when an editor gets taken out
			// of use. This is not to be confused with being disposed, the difference being
			// the editor will probably be used again. This could probably use a better name.
		}

		#region Methods for managing child control collection and focus.
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Control> ChildControls
		{
			get { return _childControls; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleControlAdded(object sender, ControlEventArgs e)
		{
			if (_childControls.Add(e.Control))
			{
				e.Control.Leave += HandleControlLeave;
				e.Control.ControlAdded += HandleControlAdded;
				e.Control.ControlRemoved += HandleControlRemoved;
			}

			foreach (Control ctrl in e.Control.Controls)
				HandleControlAdded(null, new ControlEventArgs(ctrl));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleControlRemoved(object sender, ControlEventArgs e)
		{
			e.Control.Leave -= HandleControlLeave;
			e.Control.ControlAdded -= HandleControlAdded;
			e.Control.ControlRemoved -= HandleControlRemoved;
			_childControls.Remove(e.Control);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleControlLeave(object sender, EventArgs e)
		{
			if (ActiveControl == null)
				OnEditorAndChildrenLostFocus();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnEditorAndChildrenLostFocus()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnFormLostFocus()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnParentTabControlVisibleChanged()
		{
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		protected void EnsureFirstRowLabelIsVisible(Label lbl)
		{
			// Makes sure the specified label at the top of the
			// editor is scrolled into view if it's not visible.
			if (!ClientRectangle.Contains(lbl.Bounds))
				AutoScrollPosition = new Point(0, 0);
		}
	}
}
