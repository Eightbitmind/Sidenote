using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PositionParser : ParserBase<NonexistentNode,PositionParser>
	{
		public PositionParser() : base("Position") { }

		// ignore Position elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
