using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionContentParser : ParserBase<Section, SectionContentParser>
	{
		public SectionContentParser() : base("Section") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (PageEntryParser.Instance.Parse(reader, parent)) ;

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
