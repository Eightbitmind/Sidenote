using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TagDefFormatter : FormatterBase<TagDefFormatter>
	{
		public TagDefFormatter() : base("TagDef") { }

		// ignore TagDef elements for now

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			return false;
		}
	}
}
