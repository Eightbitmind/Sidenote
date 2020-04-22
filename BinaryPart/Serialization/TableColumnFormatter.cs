using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnFormatter : FormatterBase<NonexistentNode, TableColumnFormatter>
	{
		public TableColumnFormatter() : base("Column") { }

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
