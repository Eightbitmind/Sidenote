using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TextParser : ParserBase<TextParser>
	{
		public TextParser() : base("T") { }

		protected override bool ParseChildren(XmlReader reader, Application app, INode parent)
		{
			((OutlineElement)parent).SetText(reader.ReadContentAsString());
			return true;
		}
	}
}
