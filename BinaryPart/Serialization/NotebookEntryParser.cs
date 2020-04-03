using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookEntryParser : ParserBase<Notebook, NotebookEntryParser>
	{
		public NotebookEntryParser() : base("Notebook") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string name = reader.GetAttribute("name");
			// string nickname = reader.GetAttribute("nickname");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");
			string color = reader.GetAttribute("color");

			var notebook = new Notebook(parent, name, id, lastModifiedTime, path, color);
			parent.Children.Add(notebook);

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
