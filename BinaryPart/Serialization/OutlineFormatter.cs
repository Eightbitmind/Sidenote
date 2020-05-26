using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OutlineFormatter : FormatterBase<Outline, OutlineFormatter>
	{
		public OutlineFormatter() : base("Outline") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			string id = reader.GetAttribute("objectID");
			string author = reader.GetAttribute("author");
			string authorInitials = reader.GetAttribute("authorInitials");

			// lastModifiedBy
			// lastModifiedInitials

			// DateTime creationTime = DateTime.Parse(reader.GetAttribute("creationTime"));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));

			this.deserializedObject = new Outline(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				lastModifiedTime, // What constitutes the creationTime?
				lastModifiedTime);

			parent.Children.Add(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			while (reader.IsStartElement())
			{
				if (!(
					PositionFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					SizeFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					IndentsFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					OEChildrenFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore)
				))
				{
					throw new Exception("unexpected Outline child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			Outline serializedObject = (Outline)node;
			writer.WriteAttributeString("author", serializedObject.Author);
			writer.WriteAttributeString("authorInitials", serializedObject.AuthorInitials);
			writer.WriteAttributeString("lastModifiedTime", Converter.ToString(serializedObject.LastModifiedTime));
			writer.WriteAttributeString("objectID", serializedObject.ID);
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			// TODO: For full fidelity, we'd need to write Position, Size, Indents ...
			OEChildrenFormatter.Instance.Serialize(node, writer);
		}

		private Outline deserializedObject;
	}
}
