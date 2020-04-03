using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class BulletListItemParser : ParserBase<NonexistentNode, BulletListItemParser>
	{
		public BulletListItemParser() : base("Bullet") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			// TODO: read attributes

			((OutlineElement)parent).ListItem = new BulletListItem();
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
