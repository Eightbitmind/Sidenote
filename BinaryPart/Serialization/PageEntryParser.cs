using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageEntryParser : ParserBase<PageEntryParser>
	{
		public PageEntryParser() : base("Page") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var creationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));


			parent.Children.Add(new Page(parent.Depth + 1, parent, name, id, creationTime, lastModifiedTime, pageLevel));

			return true;
		}
	}
}
