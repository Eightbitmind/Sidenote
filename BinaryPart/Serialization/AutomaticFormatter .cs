using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class AutomaticFormatter : FormatterBase<NonexistentNode, AutomaticFormatter>
	{
		public AutomaticFormatter() : base("Automatic") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			((PageSize)parent).IsAutomatic = true;
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			if (((PageSize)node).IsAutomatic)
			{
				writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
				writer.WriteEndElement();
				return true;
			}
			return false;
		}

	}
}
