using Sidenote.DOM;
using System;
using System.Globalization;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class RuleLinesFormatter : FormatterBase<RuleLinesFormatter>
	{
		public RuleLinesFormatter() : base("RuleLines") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is RuleLines;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;

			// required attribute
			bool isVisible = bool.Parse(reader.GetAttribute(VisibleAttributeName));

			this.deserializedObject = new RuleLines(isVisible, parentNode.Depth + 1, parentNode);
			parentNode.Children.Add(deserializedObject);

			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			// ignore content for now
			while(reader.IsStartElement()) reader.Skip();
			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (RuleLines)obj;
			writer.WriteAttributeString(VisibleAttributeName, Converter.ToString(serializedObject.IsVisible));
		}

		private RuleLines deserializedObject;
		private static string VisibleAttributeName = "visible";
	}
}
