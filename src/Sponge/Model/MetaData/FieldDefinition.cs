using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SIL.Sponge.Model.MetaData
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This code originally came from WeSay, which has a more complicated, hierarchical
	/// structure. It also had (and still has some) code for user creating/editing of the
	/// field properties. I couldn't bring myself to quite strip all that out just yet,
	/// though it surely won't be part of early releases of Sponge.
	///
	/// So, some of what is left may turn out to be overkill, but in general this has
	/// proved to be a nice model to work with.
	///
	/// At the advice of Ken Zook many years ago, this model rejects the idea of a field
	/// which will *never* be anything but a single Writing System... someone always wants
	/// to add another language.  But that was in the Lexical area, so may not be true here.
	///
	/// My intention is see about keeping this class simple by not tying particular
	/// input/output formats to fields.  I may also try to strip out the fields related to
	/// presentation (e.g. visibility) to a separate class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("field")]
	public class FieldDefinition
	{
		#region Enumerations
		public enum MultiplicityType
		{
			ZeroOr1 = 0,
			ZeroOrMore = 1
		}

		public enum BuiltInDataType
		{
			MultiText,
			Option,
			OptionCollection,
			Picture,
			RelationToOneEntry
		}

		public enum VisibilitySetting
		{
			Visible,
			ReadOnly,
			NormallyHidden
			//Invisible  use Enabled=false rather than Invisible
		}

		#endregion

		/* JH: not sure if we'll need/want each field to know its class here...
		 * not sure that we have to deal with any hierarchy yet*/
		private string _fieldName;
		private string _displayName = string.Empty;
		private List<string> _writingSystemIds;
		private bool _isSpellCheckingEnabled;
		private string _optionsListFile;
		private bool _isMultiParagraph;
		private MultiplicityType _multiplicity = MultiplicityType.ZeroOr1;
		private VisibilitySetting _visibility = VisibilitySetting.Visible;
		// private string _className = string.Empty;
		// private bool _enabled;
		// private bool _enabledNotSet = true;

		#region Construction and Initialization
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDefinition"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldDefinition()
		{
			Initialize("unknown", "MultiText", MultiplicityType.ZeroOr1, new List<string>());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDefinition"/> class.
		/// REVIEW: Don't know what to do with the className. Do we need it?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldDefinition(string fieldName, string className, IEnumerable<string> writingSystemIds)
			: this(fieldName, className, writingSystemIds, MultiplicityType.ZeroOr1, "MultiText")
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDefinition"/> class.
		/// REVIEW: Don't know what to do with the parentClassName. Do we need it?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldDefinition(string fieldName, string parentClassName,
			IEnumerable<string> writingSystemIds, MultiplicityType multiplicity, string dataTypeName)
		{
			if (writingSystemIds == null)
				throw new ArgumentNullException();

			Initialize(fieldName, dataTypeName, multiplicity, writingSystemIds);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the specified field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize(string fieldName, string dataTypeName,
			MultiplicityType multiplicity, IEnumerable<string> writingSystemIds)
		{
			FieldName = fieldName;
			WritingSystemIds = new List<string>(writingSystemIds);
			_multiplicity = multiplicity;
			DataTypeName = dataTypeName;
		}

		#endregion

		#region Static stuff
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up after exposing field name to UI for user editting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeFieldNameSafe(string text)
		{
			// Parentheses mess up our greps, don't really belong in xml names
			foreach (char c in "()[]{}<>\\?*+& ")
				text = text.Replace(c.ToString(), string.Empty);

			return text.Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modifies the master from user.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ModifyMasterFromUser(FieldDefinition master, FieldDefinition user)
		{
			// this worked so long as the master had all possible valid writing systems in each field
			//          master.WritingSystemIds = GetIntersectionOfWsIdLists(master, user);
			master.WritingSystemIds = user.WritingSystemIds;
			master.Visibility = user.Visibility;
		}

		//        private static List<string> GetIntersectionOfWsIdLists(FieldDefinition master, FieldDefinition user)
		//        {
		//            List<string> l = new List<string>();
		//            foreach (string id in master.WritingSystemIds)
		//            {
		//                if (user.WritingSystemIds.Contains(id))
		//                {
		//                    l.Add(id);
		//                }
		//            }
		//            return l;
		//        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the new field name prefix.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string NewFieldNamePrefix
		{
			get { return "newField"; }
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		[Description("The name of the field, as it will appear an external file.")]
		public string FieldName
		{
			get
			{
				if (_fieldName == null)
					throw new InvalidOperationException("FieldName must be set before it can be used.");

				return _fieldName;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_fieldName = MakeFieldNameSafe(value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Key
		{
			get { return /*_className + "." +*/ _fieldName; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the localization string id. REVIEW: Should this be the Key?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("stringId")]
		[Description("The string id used to identify the DisplayName for localization purposes.")]
		public string StringId { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("defaultDisplayName")]
		[Description("The label of the field as it will be displayed to the user.")]
		public string DisplayName
		{
			get { return (string.IsNullOrEmpty(_displayName) ? "*" + FieldName : _displayName); }
			set { _displayName = value; }
		}

		//            [Browsable(false)]
		//            public bool UserCanDeleteOrModify
		//            {
		//                get
		//                {
		//                    return false;
		//                }
		//            }

		//            [Browsable(false)]
		//            public bool UserCanRelocate
		//            {
		//                get
		//                {
		//                    if (_fieldName == FieldNames.EntryLexicalForm.ToString() ||
		//                        _fieldName == LexSense.WellKnownProperties.Definition ||
		//                        _fieldName == FieldNames.ExampleSentence.ToString())
		//                    {
		//                        return false;
		//                    }
		//
		//                    return true;
		//                }
		//            }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the data type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("dataType")]
		[Description("The type of the field. E.g. multilingual text, option, option collection, relation.")]
		public string DataTypeName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the options list file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Description("For options and option collections, the name of the xml file containing the valid set of options.")]
		public string OptionsListFile
		{
			get
			{
				// This is about trying to get the win version to stop outputing
				// <optionsListfile>(return)</optionsListFile>(whereas mono doesn't)
				return (_optionsListFile == null ? null : _optionsListFile.Trim());
			}
			set { _optionsListFile = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the writing system ids. TODO: Turn off XmlIgnore when this is
		/// supported.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		[Browsable(false)]
		public IList<string> WritingSystemIds
		{
			get { return _writingSystemIds; }
			set
			{
				int i = value.IndexOf(null);
				if (i >= 0)
				{
					throw new ArgumentNullException("value",
						string.Format("Writing System argument {0} is null", i + 1));
				}

				_writingSystemIds = new List<string>(value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("description")]
		[Browsable(false)]
		public string Description { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the visibility.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public VisibilitySetting Visibility
		{
			get { return _visibility; }
			set { _visibility = value; }
		}

		//            public bool Enabled
		//            {
		//                get
		//                {
		//                    if (_enabledNotSet) //for backwards compatibility, before we added Enabled
		//                    {
		//                        Enabled = Visibility == CommonEnumerations.VisibilitySetting.Visible;
		//                    }
		//                    return _enabled;
		//                }
		//                set
		//                {
		//                    _enabled = value;
		//                    _enabledNotSet = false;
		//                }
		//            }

		//            public void ChangeWritingSystemId(string oldId, string newId)
		//            {
		//                int i = _writingSystemIds.FindIndex(delegate(string id) { return id == oldId; });
		//                if (i > -1)
		//                {
		//                    _writingSystemIds[i] = newId;
		//                }
		//            }

		/// <summary>
		/// built-in fields have properties which aren't user editable
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [show option list stuff]; otherwise, <c>false</c>.
		/// </value>
		//        [Browsable(false)]
		//        public bool CanOnlyEditDisplayName
		//        {
		//            get { return IsBuiltInViaCode; }
		//        }

		///  ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		[Browsable(false)]
		public bool ShowOptionListStuff
		{
			get
			{
				return DataTypeName == BuiltInDataType.Option.ToString() ||
					   DataTypeName == BuiltInDataType.OptionCollection.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the multiplicity.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public MultiplicityType Multiplicity
		{
			get { return _multiplicity; }
			set { _multiplicity = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether this instance is spell checking enabled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Description("Do you want words in this field to be spell checked?")]
		public bool IsSpellCheckingEnabled
		{
			get { return _isSpellCheckingEnabled; }
			set { _isSpellCheckingEnabled = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether this instance is multi paragraph.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsMultiParagraph
		{
			get { return _isMultiParagraph; }
			set { _isMultiParagraph = value; }
		}

		#endregion

		#region Misc. methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether [has writing system] [the specified writing system id].
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public bool HasWritingSystem(string writingSystemId)
		{
			return _writingSystemIds.Exists(s => s == writingSystemId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DisplayName;
		}

		#endregion
	}
}
