using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OutlineParser : ParserBase<OutlineParser>
	{
		public OutlineParser() : base("Outline") { }

		protected override bool ParseAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute("objectID");
			string author = reader.GetAttribute("author");
			string authorInitials = reader.GetAttribute("authorInitials");

			// lastModifiedBy
			// lastModifiedInitials

			// DateTime creationTime = DateTime.Parse(reader.GetAttribute("creationTime"));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));

			this.outline = new Outline(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				lastModifiedTime, // What constitutes the creationTime?
				lastModifiedTime);

			parent.Children.Add(this.outline);

			return true;
		}

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					PositionParser.Instance.Parse(reader, this.outline) ||
					SizeParser.Instance.Parse(reader, this.outline) ||
					IndentsParser.Instance.Parse(reader, this.outline) ||
					OEChildrenParser.Instance.Parse(reader, this.outline)
				))
				{
					throw new Exception("unexpected Outline child " + reader.LocalName);
				}
			}

			return true;
		}

		private Outline outline;
	}
}
