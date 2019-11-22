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
	internal class PagesFormatter : IFormatter<IList<INode>>
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

			List<INode> pages = new List<INode>();

			Debug.Assert(xmlReader.IsStartElement() && string.CompareOrdinal(xmlReader.LocalName, "Section") == 0);
			bool expectEndElement = !xmlReader.IsEmptyElement;
			Debug.Assert(xmlReader.Read());

			while (true)
			{
				IPage page;
				if (ParsePage(xmlReader, app, parent, out page))
				{
					pages.Add(page);
				}
				else
				{
					break;
				}
			}

			if (expectEndElement) xmlReader.ReadEndElement();

			return pages;
		}

		private bool ParsePage(XmlReader xmlReader, Application app, INode parent, out IPage page)
		{
			page = null;

			if (!xmlReader.IsStartElement() || string.CompareOrdinal(xmlReader.LocalName, "Page") != 0)
			{
				return false;
			}

			bool expectEndElement = !xmlReader.IsEmptyElement;

			string name = xmlReader.GetAttribute("name");
			string id = xmlReader.GetAttribute("ID");
			var lastModifiedTime = DateTime.Parse(xmlReader.GetAttribute("lastModifiedTime"));
			var dateTime = DateTime.Parse(xmlReader.GetAttribute("dateTime"));
			var pageLevel = uint.Parse(xmlReader.GetAttribute("pageLevel"));

			xmlReader.ReadStartElement();

			page = new Page(app, parent, name, id, lastModifiedTime, dateTime, pageLevel);

			if (expectEndElement) xmlReader.ReadEndElement();

			return true;
		}
	}
}
