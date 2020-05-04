using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableCellFormatter : FormatterBase<TableCell, TableCellFormatter>
	{
		public TableCellFormatter() : base("Cell") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
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

			this.deserializedObject = new TableCell(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				creationTime,
				lastModifiedTime,
				shadingColor);

			((Table)parent).AddCell(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (OEChildrenFormatter.Instance.Deserialize(reader, this.deserializedObject)) ;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}

		private TableCell deserializedObject;
	}
}
