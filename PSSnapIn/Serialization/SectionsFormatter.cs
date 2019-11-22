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
	internal class SectionsFormatter : IFormatter<IList<INode>>
	{
		public void Serialize(IList<INode> sections, StringBuilder xml)
		{
			// TODO: implement
		}

		public IList<INode> Deserialize(Application app, INode parent)
		{
			string childrenXml;
			app.GetHierarchy(parent.ID, HierarchyScope.hsChildren, out childrenXml);
			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);
			XmlReader xmlReader = XmlReader.Create(textReader);

			List<INode> sections = new List<INode>();

			Debug.Assert(xmlReader.IsStartElement() && string.CompareOrdinal(xmlReader.LocalName, "Notebook") == 0);
			bool expectEndElement = !xmlReader.IsEmptyElement;
			xmlReader.ReadStartElement();

			while (true)
			{
				ISection section;
				if (ParseSection(xmlReader, app, parent, out section))
				{
					sections.Add(section);
				}
				else if (ParseSectionGroup(xmlReader, app))
				{
					// ignore section groups for now
				}
				else
				{
					break;
				}
			}

			if (expectEndElement) xmlReader.ReadEndElement();

			return sections;
		}

		private bool ParseSection(XmlReader xmlReader, Application app, INode parent, out ISection section)
		{
			section = null;

			if (!xmlReader.IsStartElement() || string.CompareOrdinal(xmlReader.LocalName, "Section") != 0)
			{
				return false;
			}

			bool expectEndElement = !xmlReader.IsEmptyElement;

			string name = xmlReader.GetAttribute("name");
			string id = xmlReader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(xmlReader.GetAttribute("lastModifiedTime"));
			string path = xmlReader.GetAttribute("path");

			// TODO: find appropriate Color type and deserialize an instance
			string color = xmlReader.GetAttribute("color");

			xmlReader.ReadStartElement();

			section = new Section(app, parent, name, id, lastModifiedTime, path, color);

			if (expectEndElement) xmlReader.ReadEndElement();

			return true;
		}

		private bool ParseSectionGroup(XmlReader xmlReader, Application app)
		{
			if (!xmlReader.IsStartElement() || string.CompareOrdinal(xmlReader.LocalName, "SectionGroup") != 0)
			{
				return false;
			}

			bool expectEndElement = !xmlReader.IsEmptyElement;
			xmlReader.ReadStartElement();

			// ignore content of section groups for now
			ISection unused;
			while (ParseSection(xmlReader, null, null, out unused)) ;

			if (expectEndElement) xmlReader.ReadEndElement();
			return true;
		}
	}
}
