using Sidenote.DOM;
using System.Globalization;
using System.Xml;
using System;

namespace Sidenote.Serialization
{
	internal class QuickStyleFormatter : FormatterBase<QuickStyle, QuickStyleFormatter>
	{
		public QuickStyleFormatter() : base("QuickStyleDef") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			// required attributes

			uint index = uint.Parse(reader.GetAttribute(IndexAttributeName));
			string name = reader.GetAttribute(NameAttributeName);
			string fontName = reader.GetAttribute(FontNameAttributeName);
			double fontSize = double.Parse(reader.GetAttribute(FontSizeAttributeName));

			var quickStyle = new QuickStyle(
				index,
				name,
				fontName,
				fontSize,
				parent.Depth + 1,
				parent);

			// optional attributes

			string fontColor = reader.GetAttribute(FontColorAttributeName);
			if (!string.IsNullOrEmpty(fontColor))
			{
				quickStyle.FontColor = fontColor;
			}

			string highlightColor = reader.GetAttribute(HighlightColorAttributeName);
			if (!string.IsNullOrEmpty(highlightColor))
			{
				quickStyle.HighlightColor = highlightColor;
			}

			string fontSizeString = reader.GetAttribute(FontSizeAttributeName);
			if (!string.IsNullOrEmpty(fontSizeString))
			{
				quickStyle.FontSize = double.Parse(fontSizeString);
			}

			string boldString = reader.GetAttribute(BoldAttributeName);
			if (!string.IsNullOrEmpty(boldString))
			{
				quickStyle.Bold = bool.Parse(boldString);
			}

			string italicString = reader.GetAttribute(ItalicAttributeName);
			if (!string.IsNullOrEmpty(italicString))
			{
				quickStyle.Italic = bool.Parse(italicString);
			}

			string underlineString = reader.GetAttribute(UnderlineAttributeName);
			if (!string.IsNullOrEmpty(underlineString))
			{
				quickStyle.Underline = bool.Parse(underlineString);
			}

			string strikethroughString = reader.GetAttribute(StrikethroughAttributeName);
			if (!string.IsNullOrEmpty(strikethroughString))
			{
				quickStyle.Strikethrough = bool.Parse(strikethroughString);
			}

			string superscriptString = reader.GetAttribute(SuperscriptAttributeName);
			if (!string.IsNullOrEmpty(superscriptString))
			{
				quickStyle.Superscript = bool.Parse(superscriptString);
			}

			string subscriptString = reader.GetAttribute(SubscriptAttributeName);
			if (!string.IsNullOrEmpty(subscriptString))
			{
				quickStyle.Subscript = bool.Parse(subscriptString);
			}

			string spaceBeforeString = reader.GetAttribute(SpaceBeforeAttributeName);
			if (!string.IsNullOrEmpty(spaceBeforeString))
			{
				quickStyle.SpaceBefore = float.Parse(spaceBeforeString);
			}

			string spaceAfterString = reader.GetAttribute(SpaceAfterAttributeName);
			if (!string.IsNullOrEmpty(spaceAfterString))
			{
				quickStyle.SpaceAfter = float.Parse(spaceAfterString);
			}

			parent.Children.Add(quickStyle);

			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			var quickStyle = (QuickStyle)node;

			// required attributes

			writer.WriteAttributeString(IndexAttributeName, Convert.ToString(quickStyle.Index, CultureInfo.InvariantCulture));
			writer.WriteAttributeString(NameAttributeName, quickStyle.Name);
			writer.WriteAttributeString(FontNameAttributeName, quickStyle.FontName);
			writer.WriteAttributeString(FontSizeAttributeName, quickStyle.FontSize.ToString("N1"));

			// optional attributes

			if (string.CompareOrdinal(quickStyle.FontColor, QuickStyle.FontColorDefaultValue) != 0)
			{
				writer.WriteAttributeString(FontColorAttributeName, quickStyle.FontColor);
			}

			if (string.CompareOrdinal(quickStyle.HighlightColor, QuickStyle.HighlightColorDefaultValue) != 0)
			{
				writer.WriteAttributeString(HighlightColorAttributeName, quickStyle.HighlightColor);
			}

			if (quickStyle.Bold != QuickStyle.BoldDefaultValue)
			{
				writer.WriteAttributeString(BoldAttributeName, Convert.ToString(quickStyle.Bold, CultureInfo.InvariantCulture));
			}

			if (quickStyle.Italic != QuickStyle.ItalicDefaultValue)
			{
				writer.WriteAttributeString(ItalicAttributeName, Convert.ToString(quickStyle.Italic, CultureInfo.InvariantCulture));
			}

			if (quickStyle.Underline != QuickStyle.UnderlineDefaultValue)
			{
				writer.WriteAttributeString(UnderlineAttributeName, Convert.ToString(quickStyle.Underline, CultureInfo.InvariantCulture));
			}

			if (quickStyle.Strikethrough != QuickStyle.StrikethroughDefaultValue)
			{
				writer.WriteAttributeString(StrikethroughAttributeName, Convert.ToString(quickStyle.Strikethrough, CultureInfo.InvariantCulture));
			}

			if (quickStyle.Superscript != QuickStyle.SuperscriptDefaultValue)
			{
				writer.WriteAttributeString(SuperscriptAttributeName, Convert.ToString(quickStyle.Superscript, CultureInfo.InvariantCulture));
			}

			if (quickStyle.Subscript != QuickStyle.SubscriptDefaultValue)
			{
				writer.WriteAttributeString(SubscriptAttributeName, Convert.ToString(quickStyle.Subscript, CultureInfo.InvariantCulture));
			}

			if (quickStyle.SpaceBefore != QuickStyle.SpaceBeforeDefaultValue)
			{
				writer.WriteAttributeString(SpaceBeforeAttributeName, quickStyle.SpaceBefore.ToString("N1"));
			}

			if (quickStyle.SpaceAfter != QuickStyle.SpaceAfterDefaultValue)
			{
				writer.WriteAttributeString(SpaceAfterAttributeName, quickStyle.SpaceAfter.ToString("N1"));
			}
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
