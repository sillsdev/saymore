using System;

namespace Sponge2.Model.Fields
{
	/// <summary>
	/// A FieldValue is a conceptually a key-value pair.  We add other properties as necessary,
	/// but that's the simple idea.
	/// </summary>
	public class FieldValue : IEquatable<FieldValue>
	{
		public string FieldDefinitionKey { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldValue(string id, string type, string value)
		{
			FieldDefinitionKey = id;
			Type = type;
			Value = value;
		}

		/// ------------------------------------------------------------------------------------
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (FieldValue)) return false;
			return Equals((FieldValue) obj);
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
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.FieldDefinitionKey, FieldDefinitionKey) && Equals(other.Type, Type) && Equals(other.Value, Value);
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
				int result = (FieldDefinitionKey != null ? FieldDefinitionKey.GetHashCode() : 0);
				result = (result * 397) ^ (Type != null ? Type.GetHashCode() : 0);
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
			return string.Format("{0}='{1}'", FieldDefinitionKey, Value);
		}
	}
}
