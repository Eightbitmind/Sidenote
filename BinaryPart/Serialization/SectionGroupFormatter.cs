using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionGroupFormatter : FormatterBase<NonexistentNode, SectionGroupFormatter>
	{
		public SectionGroupFormatter() : base("SectionGroup") { }

		internal override bool Deserialize(XmlReader reader, INode parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore SectionGroup elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
