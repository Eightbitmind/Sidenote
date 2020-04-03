﻿using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEChildrenParser : ParserBase<OutlineElement, OEChildrenParser>
	{
		public OEChildrenParser() : base("OEChildren") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					OEParser.Instance.Parse(reader, parent)
				))
				{
					throw new Exception("unexpected OEChildren child element " + reader.LocalName);
				}
			}

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			if (!((node is Outline) || (node is OutlineElement))) return false;

			if (node.Children.Count > 0)
			{
				writer.WriteStartElement(xmlNSPrefix, this.tagName, xmlNS);
				SerializeAttributes(node, writer);
				SerializeChildren(node, writer);
				writer.WriteEndElement();
			}

			return true;
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach(INode child in node.Children)
			{
				if (!(
					OEParser.Instance.Serialize(child, writer)
				))
				{
					throw new Exception("unexpected child node");
				}

			}
		}
	}
}
