using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using L10NSharp;
using SayMore.Utilities;
using SIL.Reporting;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.DialogResult;

namespace SayMore.UI.ProjectWindow
{
	// Deliberately not a SayMoreFormBase: this dialog owns the UITextScale setting, so it
	// already implements its own scale-on-load (from the saved setting) and live-preview-as-
	// you-adjust-the-spinner (from the in-dialog control, before it's saved) logic below.
	// Wiring it to the generic base's Load/ScaleChanged hooks as well would mean the two had
	// to be coordinated (the base would scale from the saved setting first, so the live preview
	// would need to treat that as its baseline instead of the form's true original font) for no
	// benefit, since this class already covers both the initial-load and setting-changed cases
	// on its own. See SayMoreFormBaseComplianceTests for the matching exclusion.
	public partial class UILanguageDlg : Form
	{
		private float _baseLineFontSize;
		private int _initialScalePercent;
		private bool _isSettingScaleValueProgrammatically;
		private bool _scaleWasAutoBumped;
		private bool _isInitializing = true;
		private Dictionary<Control, Padding> _baselineMargins;
		public delegate UILanguageDlg Factory(); //autofac uses this
		public string UILanguage { get; private set; }

		public UILanguageDlg()
		{
			Logger.WriteEvent("UILanguageDlg constructor");
			InitializeComponent();

			// The Designer sets these too, but that copy can drift out of sync with the real
			// source of truth (it did, during development of this feature) with nothing to
			// catch it beyond a comment asking nicely. Overwriting them here means a mismatch
			// is at worst a harmless no-op, not a silent bug.
			_numUpDownScale.Minimum = UITextScaler.kMinScalePercent;
			_numUpDownScale.Maximum = UITextScaler.kMaxScalePercent;

			_labelLanguage.Font = Program.DialogFont;
			_linkIWantToLocalize.Font = Program.DialogFont;
			_comboUILanguage.Font = Program.DialogFont;

			// Captured here, rather than in OnLoad as before, because applying the selected
			// language's recommended scale below can itself trigger a live-preview font scale
			// (via _numUpDownScale's ValueChanged) before OnLoad ever runs. Nothing changes
			// Font between here and OnLoad, so the value is the same either way.
			_baseLineFontSize = Font.Size;
			_initialScalePercent = UITextScaler.ClampScalePercent(Properties.Settings.Default.UITextScale);
			CaptureBaselineMargins();

			// _isInitializing suppresses the auto-bump/undo logic that
			// _comboUILanguage_SelectedIndexChanged would otherwise run in response to the
			// assignment below -- without it, reopening the dialog with a language the user
			// had previously, deliberately set back to 100% for would look indistinguishable
			// from a fresh dialog and get re-bumped, since there's nothing here to say "this
			// 100% was already a deliberate choice for this language."
			_comboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			_numUpDownScale.Value = UITextScaler.ClampScalePercent(Properties.Settings.Default.UITextScale);
			_isInitializing = false;
			RefreshHintAndResetButtonDisplay();

			DialogResult = Cancel;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ScaleFonts();
		}
		
