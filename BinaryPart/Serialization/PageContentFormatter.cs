using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageContentFormatter : FormatterBase<Page, PageContentFormatter>
	{
		public PageContentFormatter() : base("Page") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			Page deserializedObject = (Page)parent;

			deserializedObject.CreationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			deserializedObject.LastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			deserializedObject.Language = reader.GetAttribute("lang");

			// REVIEW: Any other attributes on the page content we should add to the Page object?
			// old code:
			// string name = reader.GetAttribute("name");
			// string id = reader.GetAttribute("ID");
			// var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			while (reader.IsStartElement())
			{
				if (!(
					QuickStyleFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					TagDefFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					MediaPlaylistFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					PageSettingsFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					TitleFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					OutlineFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					InkDrawingFormatter.Instance.Deserialize(reader, parent, patchStore)
				))
				{
					throw new Exception("unexpected Page child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			Page serializedObject = (Page)node;
			writer.WriteAttributeString("ID", serializedObject.ID);
			writer.WriteAttributeString("name", serializedObject.Name);
			writer.WriteAttributeString("dateTime", Converter.ToString(serializedObject.CreationTime));
			writer.WriteAttributeString("lastModifiedTime", Converter.ToString(serializedObject.LastModifiedTime));
			writer.WriteAttributeString("pageLevel", serializedObject.PageLevel.ToString());

			if (!string.IsNullOrEmpty(serializedObject.Language))
			{
				writer.WriteAttributeString("lang", serializedObject.Language);
			}
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach(INode child in node.Children)
			{
				if (!(
					QuickStyleFormatter.Instance.Serialize(child, writer) ||
					TagDefFormatter.Instance.Serialize(child, writer) ||
					MediaPlaylistFormatter.Instance.Serialize(child, writer) ||
					PageSettingsFormatter.Instance.Serialize(child, writer) ||
					TitleFormatter.Instance.Serialize(child, writer) ||
					OutlineFormatter.Instance.Serialize(child, writer) ||
					InkDrawingFormatter.Instance.Serialize(child, writer)
				))
				{
					throw new Exception("unexpected Page child " + child.Type);
				}
			}
		}
	}
}
