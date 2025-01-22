using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Core.ClearShare;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A FieldInstance is conceptually a key-value pair. We add other properties as necessary,
	/// but that's the simple idea.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldInstance : IEquatable<FieldInstance>
	{
		public const char kDefaultMultiValueDelimiter = ';';
		public const char kAlternateMultiValueDelimiter = ',';
		public const string kStringType = "string";

		public string FieldId { get; set; }
		public string Type { get; set; }
		public bool IsCustom { get; set; }
		public object Value { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldInstance(string id, string type, object value)
		{
			FieldId = id;
			Type = type;
			Value = value;
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance(string id, object value) :
			this(id, (value == null || value is string) ? kStringType : "xml", value)
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldInstance(string id)
			: this(id, string.Empty)
		{
		}

		/// ------------------------------------------------------------------------------------
		public string ValueAsString
		{
			get
			{
				if (Value == null)
					return null;

				// Following turns a contribution collection into a list of contributor names.
				// According to JohnH in SP-575, the people field only applies to Persons in the
				// People Tab. It's not about "who contributed", but "who was recorded". Not sure
				// if this will ever be needed elsewhere, but at least for this purpose then, it
				// seems we just want to return null. Ideally, I think this should be done in an
				// override of ToString in the ContributionCollection class, but that is part of
				// Palaso, and it might be used elsewhere, besides it feels a little weird to
				// override ToString just to make it return null.
				// var contribs = Value as ContributionCollection;
				//return contribs != null ? String.Join(";", contribs) : Value.ToString();
				return Value is ContributionCollection ? null : Value as string ??
					Value.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a copy of the field's value if the value is a value type or if it
		/// implements ICloneable. Otherwise, the returned value will be a reference to
		/// this instance's value object. So be warned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object CopyValue()
		{
			// Reference types need to do a deep copy.
			return (Value is ICloneable ? ((ICloneable)Value).Clone() : Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copies the contents of the source instance to this instance. If the value is a
		/// value type or implements ICloneable (and the cloning is a deep copy), then
		/// all should work fine. Otherwise, the value will just refer to the same object
		/// that's in the source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Copy(FieldInstance source)
		{
			FieldId = source.FieldId;
			Type = source.Type;

			// Reference types need to do a deep copy.
			Value = (Value is ICloneable ? ((ICloneable)source.Value).Clone() : source.Value);
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

			if (!Equals(other.FieldId, FieldId) || !Equals(other.Type, Type))
				return false;

			if (Value is string && other.Value is string)
				return Equals(other.ValueAsString, ValueAsString);

			return Value.Equals(other.Value);
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
				result = (result * 397) ^ (ValueAsString != null ? ValueAsString.GetHashCode() : 0);
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
			return string.Format("{0}='{1}'", FieldId, ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasMultipleValues()
		{
			return (!string.IsNullOrEmpty(ValueAsString) && (GetValues().Count() > 1));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetValues()
		{
			return GetMultipleValuesFromText(ValueAsString);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Splits the specified string using a delimiter and returns the resulting list of
		/// values. Values are trimmed and any resulting in empty values are not included in
		/// the returned list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetMultipleValuesFromText(string text)
		{
			if (text == null)
				text = string.Empty;

			var list = text.Split(new[] { kDefaultMultiValueDelimiter, kAlternateMultiValueDelimiter },
				StringSplitOptions.RemoveEmptyEntries);

			list = NamesWithoutRoles(list);

			return (from val in list
					where val.Trim() != string.Empty
					select val.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unique sorted list of names with role removed from name if present
		/// </summary>
		/// <param name="list">list of names</param>
		/// <returns>names list without role</returns>
		/// ------------------------------------------------------------------------------------
		private static string[] NamesWithoutRoles(IEnumerable<string> list)
		{
			if (list.Any(name => name.Contains(" (")))
				return new SortedSet<string>(from name in list
					let i = name.IndexOf(" (", StringComparison.Ordinal)
					select i >= 0 ? name.Substring(0, i) : name).ToArray();
			return list.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public static string GetTextFromMultipleValues(IEnumerable<string> values)
		{
			var bldr = new StringBuilder();
			foreach (var val in values.Where(x => x != null && x.Trim().Length > 0))
				bldr.AppendFormat("{0}{1} ", val, kDefaultMultiValueDelimiter);

			// Whack off the last space and delimiter.
			if (bldr.Length > 1)
				bldr.Length -= 2;

			return bldr.ToString();
		}
	}
}
