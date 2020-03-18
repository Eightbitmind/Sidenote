using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NumberedListItemParser : ParserBase<NumberedListItemParser>
	{
		public NumberedListItemParser() : base("Number") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			// TODO: read more attributes
			string text = reader.GetAttribute("text");
			((OutlineElement)parent).ListItem = new NumberedListItem(text);
			return true;
		}
	}
}
