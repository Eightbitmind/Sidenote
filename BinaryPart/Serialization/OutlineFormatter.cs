using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OutlineFormatter : FormatterBase<Outline, OutlineFormatter>
	{
		public OutlineFormatter() : base("Outline") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			string id = reader.GetAttribute(ObjectIDAttributeName);
			string author = reader.GetAttribute(AuthorAttributeName);
			string authorInitials = reader.GetAttribute(AuthorInitialsAttributeName);

			// DateTime creationTime = DateTime.Parse(reader.GetAttribute("creationTime"));
			DateTime lastModifiedTime = DateTime.Parse(reader.GetAttribute(LastModifiedTimeAttributeName));

			this.deserializedObject = new Outline(
				parentNode.Depth + 1,
				parentNode,
				id,
				author,
				authorInitials,
				lastModifiedTime, // What constitutes the creationTime?
				lastModifiedTime);

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

			parentNode.Children.Add(this.deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
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

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			Outline serializedObject = (Outline)obj;
			writer.WriteAttributeString(AuthorAttributeName, serializedObject.Author);
			writer.WriteAttributeString(AuthorInitialsAttributeName, serializedObject.AuthorInitials);
			writer.WriteAttributeString(LastModifiedTimeAttributeName, Converter.ToString(serializedObject.LastModifiedTime));
			writer.WriteAttributeString(ObjectIDAttributeName, serializedObject.ID);

			if (!string.IsNullOrEmpty(serializedObject.LastModifiedBy))
			{
				writer.WriteAttributeString(LastModifiedByAttributeName, serializedObject.LastModifiedBy);
			}

			if (!string.IsNullOrEmpty(serializedObject.LastModifiedByInitials))
			{
				writer.WriteAttributeString(LastModifiedByInitialsAttributeName, serializedObject.LastModifiedByInitials);
			}
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			var serializedObject = (Outline)obj;

			Position position = serializedObject.Position;
			if (position.X != default || position.Y != default || position.Z != default)
			{
				PositionFormatter.Instance.Serialize(obj, writer);
			}

			Size size = serializedObject.Size;
			if (size.Width != default || size.Height != default)
			{
				SizeFormatter.Instance.Serialize(obj, writer);
			}

			IndentsFormatter.Instance.Serialize(obj, writer);
			OEChildrenFormatter.Instance.Serialize(obj, writer);
		}

		private Outline deserializedObject;

		private static string AuthorAttributeName = "author";
		private static string AuthorInitialsAttributeName = "authorInitials";
		private static string LastModifiedTimeAttributeName = "lastModifiedTime";
		private static string LastModifiedByAttributeName = "lastModifiedBy";
		private static string LastModifiedByInitialsAttributeName = "lastModifiedByInitials";
		private static string ObjectIDAttributeName = "objectID";
	}
}
