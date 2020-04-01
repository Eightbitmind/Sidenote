using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionEntryParser : ParserBase<SectionEntryParser>
	{
		public SectionEntryParser() : base("Section") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");

			// TODO: find appropriate Color type and deserialize an instance
			string color = reader.GetAttribute("color");

			parent.Children.Add(new Section(parent.Depth + 1, parent, name, id, lastModifiedTime, path, color));

			return true;
		}
	}
}
