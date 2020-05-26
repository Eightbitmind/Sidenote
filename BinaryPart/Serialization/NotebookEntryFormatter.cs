using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookEntryFormatter : FormatterBase<Notebook, NotebookEntryFormatter>
	{
		public NotebookEntryFormatter() : base("Notebook") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			string name = reader.GetAttribute("name");
			// string nickname = reader.GetAttribute("nickname");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");
			string color = reader.GetAttribute("color");

			var deserializedObject = new Notebook(parent, name, id, lastModifiedTime, path, color);
			parent.Children.Add(deserializedObject);

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
