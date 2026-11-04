namespace SayMore.Utilities
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Implemented by composite controls whose scaling needs more than the generic recursive
	/// Font-reassignment ApplyFontCore does for Label/Button/etc. -- so they can be reached by
	/// the same recursive tree walk that drives every other scaled control, instead of each
	/// independently subscribing to and unsubscribing from UITextScaler.ScaleChanged by hand.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IUiScalable
	{
		void ApplyScale(float scale);
	}
}
