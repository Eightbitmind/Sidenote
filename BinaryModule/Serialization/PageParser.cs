using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageParser : ParserBase<PageParser>
	{
		public PageParser() : base("Page") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					QuickStyleDefParser.Instance.Parse(reader, parent) ||
					TagDefParser.Instance.Parse(reader, parent) ||
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
	}
}
