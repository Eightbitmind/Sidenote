using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OutlineFormatter : FormatterBase<Outline, OutlineFormatter>
	{
		public OutlineFormatter() : base("Outline") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
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

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					PositionFormatter.Instance.Deserialize(reader, this.outline) ||
					SizeFormatter.Instance.Deserialize(reader, this.outline) ||
					IndentsFormatter.Instance.Deserialize(reader, this.outline) ||
					OEChildrenFormatter.Instance.Deserialize(reader, this.outline)
				))
				{
					throw new Exception("unexpected Outline child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			Outline outline = (Outline)node;
			writer.WriteAttributeString("author", outline.Author);
			writer.WriteAttributeString("authorInitials", outline.AuthorInitials);
			writer.WriteAttributeString("lastModifiedTime", FormatDateTime(outline.LastModifiedTime));
			writer.WriteAttributeString("objectID", outline.ID);
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			// TODO: For full fidelity, we'd need to write Position, Size, Indents ...
			OEChildrenFormatter.Instance.Serialize(node, writer);
		}

		private Outline outline;
	}
}
