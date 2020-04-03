using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SizeParser : ParserBase<NonexistentNode, SizeParser>
	{
		public SizeParser() : base("Size") { }

		// ignore Size elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
