using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnParser : ParserBase<NonexistentNode, TableColumnParser>
	{
		public TableColumnParser() : base("Column") { }

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
