using System.Xml.Serialization;
using SIL.Sponge.Controls;

namespace SIL.Sponge.Model
{
	#region SessionFileField class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class defining a field/value pair for a single session file field.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("field")]
	public class SessionFileField : IInfoPanelField
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileField"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFileField()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileField"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SessionFileField(string fieldName, string displayText)
		{
			Name = fieldName;
			DisplayText = displayText;
		}

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlText]
		public string Value { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DisplayText { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Value;
		}
	}

	#endregion
}
