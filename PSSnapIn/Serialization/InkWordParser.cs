using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class InkWordParser : ParserBase<InkWordParser>
	{
		public InkWordParser() : base("InkWord") { }

		internal override bool Parse(XmlReader reader, Application app, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore InkWord elements for now
			reader.Skip();

			return true;
		}
	}
}
