using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal abstract class ParserBase<TDerived> where TDerived : new()
	{
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

		protected ParserBase(string tagName)
		{
			this.tagName = tagName;
		}

		protected string tagName;

		internal static TDerived Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TDerived();
				}

				return instance;
			}
		}

		private static TDerived instance;
	}
}
