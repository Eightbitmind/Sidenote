using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NumberedListItemFormatter : FormatterBase<NumberedListItemFormatter>
	{
		public NumberedListItemFormatter() : base("Number") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			// TODO: read more attributes
			string text = reader.GetAttribute("text");
			((OutlineElement)parent).ListItem = new NumberedListItem(text);
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
