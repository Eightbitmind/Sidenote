using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal abstract class FormatterBase<THandledObject, TDerivedFormatter> where TDerivedFormatter : new()
	{
		internal const string xmlNS = "http://schemas.microsoft.com/office/onenote/2013/onenote";
		internal const string xmlNSPrefix = "one";

		internal virtual bool Deserialize(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			if (!DeserializeAttributes(reader, parent, patchStore))
			{
				return false;
			}

			reader.ReadStartElement();

			if (!DeserializeChildren(reader, parent, patchStore))
			{
				return false;
			}

			if (expectEndElement) reader.ReadEndElement();
			return true;
		}

		protected virtual bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			return true;
		}

		protected virtual bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			return true;
		}

		internal virtual bool Serialize(object obj, XmlWriter writer)
		{
			// The 'is' operator matches base classes. If this is too inclusive, we can resort to
			// type comparison.
			if (!(obj is THandledObject)) return false;

			// TODO: add namespace
			writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
			SerializeAttributes(obj, writer);
			SerializeChildren(obj, writer);
			writer.WriteEndElement();

			return true;
		}

		protected virtual void SerializeAttributes(object obj, XmlWriter writer)
		{
		}

		protected virtual void SerializeChildren(object obj, XmlWriter writer)
		{
		}

		protected FormatterBase(string tagName)
		{
			this.tagName = tagName;
		}

		protected string tagName;

		internal static TDerivedFormatter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TDerivedFormatter();
				}

				return instance;
			}
		}

		private static TDerivedFormatter instance;
	}
}
