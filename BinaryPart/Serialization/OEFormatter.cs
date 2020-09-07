using Sidenote.DOM;
using System;
using System.Management.Automation;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEFormatter : FormatterBase<OutlineElement, OEFormatter>
	{
		public OEFormatter() : base("OE") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			string id = reader.GetAttribute(ObjectIdAttributeName);
			string author = reader.GetAttribute(AuthorAttributeName);
			string authorInitials = reader.GetAttribute(AuthorInitialsAttributeName);

			DateTime creationTime = DateTime.Parse(reader.GetAttribute(CreationTimeAttributeName));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute(LastModifiedTimeAttributeName));
			string alignment = reader.GetAttribute(AlignmentAttributeName);

			this.deserializedObject = new OutlineElement(
				parentNode.Depth + 1,
				parentNode,
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

			parentNode.Children.Add(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object obj, PatchStore patchStore)
		{
			while (reader.IsStartElement())
			{
				if (!(
					TextFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					ListFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					TagFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					TableFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					MediaFileFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					ImageFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					InkParagraphFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					InkWordFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
					OEChildrenFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore)
				))
				{
					throw new Exception("unexpected OE child " + reader.LocalName);
				}
			}

			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			OutlineElement serializedObject = (OutlineElement)obj;

			writer.WriteAttributeString(AlignmentAttributeName, serializedObject.Alignment);

			if (!string.IsNullOrEmpty(serializedObject.Author))
			{
				writer.WriteAttributeString(AuthorAttributeName, serializedObject.Author);
			}

			if (!string.IsNullOrEmpty(serializedObject.AuthorInitials))
			{
				writer.WriteAttributeString(AuthorInitialsAttributeName, serializedObject.AuthorInitials);
			}

			writer.WriteAttributeString(CreationTimeAttributeName, Converter.ToString(serializedObject.CreationTime));
			writer.WriteAttributeString(LastModifiedTimeAttributeName, Converter.ToString(serializedObject.LastModifiedTime));

			if (!string.IsNullOrEmpty(serializedObject.LastModifiedBy))
			{
				writer.WriteAttributeString(LastModifiedByAttributeName, serializedObject.LastModifiedBy);
			}

			if (!string.IsNullOrEmpty(serializedObject.LastModifiedByInitials))
			{
				writer.WriteAttributeString(LastModifiedByInitialsAttributeName, serializedObject.LastModifiedByInitials);
			}

			writer.WriteAttributeString(ObjectIdAttributeName, serializedObject.ID);

			IQuickStyle quickStyle = serializedObject.QuickStyle;
			if (quickStyle != null)
			{
				writer.WriteAttributeString(QuickStyleIndexAttributeName, Converter.ToString(quickStyle.Index));
			}
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			TextFormatter.Instance.Serialize(obj, writer);
			OEChildrenFormatter.Instance.Serialize(obj, writer);
		}

		private static string AlignmentAttributeName = "alignment";
		private static string AuthorAttributeName = "author";
		private static string AuthorInitialsAttributeName = "authorInitials";
		private static string ObjectIdAttributeName = "objectID";
		private static string CreationTimeAttributeName = "creationTime";
		private static string LastModifiedTimeAttributeName = "lastModifiedTime";
		private static string LastModifiedByAttributeName = "lastModifiedBy";
		private static string LastModifiedByInitialsAttributeName = "lastModifiedByInitials";
		private static string QuickStyleIndexAttributeName = "quickStyleIndex";

		private OutlineElement deserializedObject;
	}
}
