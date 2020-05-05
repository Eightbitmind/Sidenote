using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TitleFormatter : FormatterBase<Title, TitleFormatter>
	{
		public TitleFormatter() : base("Title") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			this.deserializedObject = new Title(parent.Depth + 1, parent);

			string language = reader.GetAttribute(LanguageAttributeName);
			if (!string.IsNullOrEmpty(language))
			{
				this.deserializedObject.Language = language;
			}

			parent.Children.Add(deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			if (!OEFormatter.Instance.Deserialize(reader, deserializedObject))
			{
				throw new Exception("Title element missing OE");
			}

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			var serializedObject = (Title)node;

			if (!string.IsNullOrEmpty(serializedObject.Language))
			{
				writer.WriteAttributeString(LanguageAttributeName, serializedObject.Language);
			}
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach (INode child in node.Children)
			{
				if (!OEFormatter.Instance.Serialize(child, writer))
				{
					throw new Exception("unexpected Title child " + child.Type);
				}
			}
		}

		private Title deserializedObject;

		private static string LanguageAttributeName = "lang";
	}
}
