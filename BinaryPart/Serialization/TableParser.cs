using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableParser : ParserBase<TableParser>
	{
		public TableParser() : base("Table") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute("objectID");

			// TODO: Check schema. Examples don't have this attribute.
			string creationTimeAttribute = reader.GetAttribute("creationTime");
			DateTime creationTime = creationTimeAttribute != null ?
				DateTime.Parse(creationTimeAttribute) :
				new DateTime();

			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			
			bool bordersAreVisible = bool.Parse(reader.GetAttribute("bordersVisible"));
			bool hasHeaderRow = bool.Parse(reader.GetAttribute("hasHeaderRow"));

			this.table = new Table(
				parent.Depth + 1,
				parent,
				id,
				author: null, // TODO: Check schema
				authorInitials: null, // TODO: Check schema
				creationTime,
				lastModifiedTime,
				bordersAreVisible,
				hasHeaderRow);

			parent.Children.Add(this.table);

			return true;
		}

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			bool columnsParsed = false;
			int rowCount = 0;

			while (reader.IsStartElement())
			{

				if (!columnsParsed && TableColumnsParser.Instance.Parse(reader, this.table))
				{
					columnsParsed = true;
					continue;
				}

				if (TableRowParser.Instance.Parse(reader, this.table))
				{
					++rowCount;
				}
			}

			this.table.RowCount = rowCount;

			return true;
		}

		private Table table;
	}
}
