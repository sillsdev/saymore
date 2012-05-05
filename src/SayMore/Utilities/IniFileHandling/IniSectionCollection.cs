#region Copyright
//
// Nini Configuration Project.
// Copyright (C) 2006 Brent R. Matzelle.  All rights reserved.
//
// This software is published under the terms of the MIT X11 license, a copy of
// which has been included with this distribution in the LICENSE.txt file.
//
#endregion

using System;
using System.Collections;
using Nini.Util;

namespace Nini.Ini
{
	/// <include file='IniSectionCollection.xml' path='//Class[@name="IniSectionCollection"]/docs/*' />
	public class IniSectionCollection : ICollection, IEnumerable
	{
		#region Private variables
		OrderedList _sectionNames = new OrderedList ();
		#endregion

		#region Public properties
		/// <include file='IniSectionCollection.xml' path='//Property[@name="ItemIndex"]/docs/*' />
		public IniSection this[int index]
		{
			get { return (IniSection)_sectionNames[index]; }
		}

		/// <include file='IniSectionCollection.xml' path='//Property[@name="ItemName"]/docs/*' />
		public IniSection this[string configName]
		{
			get { return (IniSection)_sectionNames[configName]; }
		}

		/// <include file='IniSectionCollection.xml' path='//Property[@name="Count"]/docs/*' />
		public int Count
		{
			get { return _sectionNames.Count; }
		}

		/// <include file='IniSectionCollection.xml' path='//Property[@name="SyncRoot"]/docs/*' />
		public object SyncRoot
		{
			get { return _sectionNames.SyncRoot; }
		}

		/// <include file='IniSectionCollection.xml' path='//Property[@name="IsSynchronized"]/docs/*' />
		public bool IsSynchronized
		{
			get { return _sectionNames.IsSynchronized; }
		}
		#endregion

		#region Public methods
		/// <include file='IniSectionCollection.xml' path='//Method[@name="Add"]/docs/*' />
		public void Add (IniSection section)
		{
			if (_sectionNames.Contains (section)) {
				throw new ArgumentException ("IniSection already exists");
			}

			_sectionNames.Add (section.Name, section);
		}

		//added by hatton for Chorus
		public IniSection GetOrCreate(string name)
		{
			if (_sectionNames.Contains(name))
				return (IniSection)_sectionNames[name];
			var s = new IniSection(name);
			Add(s);
			return s;
		}

		/// <include file='IniSectionCollection.xml' path='//Method[@name="Remove"]/docs/*' />
		public void RemoveSection (string sectionName)
		{
			if(_sectionNames.Contains(sectionName))
				_sectionNames.Remove (sectionName);
		}

		/// <include file='IniSectionCollection.xml' path='//Method[@name="CopyTo"]/docs/*' />
		public void CopyTo (Array array, int index)
		{
			_sectionNames.CopyTo (array, index);
		}

		/// <include file='IniSectionCollection.xml' path='//Method[@name="CopyToStrong"]/docs/*' />
		public void CopyTo (IniSection[] array, int index)
		{
			((ICollection)_sectionNames).CopyTo (array, index);
		}

		/// <include file='IniSectionCollection.xml' path='//Method[@name="GetEnumerator"]/docs/*' />
		public IEnumerator GetEnumerator ()
		{
			return _sectionNames.GetEnumerator ();
		}
		#endregion

		#region Private methods
		#endregion
	}
}