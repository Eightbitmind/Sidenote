using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class ListFormatter : FormatterBase<NonexistentNode, ListFormatter>
	{
		public ListFormatter() : base("List") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			if (!(
				BulletListItemFormatter.Instance.Deserialize(reader, parent, patchStore) ||
				NumberedListItemFormatter.Instance.Deserialize(reader, parent, patchStore)
			))
			{
				throw new Exception("unexpected List child " + reader.LocalName);
			}

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
