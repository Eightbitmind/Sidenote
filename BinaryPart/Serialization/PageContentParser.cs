using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageContentParser : ParserBase<PageContentParser>
	{
		public PageContentParser() : base("Page") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			// REVIEW: Any attributes on the page content we should add to the Page object?

			// old code:
			//string name = reader.GetAttribute("name");
			//string id = reader.GetAttribute("ID");
			//var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			//var dateTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			//var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			//this.page = new Page(parent.Depth + 1, parent, name, id, lastModifiedTime, dateTime, pageLevel);

			//parent.Children.Add(this.page);

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

		// private Page page;
	}
}
