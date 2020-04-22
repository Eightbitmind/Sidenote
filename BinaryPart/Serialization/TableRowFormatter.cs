using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableRowFormatter : FormatterBase<NonexistentNode, TableRowFormatter>
	{
		public TableRowFormatter() : base("Row") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			((Table)parent).AddRow();
			while (TableCellFormatter.Instance.Deserialize(reader, parent)) ;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
