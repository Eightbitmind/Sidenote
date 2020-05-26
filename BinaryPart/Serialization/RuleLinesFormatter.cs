using Sidenote.DOM;
using System;
using System.Globalization;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class RuleLinesFormatter : FormatterBase<RuleLines, RuleLinesFormatter>
	{
		public RuleLinesFormatter() : base("RuleLines") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			// required attribute
			bool isVisible = bool.Parse(reader.GetAttribute(VisibleAttributeName));

			this.deserializedObject = new RuleLines(isVisible, parent.Depth + 1, parent);
			parent.Children.Add(deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			// ignore content for now
			while(reader.IsStartElement()) reader.Skip();
			return true;
		}

		protected override void SerializeAttributes(INode node, XmlWriter writer)
		{
			var serializedObject = (RuleLines)node;
			writer.WriteAttributeString(VisibleAttributeName, Converter.ToString(serializedObject.IsVisible));
		}

		private RuleLines deserializedObject;
		private static string VisibleAttributeName = "visible";
	}
}
