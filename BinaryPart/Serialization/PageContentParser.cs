using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageContentParser : ParserBase<Page, PageContentParser>
	{
		public PageContentParser() : base("Page") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			Page page = (Page)parent;

			var dateTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));

			page.CreationTime = dateTime;
			page.LastModifiedTime = lastModifiedTime;

			// REVIEW: Any other attributes on the page content we should add to the Page object?
			// old code:
			// string name = reader.GetAttribute("name");
			// string id = reader.GetAttribute("ID");
			// var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			return true;
		}

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					QuickStyleDefParser.Instance.Parse(reader, parent) ||
					TagDefParser.Instance.Parse(reader,parent) ||
					MediaPlaylistParser.Instance.Parse(reader, parent) ||
					PageSettingsParser.Instance.Parse(reader, parent) ||
					TitleParser.Instance.Parse(reader, parent) ||
					OutlineParser.Instance.Parse(reader, parent) ||
					InkDrawingParser.Instance.Parse(reader, parent)
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
			writer.WriteAttributeString("lastModifiedTime", FormatDateTime(page.CreationTime));
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach(INode child in node.Children)
			{
				if (!(
					QuickStyleDefParser.Instance.Serialize(child, writer) ||
					TagDefParser.Instance.Serialize(child, writer) ||
					MediaPlaylistParser.Instance.Serialize(child, writer) ||
					PageSettingsParser.Instance.Serialize(child, writer) ||
					TitleParser.Instance.Serialize(child, writer) ||
					OutlineParser.Instance.Serialize(child, writer) ||
					InkDrawingParser.Instance.Serialize(child, writer)
				))
				{
					throw new Exception("unexpected Page child " + child.Type);
				}
			}
		}
	}
}
