using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class InkWordFormatter : FormatterBase<NonexistentNode, InkWordFormatter>
	{
		public InkWordFormatter() : base("InkWord") { }

		internal override bool Deserialize(XmlReader reader, INode parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore InkWord elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
