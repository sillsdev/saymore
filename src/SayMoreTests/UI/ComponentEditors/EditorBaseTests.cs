using System;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class EditorBaseTests
	{
		#region test sub classes
		/// ------------------------------------------------------------------------------------
		private class TestForm : Form
		{
			public TestForm()
			{
				Location = new System.Drawing.Point(-5000, -5000);
				StartPosition = FormStartPosition.Manual;
				ShowInTaskbar = false;
				Show();
			}

			protected override bool ShowWithoutActivation => true;
		}

		/// ------------------------------------------------------------------------------------
		private class TestEditor : EditorBase
		{
			public event EventHandler EditorAndChildrenLostFocus;
			public event EventHandler FormLostFocus;

			protected override void OnEditorAndChildrenLostFocus()
			{
				EditorAndChildrenLostFocus(this, null);
			}

			protected override void OnFormLostFocus()
			{
				FormLostFocus?.Invoke(this, null);
			}
		}

		#endregion

		private TestEditor _editor;
		private TestForm _testForm;
		private Button _buttonNotInEditor;
		private Panel _panelInEditorOuter;
		private Panel _panelInEditorInner;
		private Label _labelInEditor;
		private Button _buttonInEditor;
		private TextBox _textBoxInEditor;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_editor = new TestEditor();
			_testForm = new TestForm();
			_testForm.Controls.Add(_editor);

			_buttonNotInEditor = new Button();
			_testForm.Controls.Add(_buttonNotInEditor);

			_panelInEditorOuter = new Panel();
			_panelInEditorInner = new Panel();
			_buttonInEditor = new Button();
			_textBoxInEditor = new TextBox();
			_labelInEditor = new Label();

			_panelInEditorInner.Controls.AddRange(new Control[] { _labelInEditor, _buttonInEditor });
			_panelInEditorOuter.Controls.AddRange(new Control[] { _panelInEditorInner, _textBoxInEditor });
			_editor.Controls.Add(_panelInEditorOuter);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_testForm.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChildControls_VerifyAllControlsInList()
		{
			Assert.That(_editor.ChildControls, Is.EquivalentTo(new Control[] 
				{
					_panelInEditorOuter,
					_panelInEditorInner,
					_labelInEditor, 
					_buttonInEditor,
					_textBoxInEditor
				}));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OnEditorAndChildrenLostFocus_FocusMovedToControlOutsideEditor_MethodCalled()
		{
			var called = false;
			_editor.EditorAndChildrenLostFocus += (s, e) => called = true;
			_buttonInEditor.Focus();
			Assert.IsFalse(called);
			_buttonNotInEditor.Focus();
			Assert.IsTrue(called);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OnEditorAndChildrenLostFocus_FocusMovedToControlInsideEditor_MethodNotCalled()
		{
			var called = false;
			_editor.EditorAndChildrenLostFocus += (s, e) => called = true;
			_buttonInEditor.Focus();
			Assert.IsFalse(called);
			_textBoxInEditor.Focus();
			Assert.IsFalse(called);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void OnFormLostFocus_FocusMovedOutsideForm_MethodCalled()
		{
			if (!Environment.UserInteractive)
				Assert.Ignore("Ignored in CI");
			
			var otherForm = new TestForm();
			_testForm.Activate();

			var called = false;
			_editor.FormLostFocus += (s, e) => called = true;
			_buttonInEditor.Focus();
			Assert.IsFalse(called);
			otherForm.Activate();
			Assert.IsTrue(called);
		}
	}
}
