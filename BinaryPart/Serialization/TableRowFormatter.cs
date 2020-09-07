using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableRowFormatter : FormatterBase<NonexistentNode, TableRowFormatter>
	{
		public TableRowFormatter() : base("Row") { }

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			((Table)parent).AddRow();
			while (TableCellFormatter.Instance.Deserialize(reader, parent, patchStore)) ;
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
