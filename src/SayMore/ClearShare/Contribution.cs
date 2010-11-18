using System.Collections.Generic;

namespace SayMore.ClearShare
{
	/// <summary>
	/// Records a single contribution of a single individual to a single "work".  If the person performed another role,
	/// they get another contribution record.
	/// </summary>
	public class Contribution
	{
		/// <summary>
		/// This will sometimes come from a controlled vocabulary of known people, but we don't really need to model that here
		/// </summary>
		public string PersonName;

		/// <summary>
		/// The choices for this will come from the owning Work, but we don't model that here.
		/// </summary>
		public License ApprovedLicense;

		/// <summary>
		/// This will often come from a controlled vocabulary supplied by the application, but we don't model that here
		/// </summary>
		public string Role;

		/* the original design had also
		 * ConsentArtifacts, which conceptuall makes sense, but I'm waiting to seet how to implement that in the
		 * context of real apps
		 */
	}
}
