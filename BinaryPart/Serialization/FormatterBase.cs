using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal abstract class FormatterBase<THandledObject, TDerivedFormatter> where TDerivedFormatter : new()
	{
		internal const string xmlNS = "http://schemas.microsoft.com/office/onenote/2013/onenote";
		internal const string xmlNSPrefix = "one";

		internal virtual bool Deserialize(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			if (!DeserializeAttributes(reader, parent))
			{
				return false;
			}

			reader.ReadStartElement();

			if (!DeserializeChildren(reader, parent))
			{
				return false;
			}

			if (expectEndElement) reader.ReadEndElement();
			return true;
		}

		protected virtual bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			return true;
		}

		protected virtual bool DeserializeChildren(XmlReader reader, INode parent)
		{
			return true;
		}

		internal virtual bool Serialize(INode node, XmlWriter writer)
		{
			// The 'is' operator matches base classes. If this is too inclusive, we can resort to
			// type comparison.
			if (!(node is THandledObject)) return false;

			// TODO: add namespace
			writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
			SerializeAttributes(node, writer);
			SerializeChildren(node, writer);
			writer.WriteEndElement();

			return true;
		}

		protected virtual void SerializeAttributes(INode node, XmlWriter writer)
		{
		}

		protected virtual void SerializeChildren(INode node, XmlWriter writer)
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
