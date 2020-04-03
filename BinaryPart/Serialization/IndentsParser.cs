using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class IndentsParser : ParserBase<NonexistentNode, IndentsParser>
	{
		public IndentsParser() : base("Indents") { }

		internal override bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore Indents elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