		/// ------------------------------------------------------------------------------------
		protected void ScaleFonts()
		{
			try
			{
				var scale = GetScalePercent(_numUpDownScale.Value) / 100f;
				var newFont = new Font(Font.FontFamily, GetPreviewFontSize(_baseLineFontSize, _numUpDownScale.Value), Font.Style);
				this.ApplyFontRecursively(newFont, scale);
				ApplyScaledMargins(scale);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remembers each row control's original (100%) Margin, before anything has scaled it,
		/// so later calls always scale from the true baseline rather than compounding on an
		/// already-scaled value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CaptureBaselineMargins()
		{
			_baselineMargins = new Dictionary<Control, Padding>();
			foreach (Control control in _tableLayoutPanel.Controls)
				_baselineMargins[control] = control.Margin;
		}

		/// ------------------------------------------------------------------------------------
		private void ApplyScaledMargins(float scale)
		{
			foreach (var pair in _baselineMargins)
				pair.Key.Margin = ScaleMargin(pair.Value, scale);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Computes the scaled baseline font size for the live preview. The spinner's raw
		/// Value can transiently be out of its own configured [Minimum, Maximum] range while
		/// the user is actively typing into it (e.g. after clearing the field to type a new
		/// value), which previously crashed System.Drawing.Font's constructor (emSize must be
		/// greater than 0). Clamping here guarantees a valid, positive font size regardless.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static float GetPreviewFontSize(float baselineSize, decimal rawSpinnerValue)
		{
			return baselineSize * GetScalePercent(rawSpinnerValue) / 100f;
		}

		/// ------------------------------------------------------------------------------------
		private static int GetScalePercent(decimal rawSpinnerValue)
		{
			return UITextScaler.ClampScalePercent((int)rawSpinnerValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scales a baseline Margin by the given factor. Font-driven control sizes grow with
		/// the live preview automatically, but a Control's Margin is a fixed pixel value that
		/// doesn't, so without this, the gap between rows shrinks relative to their now much
		/// taller content as the scale increases, eventually looking like controls are touching
		/// even though nothing is technically overlapping in the layout math. A side whose
		/// baseline margin is 0 has nothing to scale from and stays 0 regardless.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static Padding ScaleMargin(Padding baseline, float scale)
		{
			return new Padding(
				(int)Math.Round(baseline.Left * scale),
				(int)Math.Round(baseline.Top * scale),
				(int)Math.Round(baseline.Right * scale),
				(int)Math.Round(baseline.Bottom * scale));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Decides what the scale value, auto-bumped flag, and hint-label visibility should be
		/// when the selected language changes (including the initial selection at dialog load).
		/// If our own earlier auto-bump is still in effect (the user hasn't touched the value
		/// since), it's undone before possibly re-applying for the newly selected language, so
		/// switching back and forth never leaves behind a stale, unexplained bumped value. A
		/// value the user deliberately set (even if it happens to equal 100) is never touched.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static (int NewValue, bool AutoBumped, bool ShowHint) ResolveScaleForLanguageChange(
			int currentValue, bool wasAutoBumped, int? recommendedForNewLanguage)
		{
			var value = currentValue;
			var autoBumped = wasAutoBumped;

			if (autoBumped)
			{
				value = 100;
				autoBumped = false;
			}

			if (recommendedForNewLanguage.HasValue && value == 100)
			{
				value = recommendedForNewLanguage.Value;
				autoBumped = true;
			}

			return (value, autoBumped, recommendedForNewLanguage.HasValue);
		}

		/// ------------------------------------------------------------------------------------
		internal static int GetResetTargetValue(int? recommendedForLanguage)
		{
			return recommendedForLanguage ?? 100;
		}

		/// ------------------------------------------------------------------------------------
		internal static bool IsResetEnabled(int currentValue, int? recommendedForLanguage)
		{
			return currentValue != GetResetTargetValue(recommendedForLanguage);
		}

		/// ------------------------------------------------------------------------------------
		private int? GetRecommendedScaleForSelectedLanguage()
		{
			return _comboUILanguage.SelectedItem is L10NCultureInfo culture
				? UITextScaler.GetRecommendedScalePercent(culture.Name)
				: null;
		}

		/// ------------------------------------------------------------------------------------
		private void SetScaleValue(int value)
		{
			_isSettingScaleValueProgrammatically = true;
			try
			{
				_numUpDownScale.Value = value;
			}
			finally
			{
				_isSettingScaleValueProgrammatically = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ApplyRecommendationForSelectedLanguage()
		{
			var recommended = GetRecommendedScaleForSelectedLanguage();
			var result = ResolveScaleForLanguageChange(GetScalePercent(_numUpDownScale.Value),
				_scaleWasAutoBumped, recommended);

			if (result.NewValue != _numUpDownScale.Value)
				SetScaleValue(result.NewValue);

			_scaleWasAutoBumped = result.AutoBumped;
			RefreshHintAndResetButtonDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display-only: updates the hint label's visibility and the Reset button's enabled
		/// state to match the currently selected language, without touching the scale value or
		/// _scaleWasAutoBumped. Safe to call any time, including during initial construction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshHintAndResetButtonDisplay()
		{
			_lblScaleRecommendationHint.Visible = GetRecommendedScaleForSelectedLanguage().HasValue;
			UpdateResetButtonEnabled();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateResetButtonEnabled()
		{
			_btnReset.Enabled = IsResetEnabled(GetScalePercent(_numUpDownScale.Value),
				GetRecommendedScaleForSelectedLanguage());
		}

		/// ------------------------------------------------------------------------------------
		private void _comboUILanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			// During construction, this fires as a side effect of setting the initial selection
			// below, not a genuine interactive language change -- see the comment there.
			if (_isInitializing)
				return;

			ApplyRecommendationForSelectedLanguage();
		}

		private void HandleIWantToLocalizeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://crowdin.com/project/saymore");
			_comboUILanguage.RefreshList();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (DialogResult == OK)
			{
				UILanguage = ((L10NCultureInfo)_comboUILanguage.SelectedItem).Name;
				var finalScalePercent = GetScalePercent(_numUpDownScale.Value);
				Properties.Settings.Default.UITextScale = finalScalePercent;

				// At this point, the language switch itself hasn't actually been applied yet, so
				// this is purely about the scale value. Only notify when the scale genuinely
				// changed, to avoid a pointless re-scale pass over controls that are already
				// correctly scaled. This also sidesteps relying on every control's cached
				// original font surviving a redundant pass unscathed.
				if (finalScalePercent != _initialScalePercent)
					UITextScaler.NotifyScaleChanged();
			}

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void _btnReset_Click(object sender, EventArgs e)
		{
			SetScaleValue(GetResetTargetValue(GetRecommendedScaleForSelectedLanguage()));
			_scaleWasAutoBumped = true;
			UpdateResetButtonEnabled();
		}

		/// ------------------------------------------------------------------------------------
		private void _numUpDownScale_ValueChanged(object sender, EventArgs e)
		{
			if (!_isSettingScaleValueProgrammatically)
				_scaleWasAutoBumped = false;

			UpdateResetButtonEnabled();

			// This effectively gives a preview.
			ScaleFonts();
		}
	}
}
