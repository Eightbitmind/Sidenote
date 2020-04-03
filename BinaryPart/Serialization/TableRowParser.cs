using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableRowParser : ParserBase<NonexistentNode, TableRowParser>
	{
		public TableRowParser() : base("Row") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			((Table)parent).AddRow();
			while (TableCellParser.Instance.Parse(reader, parent)) ;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
