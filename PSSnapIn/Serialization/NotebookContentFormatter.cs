using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookContentFormatter : IFormatter
	{
		public void Serialize(INode parent, StringBuilder xml)
		{
			// TODO: implement
		}

		public bool Deserialize(Application app, INode notebook)
		{
			string childrenXml;
			app.GetHierarchy(((IIdentifiableObject)notebook).ID, HierarchyScope.hsChildren, out childrenXml);
			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!ParseNotebookContent(xmlReader, app, notebook))
			{
				Debug.Assert(false, "unexpected notebook content");
				return false;
			}

			return true;
		}

		private static bool ParseNotebookContent(XmlReader reader, Application app, INode notebook)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Notebook") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;
			reader.ReadStartElement();

			while (
				ParseSection(reader, app, notebook) ||
				ParseSectionGroup(reader, app)
			) ;

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}

		private static bool ParseSection(XmlReader reader, Application app, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Section") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string path = reader.GetAttribute("path");

			// TODO: find appropriate Color type and deserialize an instance
			string color = reader.GetAttribute("color");

			reader.ReadStartElement();

			parent.Children.Add(new Section(app, parent, name, id, lastModifiedTime, path, color));

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}

		private static bool ParseSectionGroup(XmlReader reader, Application app)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "SectionGroup") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;
			reader.ReadStartElement();

			// ignore content of section groups for now
			Node unused = new Node(app, null);
			while (ParseSection(reader, null, unused)) ;

			if (expectEndElement) reader.ReadEndElement();
			return true;
		}
	}
}
