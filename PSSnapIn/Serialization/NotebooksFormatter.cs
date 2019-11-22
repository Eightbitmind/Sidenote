using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebooksFormatter : IFormatter<IRoot>
	{
		public void Serialize(IRoot root, StringBuilder xml)
		{
			// TODO: implement
		}

		public IRoot Deserialize(Application app, INode parent)
		{
			string childrenXml;
			app.GetHierarchy(null, HierarchyScope.hsChildren, out childrenXml);
			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;

			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			IList<INotebook> notebooks = new List<INotebook>();

			Debug.Assert(xmlReader.IsStartElement() && string.CompareOrdinal(xmlReader.LocalName, "Notebooks") == 0);
			bool expectNotebooksClosingTag = !xmlReader.IsEmptyElement;

			Debug.Assert(xmlReader.Read());

			// How to position onto the first Section element?

			while (xmlReader.IsStartElement() && string.CompareOrdinal(xmlReader.LocalName, "Notebook") == 0)
			{
				bool expectEndElement = !xmlReader.IsEmptyElement;

				string name = xmlReader.GetAttribute("name");
				// string nickname = xmlReader.GetAttribute("nickname");
				string id = xmlReader.GetAttribute("ID");
				var lastModifiedTime = DateTime.Parse(xmlReader.GetAttribute("lastModifiedTime"));

				var notebook = new Notebook(app, parent, name, id, lastModifiedTime);
				notebooks.Add(notebook);

				Debug.Assert(xmlReader.Read());
				if (expectEndElement) xmlReader.ReadEndElement();
			}

			if (expectNotebooksClosingTag) xmlReader.ReadEndElement();

			IRoot root = new Root(notebooks);
			return root;
		}
	}
}
