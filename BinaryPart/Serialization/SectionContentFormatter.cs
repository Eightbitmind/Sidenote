using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SectionContentFormatter : FormatterBase<SectionContentFormatter>
	{
		public SectionContentFormatter() : base("Section") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Section;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			while (PageEntryFormatter.Instance.Deserialize(reader, parent, patchStore)) ;

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
