using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TitleFormatter : FormatterBase<Title, TitleFormatter>
	{
		public TitleFormatter() : base("Title") { }

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			this.deserializedObject = new Title(parentNode.Depth + 1, parentNode);

			string language = reader.GetAttribute(LanguageAttributeName);
			if (!string.IsNullOrEmpty(language))
			{
				this.deserializedObject.Language = language;
			}

			parentNode.Children.Add(deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!OEFormatter.Instance.Deserialize(reader, deserializedObject, patchStore))
			{
				throw new Exception("Title element missing OE");
			}

			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (Title)obj;

			if (!string.IsNullOrEmpty(serializedObject.Language))
			{
				writer.WriteAttributeString(LanguageAttributeName, serializedObject.Language);
			}
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			foreach (INode child in ((INode)obj).Children)
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
