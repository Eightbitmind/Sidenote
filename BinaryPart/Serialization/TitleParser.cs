using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TitleParser : ParserBase<Title, TitleParser>
	{
		public TitleParser() : base("Title") { }

		// protected override bool ParseAttributes(XmlReader reader, Application app, INode parent)
		// {
		//	// Do something with 'lang' attribute?
		// }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			Title title = new Title(parent.Depth + 1, parent);

			if (!OEParser.Instance.Parse(reader, title))
			{
				throw new Exception("Title element missing OE");
			}

			parent.Children.Add(title);
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			// return false;
			// return true;

			return (node is Title);
		}
	}
}
