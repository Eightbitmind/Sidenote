﻿using Sidenote.DOM;
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
			ApplicationManager.Application.GetHierarchy(
				null,
				Microsoft.Office.Interop.OneNote.HierarchyScope.hsChildren,
				out childrenXml);

			Debug.Assert(!string.IsNullOrEmpty(childrenXml));
			var textReader = new StringReader(childrenXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!NotebooksParser.Instance.Parse(xmlReader, root))
			{
				Debug.Assert(false, "unexpected root content");
				return false;
			}

			return true;
		}
	}
}
