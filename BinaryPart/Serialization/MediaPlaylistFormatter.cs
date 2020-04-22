using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class MediaPlaylistFormatter : FormatterBase<NonexistentNode, MediaPlaylistFormatter>
	{
		public MediaPlaylistFormatter() : base("MediaPlaylist") { }

		internal override bool Deserialize(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore MediaPlaylist elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			return false;
		}
	}
}
