using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableCellParser : ParserBase<TableCellParser>
	{
		public TableCellParser() : base("Cell") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute("objectID");
			string author = reader.GetAttribute("author");
			string authorInitials = reader.GetAttribute("authorInitials");

			string creationTimeAttribute = reader.GetAttribute("creationTime");
			DateTime creationTime = creationTimeAttribute != null ?
				DateTime.Parse(creationTimeAttribute) :
				new DateTime();

			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));

			string shadingColor = reader.GetAttribute("shadingColor");

			this.tableCell = new TableCell(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				creationTime,
				lastModifiedTime,
				shadingColor);

			((Table)parent).AddCell(this.tableCell);

			return true;
		}

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (OEChildrenParser.Instance.Parse(reader, this.tableCell)) ;
			return true;
		}

		private TableCell tableCell;
	}
}
