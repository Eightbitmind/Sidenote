using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionContentFormatter : IFormatter
	{
		public void Serialize(INode parent, StringBuilder xml)
		{
			// TODO: implement
		}

		public bool Deserialize(INode section)
		{
			string childrenXml;
			ApplicationManager.Application.GetHierarchy(((IIdentifiableObject)section).ID, HierarchyScope.hsChildren, out childrenXml);
			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!ParseSectionContent(xmlReader, section))
			{
				Debug.Assert(false, "unexpected section content");
				return false;
			}

			return true;
		}

		// Replace with SectionContentParser?
		private static bool ParseSectionContent(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Section") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			reader.ReadStartElement();

			while (ParsePageEntry(reader, parent)) ;

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}

		// TODO: Replace with PageEntryParser?
		private static bool ParsePageEntry(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, "Page") != 0)
			{
				return false;
			}

			bool expectEndElement = !reader.IsEmptyElement;

			string name = reader.GetAttribute("name");
			string id = reader.GetAttribute("ID");
			var creationTime = DateTime.Parse(reader.GetAttribute("dateTime"));
			var lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			var pageLevel = uint.Parse(reader.GetAttribute("pageLevel"));

			reader.ReadStartElement();

			parent.Children.Add(new Page(parent.Depth + 1, parent, name, id, creationTime, lastModifiedTime, pageLevel));

			if (expectEndElement) reader.ReadEndElement();

			return true;
		}
	}
}
