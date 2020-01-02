using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageParser : ParserBase<PageParser>
	{
		public PageParser() : base("Page") { }

		protected override bool ParseChildren(XmlReader reader, Application app, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					QuickStyleDefParser.Instance.Parse(reader, app, parent) ||
					TagDefParser.Instance.Parse(reader, app, parent) ||
					PageSettingsParser.Instance.Parse(reader, app, parent) ||
					TitleParser.Instance.Parse(reader, app, parent) ||
					OutlineParser.Instance.Parse(reader, app, parent) ||
					InkDrawingParser.Instance.Parse(reader, app, parent)
				))
				{
					throw new Exception("unexpected Page child " + reader.LocalName);
				}
			}

			return true;
		}
	}
}
