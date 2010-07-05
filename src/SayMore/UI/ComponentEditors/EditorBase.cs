// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: EditorBase.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.UI.Utilities;

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
		void GoDormant();
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class EditorBase : UserControl, IEditorProvider
	{
		private BindingHelper _binder;
		protected ComponentFile _file;

		public string TabText { get; protected set; }
		public string ImageKey { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public EditorBase()
		{
			DoubleBuffered = true;
			BackColor = AppColors.DataEntryPanelBegin;
			Padding = new Padding(7);
			AutoScroll = true;
			Name = "EditorBase";
		}

		/// ------------------------------------------------------------------------------------
		public EditorBase(ComponentFile file, string tabText, string imageKey) : this()
		{
			SetComponentFile(file);
			Initialize(tabText, imageKey);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(string tabText, string imageKey)
		{
			TabText = tabText;
			ImageKey = imageKey;
		}

		protected virtual IEnumerable<string> AllDefaultFieldIds
		{
			get
			{
				return from field in _file.FileType.DefaultFields select field.Key;
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
		public Control Control
		{
			get { return this; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			SetLabelFonts(this, new Font(SystemFonts.IconTitleFont, FontStyle.Bold));
			base.OnLoad(e);
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
		public virtual void GoDormant()
		{
			// Allows subclasses to do whatever they need to when a control gets taken out
			// of use. This is not to be confused with being disposed, the difference being
			// the editor will probably be used again. This could probably use a better name.
		}

	}
}
