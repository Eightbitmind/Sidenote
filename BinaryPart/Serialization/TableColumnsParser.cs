using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnsParser : ParserBase<NonexistentNode, TableColumnsParser>
	{
		public TableColumnsParser() : base("Columns") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			int columnCount = 0;
			while (TableColumnParser.Instance.Parse(reader, parent)) ++columnCount;
			((Table)parent).ColumnCount = columnCount;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
