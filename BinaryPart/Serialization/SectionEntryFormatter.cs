using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionEntryFormatter : FormatterBase<SectionEntryFormatter>
	{
		public SectionEntryFormatter() : base("Section") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Section;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");

			// TODO: find appropriate Color type and deserialize an instance
			string color = reader.GetAttribute("color");

			parentNode.Children.Add(new Section(parentNode.Depth + 1, parentNode, name, id, lastModifiedTime, path, color));

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
