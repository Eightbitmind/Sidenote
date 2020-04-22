using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionEntryFormatter : FormatterBase<Section, SectionEntryFormatter>
	{
		public SectionEntryFormatter() : base("Section") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
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

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
