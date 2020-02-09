using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEParser : ParserBase<OEParser>
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

		private OutlineElement outlineElement;
	}
}
