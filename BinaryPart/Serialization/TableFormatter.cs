using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TableFormatter : FormatterBase<Table, TableFormatter>
	{
		public TableFormatter() : base("Table") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			string id = reader.GetAttribute("objectID");

			// TODO: Check schema. Examples don't have this attribute.
			string creationTimeAttribute = reader.GetAttribute("creationTime");
			DateTime creationTime = creationTimeAttribute != null ?
				DateTime.Parse(creationTimeAttribute) :
				new DateTime();

			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			
			bool bordersAreVisible = bool.Parse(reader.GetAttribute("bordersVisible"));
			bool hasHeaderRow = bool.Parse(reader.GetAttribute("hasHeaderRow"));

			this.deserializedObject = new Table(
				parentNode.Depth + 1,
				parentNode,
				id,
				author: null, // TODO: Check schema
				authorInitials: null, // TODO: Check schema
				creationTime,
				lastModifiedTime,
				bordersAreVisible,
				hasHeaderRow);

			parentNode.Children.Add(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			bool columnsParsed = false;
			int rowCount = 0;

			while (reader.IsStartElement())
			{

				if (!columnsParsed && TableColumnsFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore))
				{
					columnsParsed = true;
					continue;
				}

				if (TableRowFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore))
				{
					++rowCount;
				}
			}

			this.deserializedObject.RowCount = rowCount;

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}

		private Table deserializedObject;
	}
}
