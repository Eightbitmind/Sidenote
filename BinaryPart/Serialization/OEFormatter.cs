using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEFormatter : FormatterBase<OutlineElement, OEFormatter>
	{
		public OEFormatter() : base("OE") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute("objectID");
			string author = reader.GetAttribute("author");
			string authorInitials = reader.GetAttribute("authorInitials");

			// lastModifiedBy
			// lastModifiedInitials

			DateTime creationTime = DateTime.Parse(reader.GetAttribute("creationTime"));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute("lastModifiedTime"));
			string alignment = reader.GetAttribute("alignment");

			this.deserializedObject = new OutlineElement(
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
				this.deserializedObject.QuickStyleIndex = int.Parse(quickStyleIndexStr);
			}


			parent.Children.Add(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					TextFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					ListFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					TagFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					TableFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					MediaFileFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					ImageFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					InkParagraphFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					InkWordFormatter.Instance.Deserialize(reader, this.deserializedObject) ||
					OEChildrenFormatter.Instance.Deserialize(reader, this.deserializedObject)
				))
				{
					throw new Exception("unexpected OE child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			OutlineElement serializedObject = (OutlineElement)node;

			// TODO: serialize author, authorInitials ... if present

			writer.WriteAttributeString("creationTime", FormatDateTime(serializedObject.CreationTime));
			writer.WriteAttributeString("lastModifiedTime", FormatDateTime(serializedObject.LastModifiedTime));
			writer.WriteAttributeString("objectID", serializedObject.ID);
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			TextFormatter.Instance.Serialize(node, writer);
			OEChildrenFormatter.Instance.Serialize(node, writer);
		}

		private OutlineElement deserializedObject;
	}
}
