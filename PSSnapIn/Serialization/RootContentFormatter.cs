using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class RootContentFormatter : IFormatter
	{
		public void Serialize(INode root, StringBuilder xml)
		{
			// TODO: implement
		}

		public bool Deserialize(INode root)
		{
			string childrenXml;
			ApplicationManager.Application.GetHierarchy(null, HierarchyScope.hsChildren, out childrenXml);
			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!ParseNotebooks(xmlReader, root))
			{
				Debug.Assert(false, "unexpected root content");
				return false;
			}

			return true;
		}

		// TODO: Replace with NotebookParser?
		private static bool ParseNotebooks(XmlReader reader, INode root)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Notebooks") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;
			reader.ReadStartElement();

			bool parsedAtLeastOneNotebook = false;

			while (ParseNotebook(reader, root))
			{
				parsedAtLeastOneNotebook = true;
			}

			Debug.Assert(parsedAtLeastOneNotebook);

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}

		private static bool ParseNotebook(XmlReader reader, INode root)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Notebook") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			string name = reader.GetAttribute("name");
			// string nickname = reader.GetAttribute("nickname");
			string id = reader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));

			var notebook = new Notebook(root, name, id, lastModifiedTime);
			root.Children.Add(notebook);

			reader.ReadStartElement();

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}
	}
}
