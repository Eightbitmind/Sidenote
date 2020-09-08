using Sidenote.DOM;
using System.Globalization;
using System.Xml;
using System;

namespace Sidenote.Serialization
{
	internal class QuickStyleFormatter : FormatterBase<QuickStyleFormatter>
	{
		public QuickStyleFormatter() : base("QuickStyleDef") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is QuickStyle;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			// required attributes

			uint index = uint.Parse(reader.GetAttribute(IndexAttributeName));
			string name = reader.GetAttribute(NameAttributeName);
			string fontName = reader.GetAttribute(FontNameAttributeName);
			double fontSize = double.Parse(reader.GetAttribute(FontSizeAttributeName));

			var deserializedObject = new QuickStyle(
				index,
				name,
				fontName,
				fontSize,
				parentNode.Depth + 1,
				parentNode);

			// optional attributes

			string fontColor = reader.GetAttribute(FontColorAttributeName);
			if (!string.IsNullOrEmpty(fontColor))
			{
				deserializedObject.FontColor = fontColor;
			}

			string highlightColor = reader.GetAttribute(HighlightColorAttributeName);
			if (!string.IsNullOrEmpty(highlightColor))
			{
				deserializedObject.HighlightColor = highlightColor;
			}

			string fontSizeString = reader.GetAttribute(FontSizeAttributeName);
			if (!string.IsNullOrEmpty(fontSizeString))
			{
				deserializedObject.FontSize = double.Parse(fontSizeString);
			}

			string boldString = reader.GetAttribute(BoldAttributeName);
			if (!string.IsNullOrEmpty(boldString))
			{
				deserializedObject.Bold = bool.Parse(boldString);
			}

			string italicString = reader.GetAttribute(ItalicAttributeName);
			if (!string.IsNullOrEmpty(italicString))
			{
				deserializedObject.Italic = bool.Parse(italicString);
			}

			string underlineString = reader.GetAttribute(UnderlineAttributeName);
			if (!string.IsNullOrEmpty(underlineString))
			{
				deserializedObject.Underline = bool.Parse(underlineString);
			}

			string strikethroughString = reader.GetAttribute(StrikethroughAttributeName);
			if (!string.IsNullOrEmpty(strikethroughString))
			{
				deserializedObject.Strikethrough = bool.Parse(strikethroughString);
			}

			string superscriptString = reader.GetAttribute(SuperscriptAttributeName);
			if (!string.IsNullOrEmpty(superscriptString))
			{
				deserializedObject.Superscript = bool.Parse(superscriptString);
			}

			string subscriptString = reader.GetAttribute(SubscriptAttributeName);
			if (!string.IsNullOrEmpty(subscriptString))
			{
				deserializedObject.Subscript = bool.Parse(subscriptString);
			}

			string spaceBeforeString = reader.GetAttribute(SpaceBeforeAttributeName);
			if (!string.IsNullOrEmpty(spaceBeforeString))
			{
				deserializedObject.SpaceBefore = float.Parse(spaceBeforeString);
			}

			string spaceAfterString = reader.GetAttribute(SpaceAfterAttributeName);
			if (!string.IsNullOrEmpty(spaceAfterString))
			{
				deserializedObject.SpaceAfter = float.Parse(spaceAfterString);
			}

			parentNode.Children.Add(deserializedObject);

			patchStore.AddPatchOperation(node =>
			{
				var outlineElement = node as OutlineElement;
				if (outlineElement != null && outlineElement.QuickStyleIndex != -1 && outlineElement.QuickStyleIndex == index)
				{
					outlineElement.QuickStyle = deserializedObject;
					outlineElement.QuickStyleIndex = -1;
				}
			});

			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (QuickStyle)obj;

			// required attributes

			writer.WriteAttributeString(IndexAttributeName, Convert.ToString(serializedObject.Index, CultureInfo.InvariantCulture));
			writer.WriteAttributeString(NameAttributeName, serializedObject.Name);
			writer.WriteAttributeString(FontNameAttributeName, serializedObject.FontName);
			writer.WriteAttributeString(FontSizeAttributeName, serializedObject.FontSize.ToString("N1"));

			// optional attributes

			// Win32 client appears to serialize this even if it has the default value
			// if (string.CompareOrdinal(serializedObject.FontColor, QuickStyle.FontColorDefaultValue) != 0)
			// {
				writer.WriteAttributeString(FontColorAttributeName, serializedObject.FontColor);
			// }

			// Win32 client appears to serialize this even if it has the default value
			// if (string.CompareOrdinal(serializedObject.HighlightColor, QuickStyle.HighlightColorDefaultValue) != 0)
			// {
				writer.WriteAttributeString(HighlightColorAttributeName, serializedObject.HighlightColor);
			// }

			if (serializedObject.Bold != QuickStyle.BoldDefaultValue)
			{
				writer.WriteAttributeString(BoldAttributeName, Converter.ToString(serializedObject.Bold));
			}

			if (serializedObject.Italic != QuickStyle.ItalicDefaultValue)
			{
				writer.WriteAttributeString(ItalicAttributeName, Converter.ToString(serializedObject.Italic));
			}

			if (serializedObject.Underline != QuickStyle.UnderlineDefaultValue)
			{
				writer.WriteAttributeString(UnderlineAttributeName, Converter.ToString(serializedObject.Underline));
			}

			if (serializedObject.Strikethrough != QuickStyle.StrikethroughDefaultValue)
			{
				writer.WriteAttributeString(StrikethroughAttributeName, Converter.ToString(serializedObject.Strikethrough));
			}

			if (serializedObject.Superscript != QuickStyle.SuperscriptDefaultValue)
			{
				writer.WriteAttributeString(SuperscriptAttributeName, Converter.ToString(serializedObject.Superscript));
			}

			if (serializedObject.Subscript != QuickStyle.SubscriptDefaultValue)
			{
				writer.WriteAttributeString(SubscriptAttributeName, Converter.ToString(serializedObject.Subscript));
			}

			// Win32 client appears to serialize even if it has the default value
			// if (serializedObject.SpaceBefore != QuickStyle.SpaceBeforeDefaultValue)
			// {
				writer.WriteAttributeString(SpaceBeforeAttributeName, Converter.ToString(serializedObject.SpaceBefore));
			// }

			// Win32 client appears to serialize even if it has the default value
			// if (serializedObject.SpaceAfter != QuickStyle.SpaceAfterDefaultValue)
			// {
				writer.WriteAttributeString(SpaceAfterAttributeName, Converter.ToString(serializedObject.SpaceAfter));
			// }
		}

		private static string IndexAttributeName = "index";
		private static string NameAttributeName = "name";
		private static string FontColorAttributeName = "fontColor";
		private static string HighlightColorAttributeName = "highlightColor";
		private static string FontNameAttributeName = "font";
		private static string FontSizeAttributeName = "fontSize";
		private static string BoldAttributeName = "bold";
		private static string ItalicAttributeName = "italic";
		private static string UnderlineAttributeName = "underline";
		private static string StrikethroughAttributeName = "strikethrough";
		private static string SuperscriptAttributeName = "superscript";
		private static string SubscriptAttributeName = "subscript";
		private static string SpaceBeforeAttributeName = "spaceBefore";
		private static string SpaceAfterAttributeName = "spaceAfter";
	}
}
