using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class MediaPlaylistParser : ParserBase<MediaPlaylistParser>
	{
		public MediaPlaylistParser() : base("MediaPlaylist") { }

		internal override bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore MediaPlaylist elements for now
			reader.Skip();

			return true;
		}
	}
}
