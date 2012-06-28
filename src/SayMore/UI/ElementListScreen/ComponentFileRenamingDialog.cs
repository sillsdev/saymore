using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;
using SilTools;

namespace SayMore.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	public partial class ComponentFileRenamingDialog : Form
	{
		private readonly PathValidator _pathValidator;
		private readonly ComponentFile _componentFile;
		private readonly ComponentRole[] _componentRoles;

		/// ------------------------------------------------------------------------------------
		public ComponentFileRenamingDialog()
		{
			InitializeComponent();

			_pathValidator = new PathValidator(_labelMessage, null)
			{
				InvalidMessage = LocalizationManager.GetString(
					"DialogBoxes.ComponentFileRenamingDlg.FileAlreadyExistsMsg",
					"A file by that name already exists or the name is invalid.")
			};
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFileRenamingDialog(ComponentFile componentFile,
			IEnumerable<ComponentRole> componentRoles) : this()
		{
			_componentFile = componentFile;
			_componentRoles = componentRoles.ToArray();

			_labelPrefix.Text = _componentFile.ParentElement.Id + "_";
			_labelExtension.Text = Path.GetExtension(_componentFile.PathToAnnotatedFile);
			HandleTextBoxTextChanged(null, null);
			SetFonts();

			IntializeLink(_linkSource, ComponentRole.kSourceComponentRoleId);
			IntializeLink(_linkConsent, ComponentRole.kConsentComponentRoleId);
			IntializeLink(_linkCareful, ComponentRole.kCarefulSpeechComponentRoleId);
			IntializeLink(_linkOralTranslation, ComponentRole.kOralTranslationComponentRoleId);
			IntializeLink(_linkTranscription, ComponentRole.kTranscriptionComponentRoleId);
			IntializeLink(_linkWrittenTranslation, ComponentRole.kTranscriptionComponentRoleId);

			_textBox.ReadOnly = !_componentFile.CanBeCustomRenamed;
		}

		/// ------------------------------------------------------------------------------------
		private void SetFonts()
		{
			_linkCareful.Font = Program.DialogFont;
			_linkConsent.Font = Program.DialogFont;
			_linkOralTranslation.Font = Program.DialogFont;
			_linkSource.Font = Program.DialogFont;
			_linkTranscription.Font = Program.DialogFont;
			_linkWrittenTranslation.Font = Program.DialogFont;

			_labelReadAboutRenaming.Font = Program.DialogFont;
			_labelPrefix.Font = Program.DialogFont;
			_textBox.Font = Program.DialogFont;
			_labelExtension.Font = Program.DialogFont;
			_labelShortcutsHint.Font = Program.DialogFont;
			_labelNonSayMoreImportHint.Font = Program.DialogFont;

			_labelChangeNameTo.Font = FontHelper.MakeFont(Program.DialogFont,
				Program.DialogFont.SizeInPoints + 1, FontStyle.Bold);

			_labelShortcuts.Font = _labelChangeNameTo.Font;
			_labelMessage.Font = FontHelper.MakeFont(Program.DialogFont, 8);
		}

		/// ------------------------------------------------------------------------------------
		private void IntializeLink(LinkLabel link, string roleId)
		{
			var role = _componentRoles.FirstOrDefault(r => r.Id == roleId);

			if (role == null)
				return;

			link.Enabled = (role.IsPotential(_componentFile.PathToAnnotatedFile) &&
				!role.IsMatch(_componentFile.PathToAnnotatedFile));

			if (link.Enabled)
			{
				link.Tag = role;
				link.LinkClicked += HandleLinkClicked;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string NewFileName
		{
			get { return _labelPrefix.Text + _textBox.Text.Trim() + _labelExtension.Text; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// New file path, including the file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewFilePath
		{
			get { return Path.Combine(Path.GetDirectoryName(_componentFile.PathToAnnotatedFile), NewFileName); }
		}

		/// ------------------------------------------------------------------------------------
		public ComponentRole GetNewRoleOfFile()
		{
			return _componentRoles.FirstOrDefault(r =>
				_textBox.Text.ToLower() == r.GetRenamingTemplateSuffix().ToLower());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var role = ((LinkLabel)sender).Tag as ComponentRole;
			if (role == null)
				return;

			_textBox.Text = role.GetRenamingTemplateSuffix();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxTextChanged(object sender, EventArgs e)
		{
			var fmt = LocalizationManager.GetString(
				"DialogBoxes.ComponentFileRenamingDlg.NewFileNameMsg",
				"New file: {0}", "Displayed under the text box.", "Parameter is file name.");

			var validMsg = (_textBox.Text.Trim() == string.Empty ?
				string.Empty : string.Format(fmt, NewFilePath));

			_buttonRename.Enabled = _pathValidator.IsPathValid(
				Path.GetDirectoryName(_componentFile.PathToAnnotatedFile), NewFileName, validMsg);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleReadAboutRenamingButtonClick(object sender, EventArgs e)
		{
			Program.ShowHelpTopic("/Concepts/File_names.htm");
		}
	}
}
