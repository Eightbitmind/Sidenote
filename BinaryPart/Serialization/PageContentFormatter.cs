using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageContentFormatter : FormatterBase<Page, PageContentFormatter>
	{
		public PageContentFormatter() : base("Page") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			Page page = (Page)parent;

			page.CreationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			page.LastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			page.Language = reader.GetAttribute("lang");

			// REVIEW: Any other attributes on the page content we should add to the Page object?
			// old code:
			// string name = reader.GetAttribute("name");
			// string id = reader.GetAttribute("ID");
			// var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					QuickStyleDefFormatter.Instance.Deserialize(reader, parent) ||
					TagDefFormatter.Instance.Deserialize(reader,parent) ||
					MediaPlaylistFormatter.Instance.Deserialize(reader, parent) ||
					PageSettingsFormatter.Instance.Deserialize(reader, parent) ||
					TitleFormatter.Instance.Deserialize(reader, parent) ||
					OutlineFormatter.Instance.Deserialize(reader, parent) ||
					InkDrawingFormatter.Instance.Deserialize(reader, parent)
				))
				{
					throw new Exception("unexpected Page child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			Page page = (Page)node;
			writer.WriteAttributeString("ID", page.ID);
			writer.WriteAttributeString("name", page.Name);
			writer.WriteAttributeString("dateTime", FormatDateTime(page.CreationTime));
			writer.WriteAttributeString("lastModifiedTime", FormatDateTime(page.LastModifiedTime));
			writer.WriteAttributeString("pageLevel", page.PageLevel.ToString());

			if (!string.IsNullOrEmpty(page.Language))
			{
				writer.WriteAttributeString("lang", page.Language);
			}
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach(INode child in node.Children)
			{
				if (!(
					QuickStyleDefFormatter.Instance.Serialize(child, writer) ||
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
