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
	internal class PageContentFormatter : IFormatter<IList<INode>>
	{
		public void Serialize(IList<INode> sections, StringBuilder xml)
		{
			// TODO: implement
		}

		public IList<INode> Deserialize(Application app, INode parent)
		{
			string pageXml;
			// app.GetHierarchy(parent.ID, HierarchyScope.hsChildren, out childrenXml);
			
			app.GetPageContent(
				parent.ID,
				out pageXml,
				PageInfo.piBasic, // 'piBasic' is the default
				XMLSchema.xs2013);

			Debug.Assert(!string.IsNullOrEmpty(pageXml));
			var textReader = new StringReader(pageXml);
			XmlReader xmlReader = XmlReader.Create(textReader);

			// ---

			List<INode> pageContent = new List<INode>();

			Debug.Assert(xmlReader.IsStartElement() && string.CompareOrdinal(xmlReader.LocalName, "Section") == 0);
			bool expectEndElement = !xmlReader.IsEmptyElement;
			Debug.Assert(xmlReader.Read());

			while (true)
			{
				INode contentItem;

				if (
					ParseQuickStyleDef(xmlReader, app, parent, out contentItem)
					// || ParsePageSettings()
					)
				{

					pageContent.Add(contentItem);
				}
				else
				{
					break;
				}
			}

			if (expectEndElement) xmlReader.ReadEndElement();

			return pageContent;
		}

		private bool ParseQuickStyleDef(XmlReader xmlReader, Application app, INode parent, out INode quickStyleDef)
		{
			quickStyleDef = null;

			if (!xmlReader.IsStartElement() || string.CompareOrdinal(xmlReader.LocalName, "QuickStyleDef") != 0)
			{
				return false;
			}

			bool expectEndElement = !xmlReader.IsEmptyElement;

			// ignore QuickStyleDefs for now

			if (expectEndElement) xmlReader.ReadEndElement();

			return true;
		}
	}
}
