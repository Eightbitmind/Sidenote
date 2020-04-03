using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class QuickStyleDefParser : ParserBase<NonexistentNode, QuickStyleDefParser>
	{
		public QuickStyleDefParser() : base("QuickStyleDef") { }

		// ignore QuickStyleDef elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			return false;
		}
	}
}
