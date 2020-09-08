using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TagFormatter : FormatterBase<TagFormatter>
	{
		public TagFormatter() : base("Tag") { }

		internal override bool Deserialize(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore List elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
