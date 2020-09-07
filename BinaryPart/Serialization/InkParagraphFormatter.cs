using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class InkParagraphFormatter : FormatterBase<NonexistentNode, InkParagraphFormatter>
	{
		public InkParagraphFormatter() : base("InkParagraph") { }

		internal override bool Deserialize(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore InkParagraph elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
