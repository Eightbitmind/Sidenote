using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PositionFormatter : FormatterBase<NonexistentNode, PositionFormatter>
	{
		public PositionFormatter() : base("Position") { }

		// ignore Position elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
