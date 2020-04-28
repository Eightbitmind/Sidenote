using Sidenote.DOM;
using System;
using System.Globalization;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageSettingsFormatter : FormatterBase<PageSettings, PageSettingsFormatter>
	{
		public PageSettingsFormatter() : base("PageSettings") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			var pageSettings = new PageSettings(parent.Depth + 1, parent);
			parent.Children.Add(pageSettings);

			string rtlString = reader.GetAttribute(RtlAttributeName);
			if (!string.IsNullOrEmpty(rtlString))
			{
				pageSettings.Rtl = bool.Parse(rtlString);
			}

			string color = reader.GetAttribute(ColorAttributeName);
			if (!string.IsNullOrEmpty(color))
			{
				pageSettings.Color = color;
			}

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while(reader.IsStartElement()) reader.Skip();

			return true;

		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			var pageSettings = (PageSettings)node;

			if (pageSettings.Rtl != PageSettings.RtlDefaultValue)
			{
				writer.WriteAttributeString(RtlAttributeName, Convert.ToString(pageSettings.Rtl, CultureInfo.InvariantCulture));
			}

			if (string.CompareOrdinal(pageSettings.Color, PageSettings.ColorDefaultValue) != 0)
			{
				writer.WriteAttributeString(ColorAttributeName, pageSettings.Color);
			}
		}

		private static string RtlAttributeName = "RTL";
		private static string ColorAttributeName = "color";
	}
}
