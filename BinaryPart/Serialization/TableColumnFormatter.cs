using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableColumnFormatter : FormatterBase<TableColumnFormatter>
	{
		public TableColumnFormatter() : base("Column") { }

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
