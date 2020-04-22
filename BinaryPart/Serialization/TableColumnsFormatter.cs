using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnsFormatter : FormatterBase<NonexistentNode, TableColumnsFormatter>
	{
		public TableColumnsFormatter() : base("Columns") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			int columnCount = 0;
			while (TableColumnFormatter.Instance.Deserialize(reader, parent)) ++columnCount;
			((Table)parent).ColumnCount = columnCount;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
