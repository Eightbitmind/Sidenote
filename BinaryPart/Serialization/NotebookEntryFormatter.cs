using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookEntryFormatter : FormatterBase<NotebookEntryFormatter>
	{
		public NotebookEntryFormatter() : base("Notebook") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Notebook;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			string name = reader.GetAttribute("name");
			// string nickname = reader.GetAttribute("nickname");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");
			string color = reader.GetAttribute("color");

			var deserializedObject = new Notebook((INode)parent, name, id, lastModifiedTime, path, color);
			((INode)parent).Children.Add(deserializedObject);

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
