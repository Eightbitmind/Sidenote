using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class MediaFileFormatter : FormatterBase<NonexistentNode, MediaFileFormatter>
	{
		public MediaFileFormatter() : base("MediaFile") { }

		internal override bool Deserialize(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore MediaFile elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
