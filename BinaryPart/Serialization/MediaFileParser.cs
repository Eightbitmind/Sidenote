using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class MediaFileParser : ParserBase<MediaFileParser>
	{
		public MediaFileParser() : base("MediaFile") { }

		internal override bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore MediaFile elements for now
			reader.Skip();

			return true;
		}
	}
}
