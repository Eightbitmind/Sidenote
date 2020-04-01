using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionContentParser : ParserBase<SectionContentParser>
	{
		public SectionContentParser() : base("Section") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (PageEntryParser.Instance.Parse(reader, parent)) ;

			return true;
		}
	}
}
