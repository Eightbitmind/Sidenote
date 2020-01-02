using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEChildrenParser : ParserBase<OEChildrenParser>
	{
		public OEChildrenParser() : base("OEChildren") { }

		protected override bool ParseChildren(XmlReader reader, Application app, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					OEParser.Instance.Parse(reader, app, parent)
				))
				{
					throw new Exception("unexpected OEChildren child element " + reader.LocalName);
				}
			}

			return true;
		}
	}
}
