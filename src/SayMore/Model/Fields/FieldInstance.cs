using System;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A FieldInstance is a conceptually a key-value pair.  We add other properties as necessary,
	/// but that's the simple idea.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldInstance : IEquatable<FieldInstance>
	{
		public string FieldId { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldInstance(string id, string type, string value)
		{
			FieldId = id;
			Type = type;
			Value = value;
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance(string id, string value) : this(id, "string", value)
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance CreateCopy()
		{
			return new FieldInstance(FieldId, Type, Value);
		}

		/// ------------------------------------------------------------------------------------
		public void Copy(FieldInstance source)
		{
			FieldId = source.FieldId;
			Type = source.Type;
			Value = source.Value;
		}

		/// ------------------------------------------------------------------------------------
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() != typeof(FieldInstance))
				return false;

			return Equals((FieldInstance)obj);
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
		public bool Equals(FieldInstance other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return Equals(other.FieldId, FieldId) &&
				Equals(other.Type, Type) && Equals(other.Value, Value);
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
				int result = (FieldId != null ? FieldId.GetHashCode() : 0);
				result = (result * 397) ^ (Type != null ? Type.GetHashCode() : 0);
				result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
				return result;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator ==(FieldInstance left, FieldInstance right)
		{
			return Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public static bool operator !=(FieldInstance left, FieldInstance right)
		{
			return !Equals(left, right);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}='{1}'", FieldId, Value);
		}

		///// ------------------------------------------------------------------------------------
		//public static string MakeIdFromDisplayName(string displayName)
		//{
		//    // REVIEW: I'm sure this doesn't cover every invalid character that XML rejects
		//    // for tag names. I can't find a .Net method to give me invalid tag characters,
		//    // so this will have to do for now. Could possibly use the Unicode category.
		//    var id = from c in displayName.ToCharArray()
		//             select (" <>{}()[]/'\"\\.,;:?!@#$%^&*=+`~".IndexOf(c) >= 0 ? '_' : c);

		//    return new string(id.ToArray());
		//}
	}
}
