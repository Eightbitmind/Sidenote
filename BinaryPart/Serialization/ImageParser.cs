using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class ImageParser : ParserBase<NonexistentNode, ImageParser>
	{
		public ImageParser() : base("Image") { }

		internal override bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore Image elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
