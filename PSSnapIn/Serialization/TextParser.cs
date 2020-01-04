using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TextParser : ParserBase<TextParser>
	{
		public TextParser() : base("T") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			((OutlineElement)parent).SetText(reader.ReadContentAsString());
			return true;
		}
	}
}
