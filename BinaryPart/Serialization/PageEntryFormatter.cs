using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageEntryFormatter : FormatterBase<PageEntryFormatter>
	{
		public PageEntryFormatter() : base("Page") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Page;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var creationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			Page deserializedObject = new Page(parentNode.Depth + 1, parentNode, name, id, pageLevel);

			deserializedObject.EntryCreationTime = creationTime;
			deserializedObject.EntryLastModifiedTime = lastModifiedTime;

			parentNode.Children.Add(deserializedObject);

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
