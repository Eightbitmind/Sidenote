using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal abstract class ParserBase<THandledObject, TDerivedParser> where TDerivedParser : new()
	{
		internal const string xmlNS = "http://schemas.microsoft.com/office/onenote/2013/onenote";
		internal const string xmlNSPrefix = "one";

		internal static string FormatDateTime(DateTime dateTime)
		{
			// https://en.wikipedia.org/wiki/ISO_8601#Combined_date_and_time_representations, "Z" - zero timezone
			return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}

		internal virtual bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			if (!ParseAttributes(reader, parent))
			{
				return false;
			}

			reader.ReadStartElement();

			if (!ParseChildren(reader, parent))
			{
				return false;
			}

			if (expectEndElement) reader.ReadEndElement();
			return true;
		}

		protected virtual bool ParseAttributes(XmlReader reader, INode parent)
		{
			return true;
		}

		protected virtual bool ParseChildren(XmlReader reader, INode parent)
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

		protected ParserBase(string tagName)
		{
			this.tagName = tagName;
		}

		protected string tagName;

		internal static TDerivedParser Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TDerivedParser();
				}

				return instance;
			}
		}

		private static TDerivedParser instance;
	}
}
