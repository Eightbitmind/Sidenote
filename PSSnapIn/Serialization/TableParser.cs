using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableParser : ParserBase<TableParser>
	{
		public TableParser() : base("Table") { }

		internal override bool Parse(XmlReader reader, Application app, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore Table elements for now
			reader.Skip();

			return true;
		}
	}
}
