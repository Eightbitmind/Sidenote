using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class BulletListItemFormatter : FormatterBase<BulletListItemFormatter>
	{
		public BulletListItemFormatter() : base("Bullet") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			// TODO: read attributes

			((OutlineElement)parent).ListItem = new BulletListItem();
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
