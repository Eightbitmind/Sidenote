using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal abstract class ParserBase<TDerived> where TDerived : new()
	{
		internal virtual bool Parse(XmlReader reader, Application app, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			if (!ParseAttributes(reader, app, parent))
			{
				return false;
			}

			reader.ReadStartElement();

			if (!ParseChildren(reader, app, parent))
			{
				return false;
			}

			if (expectEndElement) reader.ReadEndElement();
			return true;
		}

		protected virtual bool ParseAttributes(XmlReader reader, Application app, INode parent)
		{
			return true;
		}

		protected virtual bool ParseChildren(XmlReader reader, Application app, INode parent)
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
