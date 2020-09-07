using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnsFormatter : FormatterBase<NonexistentNode, TableColumnsFormatter>
	{
		public TableColumnsFormatter() : base("Columns") { }

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			int columnCount = 0;
			while (TableColumnFormatter.Instance.Deserialize(reader, parent, patchStore)) ++columnCount;
			((Table)parent).ColumnCount = columnCount;
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
