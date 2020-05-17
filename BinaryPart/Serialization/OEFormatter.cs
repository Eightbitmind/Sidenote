using Sidenote.DOM;
using System;
using System.Management.Automation;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEFormatter : FormatterBase<OutlineElement, OEFormatter>
	{
		public OEFormatter() : base("OE") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			string id = reader.GetAttribute(ObjectIdAttributeName);
			string author = reader.GetAttribute(AuthorAttributeName);
			string authorInitials = reader.GetAttribute(AuthorInitialsAttributeName);

			DateTime creationTime = DateTime.Parse(reader.GetAttribute(CreationTimeAttributeName));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute(LastModifiedTimeAttributeName));
			string alignment = reader.GetAttribute(AlignmentAttributeName);

			this.deserializedObject = new OutlineElement(
				parent.Depth + 1,
				parent,
				id,
				author,
				authorInitials,
				creationTime,
				lastModifiedTime,
				alignment);

			string lastModifiedBy = reader.GetAttribute(LastModifiedByAttributeName);
			if (!string.IsNullOrEmpty(lastModifiedBy))
			{
				this.deserializedObject.LastModifiedBy = lastModifiedBy;
			}

			string lastModifiedByInitials = reader.GetAttribute(LastModifiedByInitialsAttributeName);
			if (!string.IsNullOrEmpty(lastModifiedByInitials))
			{
				this.deserializedObject.LastModifiedByInitials = lastModifiedByInitials;
			}

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

			writer.WriteAttributeString(AlignmentAttributeName, serializedObject.Alignment);
			writer.WriteAttributeString(AuthorAttributeName, serializedObject.Author);
			writer.WriteAttributeString(AuthorInitialsAttributeName, serializedObject.AuthorInitials);
			writer.WriteAttributeString(CreationTimeAttributeName, Converter.ToString(serializedObject.CreationTime));
			writer.WriteAttributeString(LastModifiedTimeAttributeName, Converter.ToString(serializedObject.LastModifiedTime));
			writer.WriteAttributeString(LastModifiedByAttributeName, serializedObject.LastModifiedBy);
			writer.WriteAttributeString(LastModifiedByInitialsAttributeName, serializedObject.LastModifiedByInitials);
			writer.WriteAttributeString(ObjectIdAttributeName, serializedObject.ID);
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			TextFormatter.Instance.Serialize(node, writer);
			OEChildrenFormatter.Instance.Serialize(node, writer);
		}

		private static string AlignmentAttributeName = "alignment";
		private static string AuthorAttributeName = "author";
		private static string AuthorInitialsAttributeName = "authorInitials";
		private static string ObjectIdAttributeName = "objectID";
		private static string CreationTimeAttributeName = "creationTime";
		private static string LastModifiedTimeAttributeName = "lastModifiedTime";
		private static string LastModifiedByAttributeName = "lastModifiedBy";
		private static string LastModifiedByInitialsAttributeName = "lastModifiedByInitials";

		private OutlineElement deserializedObject;
	}
}
