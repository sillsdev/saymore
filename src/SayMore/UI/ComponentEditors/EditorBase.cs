using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.XLiffUtils;
using L10NSharp.UI;
using SIL.Windows.Forms;
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
		ComponentFile ComponentFile { get; }
		void PrepareToDeactivate();
	}

	/// ----------------------------------------------------------------------------------------
	// Should be abstract, but that messes up the Designer
	public class EditorBase : UserControl, IEditorProvider
	{
		private bool _setWorkingFontWhenHandleIsCreated = false;
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

			LocalizeItemDlg<XLiffDocument>.StringsLocalized += HandleStringsLocalized;
			HandleStringsLocalized(null);
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
				LocalizeItemDlg<XLiffDocument>.StringsLocalized -= HandleStringsLocalized;

			try
			{
				base.Dispose(disposing);
			}
			catch (InvalidOperationException ex) when (
				ex.Message.Contains("reentrant call to the SetCurrentCellAddressCore"))
			{
				// This is not ideal. But it is better than crashing (SP-2371) and should be
				// safe to ignore. (It's possible that other changes in the TextAnnotationEditor
				// now make this exception impossible, but since I can't reproduce the crash, it
				// is hard to know for sure.)
			}
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
			if (Disposing || IsDisposed)
				return;
			ComponentFileListRefreshAction?.Invoke(fileToSelectAfterRefresh, componentEditorTypeToSelect);
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

			_binder?.SetComponentFile(file);
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
		public ComponentFile ComponentFile => _file;

		/// ------------------------------------------------------------------------------------
		public virtual void PrepareToDeactivate()
		{
		}

		/// ------------------------------------------------------------------------------------
		public Control Control => this;

		/// ------------------------------------------------------------------------------------
		public string TabText
		{
			get => _tabText;
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

			var owningTabControl = FindParent<TabControl>(this);
			if (owningTabControl != null)
				owningTabControl.VisibleChanged += (sender, args) => OnParentTabControlVisibleChanged();

			if (_setWorkingFontWhenHandleIsCreated)
				SetWorkingLanguageFont();
		}

		/// ------------------------------------------------------------------------------------
		public static T FindParent<T>(Control control) where T : Control
		{
			var parent = control.Parent;
			while (parent != null && !(parent is T))
				parent = parent.Parent;

			return parent as T;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleStringsLocalized(ILocalizationManager lm)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected string GetPropertiesTabText() =>
			LocalizationManager.GetString("CommonToMultipleViews.PropertiesEditor.TabText",
				"Properties");

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Assuming the current project has been set and a working language font is defined,
		/// this will trigger a call to <see cref="SetWorkingLanguageFont(Font)"/>.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetWorkingLanguageFont()
		{
			var workingLangFont = Program.CurrentProject?.WorkingLanguageFont;
			if (workingLangFont != null)
			{
				if (IsHandleCreated)
				{
					_setWorkingFontWhenHandleIsCreated = false;
					SetWorkingLanguageFont(workingLangFont);
				}
				else
					_setWorkingFontWhenHandleIsCreated = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Editors can override this to set controls to display text using the working language
		/// font. The base implementation is a no-op and need not be called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void SetWorkingLanguageFont(Font font)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Editors should call this as part of their initialization if they need to be
		/// informed (via <see cref="OnCurrentProjectSet"/>, which they should override) when
		/// the current project has been set.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void NotifyWhenProjectIsSet()
		{
			if (Program.CurrentProject != null)
			{
				OnCurrentProjectSet();
				return;
			}

			using (BackgroundWorker backgroundWorker = new BackgroundWorker())
			{
				backgroundWorker.DoWork += SleepUntilCurrentProjectIsSet;
				backgroundWorker.RunWorkerCompleted += NotifyCurrentProjectSet;
				backgroundWorker.RunWorkerAsync();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void NotifyCurrentProjectSet(object sender, RunWorkerCompletedEventArgs e)
		{
			OnCurrentProjectSet();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Editors can override this to perform a special action in response to the current
		/// project being set. They should call the base unless they do not need/want to set any
		/// controls to use the working language font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnCurrentProjectSet()
		{
			SetWorkingLanguageFont();
		}

		private void SleepUntilCurrentProjectIsSet(object sender, DoWorkEventArgs e)
		{
			var count = 0;
			while (Program.CurrentProject == null && count < 50)
			{
				Thread.Sleep(100);
				count++;
			}
		}
		/// ------------------------------------------------------------------------------------
		private static void SetLabelFonts(Control parent, Font fnt)
		{
			foreach (Control ctrl in parent.Controls)
			{
				// ProjectMetadataScreen should only use the font designs set within it
				if (parent.Name != "ProjectMetadataScreen")
				{
					if (ctrl.Name.StartsWith("_label"))
						ctrl.Font = fnt;
					else
						SetLabelFonts(ctrl, fnt);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToLeaveEditor => true;

		/// ------------------------------------------------------------------------------------
		public virtual bool IsOKToShow => true;

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
		internal IEnumerable<Control> ChildControls => _childControls.ToArray();

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
