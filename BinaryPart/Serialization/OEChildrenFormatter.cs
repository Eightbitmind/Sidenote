using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEChildrenFormatter : FormatterBase<OutlineElement, OEChildrenFormatter>
	{
		public OEChildrenFormatter() : base("OEChildren") { }

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			while (reader.IsStartElement())
			{
				if (!(
					OEFormatter.Instance.Deserialize(reader, parent, patchStore)
				))
				{
					throw new Exception("unexpected OEChildren child element " + reader.LocalName);
				}
			}

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			if (!((obj is Outline) || (obj is OutlineElement))) return false;

			if (((INode)obj).Children.Count > 0)
			{
				writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
				SerializeAttributes(obj, writer);
				SerializeChildren(obj, writer);
				writer.WriteEndElement();
			}

			return true;
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			foreach(INode child in ((INode)obj).Children)
			{
				if (!(
					OEFormatter.Instance.Serialize(child, writer)
				))
				{
					throw new Exception("unexpected child node");
				}
			}
		}
	}
}
