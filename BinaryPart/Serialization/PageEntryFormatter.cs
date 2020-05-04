using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageEntryFormatter : FormatterBase<Page, PageEntryFormatter>
	{
		public PageEntryFormatter() : base("Page") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var creationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			Page deserializedObject = new Page(parent.Depth + 1, parent, name, id, pageLevel);

			deserializedObject.EntryCreationTime = creationTime;
			deserializedObject.EntryLastModifiedTime = lastModifiedTime;

			parent.Children.Add(deserializedObject);

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
