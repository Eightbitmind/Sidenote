using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class ListParser : ParserBase<ListParser>
	{
		public ListParser() : base("List") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			if (!(
				BulletListItemParser.Instance.Parse(reader, parent) ||
				NumberedListItemParser.Instance.Parse(reader, parent)
			))
			{
				throw new Exception("unexpected List child " + reader.LocalName);
			}

			return true;
		}
	}
}
