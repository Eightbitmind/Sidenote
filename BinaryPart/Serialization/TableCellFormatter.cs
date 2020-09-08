using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableCellFormatter : FormatterBase<TableCellFormatter>
	{
		public TableCellFormatter() : base("Cell") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is TableCell;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;
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
				parentNode.Depth + 1,
				parentNode,
				id,
				author,
				authorInitials,
				creationTime,
				lastModifiedTime,
				shadingColor);

			((Table)parent).AddCell(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			while (OEChildrenFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore)) ;
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}

		private TableCell deserializedObject;
	}
}
