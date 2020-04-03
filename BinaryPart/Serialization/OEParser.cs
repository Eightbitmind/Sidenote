using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEParser : ParserBase<OutlineElement, OEParser>
	{
		public OEParser() : base("OE") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute("objectID");
			string author = reader.GetAttribute("author");
			string authorInitials = reader.GetAttribute("authorInitials");

			// lastModifiedBy
			// lastModifiedInitials

			DateTime creationTime = DateTime.Parse(reader.GetAttribute("creationTime"));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string alignment = reader.GetAttribute("alignment");

			this.outlineElement = new OutlineElement(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				creationTime,
				lastModifiedTime,
				alignment);

			string quickStyleIndexStr = reader.GetAttribute("quickStyleIndex");

			if (!string.IsNullOrEmpty(quickStyleIndexStr))
			{
				this.outlineElement.QuickStyleIndex = int.Parse(quickStyleIndexStr);
			}


			parent.Children.Add(this.outlineElement);

			return true;
		}

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					TextParser.Instance.Parse(reader, this.outlineElement) ||
					ListParser.Instance.Parse(reader, this.outlineElement) ||
					TagParser.Instance.Parse(reader, this.outlineElement) ||
					TableParser.Instance.Parse(reader, this.outlineElement) ||
					MediaFileParser.Instance.Parse(reader, this.outlineElement) ||
					ImageParser.Instance.Parse(reader, this.outlineElement) ||
					InkParagraphParser.Instance.Parse(reader, this.outlineElement) ||
					InkWordParser.Instance.Parse(reader, this.outlineElement) ||
					OEChildrenParser.Instance.Parse(reader, this.outlineElement)
				))
				{
					throw new Exception("unexpected OE child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			OutlineElement outlineElement = (OutlineElement)node;

			// TODO: serialize author, authorInitials ... if present

			writer.WriteAttributeString("creationTime", FormatDateTime(outlineElement.CreationTime));
			writer.WriteAttributeString("lastModifiedTime", FormatDateTime(outlineElement.LastModifiedTime));
			writer.WriteAttributeString("objectID", outlineElement.ID);
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			TextParser.Instance.Serialize(node, writer);
			OEChildrenParser.Instance.Serialize(node, writer);
		}

		private OutlineElement outlineElement;
	}
}
