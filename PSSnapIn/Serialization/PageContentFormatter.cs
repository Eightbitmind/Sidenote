using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageContentFormatter : IFormatter
	{
		public void Serialize(INode parent, StringBuilder xml)
		{
			// TODO: implement
		}

		public bool Deserialize(INode page)
		{
			string pageXml;
			ApplicationManager.Application.GetPageContent(
				((IIdentifiableObject)page).ID,
				out pageXml,
				PageInfo.piBasic, // 'piBasic' is the default
				XMLSchema.xs2013);

			Debug.Assert(!string.IsNullOrEmpty(pageXml));
			var textReader = new StringReader(pageXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!PageParser.Instance.Parse(xmlReader, page))
			{
				Debug.Assert(false, "unexpected page content");
				return false;
			}

			return true;
		}
	}
}
