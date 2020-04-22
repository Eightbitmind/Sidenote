using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SizeFormatter : FormatterBase<NonexistentNode, SizeFormatter>
	{
		public SizeFormatter() : base("Size") { }

		// ignore Size elements for now

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
