using Sidenote.DOM;
using System;
using System.Globalization;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageSettingsFormatter : FormatterBase<PageSettings, PageSettingsFormatter>
	{
		public PageSettingsFormatter() : base("PageSettings") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			this.deserializedObject = new PageSettings(parent.Depth + 1, parent);
			parent.Children.Add(this.deserializedObject);

			string rtlString = reader.GetAttribute(RtlAttributeName);
			if (!string.IsNullOrEmpty(rtlString))
			{
				this.deserializedObject.Rtl = bool.Parse(rtlString);
			}

			string color = reader.GetAttribute(ColorAttributeName);
			if (!string.IsNullOrEmpty(color))
			{
				this.deserializedObject.Color = color;
			}

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			while (
				PageSizeFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore) ||
				RuleLinesFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore)) ;

			return true;

		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			var serializedObject = (PageSettings)node;

			// Win32 client appears to serialize this even if it has the default value
			// if (serializedObject.Rtl != PageSettings.RtlDefaultValue)
			// {
				writer.WriteAttributeString(RtlAttributeName, Converter.ToString(serializedObject.Rtl));
			// }

			// Win32 client appears to serialize this even if it has the default value
			// if (string.CompareOrdinal(serializedObject.Color, PageSettings.ColorDefaultValue) != 0)
			// {
				writer.WriteAttributeString(ColorAttributeName, serializedObject.Color);
			// }
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			foreach (INode child in node.Children)
			{
				if (!(
					PageSizeFormatter.Instance.Serialize(child, writer) ||
					RuleLinesFormatter.Instance.Serialize(child, writer)))
				{
					throw new Exception("unexpected PageSettings child " + child.Type);

				}
			}
		}

		private static string RtlAttributeName = "RTL";
		private static string ColorAttributeName = "color";

		private PageSettings deserializedObject;
	}
}
