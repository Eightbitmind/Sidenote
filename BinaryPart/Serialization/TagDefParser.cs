using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TagDefParser : ParserBase<NonexistentNode, TagDefParser>
	{
		public TagDefParser() : base("TagDef") { }

		// ignore TagDef elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			return false;
		}
	}
}
