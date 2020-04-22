using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionContentFormatter : FormatterBase<Section, SectionContentFormatter>
	{
		public SectionContentFormatter() : base("Section") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (PageEntryFormatter.Instance.Deserialize(reader, parent)) ;

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
