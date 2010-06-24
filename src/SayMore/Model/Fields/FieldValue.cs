using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Serialization;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A FieldValue is a conceptually a key-value pair.  We add other properties as necessary,
	/// but that's the simple idea.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("field")]
	public class FieldValue : IEquatable<FieldValue>
	{
		[XmlAttribute("key")]
		public string FieldKey { get; set; }

		[XmlElement("type")]
		public string Type { get; set; }

		[XmlElement("displayName")]
		public string DisplayName { get; set; }

		[XmlElement("isCustom")]
		public bool IsCustomField { get; set; }

		[XmlIgnore]
		public string Value { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used only for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldValue() { }

		/// ------------------------------------------------------------------------------------
		public FieldValue(string id, string type, string displayName, string value)
		{
			FieldKey = id;
			Type = type;
			DisplayName = displayName;
			Value = value;
		}

		/// ------------------------------------------------------------------------------------
		public FieldValue(string id, string type, string value) : this(id, type, id.TrimStart('_'), value)
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldValue(string id, string value) : this(id, "string", value)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void Copy(FieldValue srcFieldValue)
		{
			FieldKey = srcFieldValue.FieldKey;
			Type = srcFieldValue.Type;
			DisplayName = srcFieldValue.DisplayName;
			Value = srcFieldValue.Value;
			IsCustomField = srcFieldValue.IsCustomField;
		}

		/// ------------------------------------------------------------------------------------
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() != typeof(FieldValue))
				return false;

			return Equals((FieldValue)obj);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter;
		/// otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		/// ------------------------------------------------------------------------------------
		public bool Equals(FieldValue other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return Equals(other.FieldKey, FieldKey) && Equals(other.Type, Type) &&
				Equals(other.DisplayName, DisplayName) && Equals(other.Value, Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		/// ------------------------------------------------------------------------------------
		public override int GetHashCode()
		{
			unchecked
			{
				int result = (FieldKey != null ? FieldKey.GetHashCode() : 0);
				result = (result * 397) ^ (Type != null ? Type.GetHashCode() : 0);
				result = (result * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
				result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
				return result;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator ==(FieldValue left, FieldValue right)
		{
			return Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator !=(FieldValue left, FieldValue right)
		{
			return !Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}='{1}'", FieldKey, Value);
		}

		/// ------------------------------------------------------------------------------------
		public static string MakeIdFromDisplayName(string displayName)
		{
			// REVIEW: I'm sure this doesn't cover every invalid character that XML rejects
			// for tag names. I can't find a .Net method to give me invalid tag characters,
			// so this will have to do for now. Could possibly use the Unicode category.
			var id = from c in displayName.ToCharArray()
					 select (" <>{}()[]/'\"\\.,;:?!@#$%^&*=+`~".IndexOf(c) >= 0 ? '_' : c);

			return new string(id.ToArray());
		}
	}

	public class DefaultFieldList : List<FieldValue>
	{
	}
}
