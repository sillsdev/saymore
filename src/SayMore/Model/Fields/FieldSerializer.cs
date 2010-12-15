using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SayMore.Model.Fields
{
	/// ----------------------------------------------------------------------------------------
	public interface IXmlFieldSerializer
	{
		object Deserialize(string xmlBlob);
		XElement Serialize(object obj);
		string ElementName { get; }
	}

	/// ----------------------------------------------------------------------------------------
	public abstract class FieldSerializer : IXmlFieldSerializer
	{
		public string ElementName { get; protected set; }

		/// ------------------------------------------------------------------------------------
		protected FieldSerializer(string elementName)
		{
			ElementName = elementName;
		}

		#region IXmlFieldSerializer Members
		/// ------------------------------------------------------------------------------------
		public virtual object Deserialize(string xmlBlob)
		{
			throw new NotImplementedException("Must override in derived class");
		}

		/// ------------------------------------------------------------------------------------
		public virtual XElement Serialize(object obj)
		{
			throw new NotImplementedException("Must override in derived class");
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected XElement GetElementFromXml(string xmlBlob)
		{
			return XElement.Load(XmlReader.Create(new StringReader(xmlBlob)));
		}

		/// ------------------------------------------------------------------------------------
		protected XElement InternalSerialize(object obj, Type expectedObjType,
			Func<XElement, XElement> elementBuilder)
		{
			var element = new XElement(ElementName);

			if (obj == null)
				return element;

			if (!expectedObjType.IsAssignableFrom(obj.GetType()))
			{
				var msg = string.Format("obj is not of type '{0}'.", expectedObjType);
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(msg);
				return element;
			}

			return elementBuilder(element);
		}
	}
}
