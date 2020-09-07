using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class AutomaticFormatter : FormatterBase<NonexistentNode, AutomaticFormatter>
	{
		public AutomaticFormatter() : base("Automatic") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			((PageSize)parent).IsAutomatic = true;
			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			if (((PageSize)obj).IsAutomatic)
			{
				writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
				writer.WriteEndElement();
				return true;
			}
			return false;
		}

	}
}
