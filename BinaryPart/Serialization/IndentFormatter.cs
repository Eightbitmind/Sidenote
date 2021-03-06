﻿using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class IndentFormatter : FormatterBase<IndentFormatter>
	{
		public IndentFormatter() : base("Indent") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Indent;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var deserializedObject = (IOutline)parent;

			Indent indent = new Indent();

			string indentStr = reader.GetAttribute(IndentAttributeName);
			if (!string.IsNullOrEmpty(indentStr))
			{
				indent.Indentation = double.Parse(indentStr);
			}

			string levelStr = reader.GetAttribute(LevelAttributeName);
			if (!string.IsNullOrEmpty(levelStr))
			{
				indent.Level = uint.Parse(levelStr);
			}

			deserializedObject.Indents.Add(indent);
			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (Indent)obj;
			writer.WriteAttributeString(IndentAttributeName, Converter.ToString(serializedObject.Indentation));
			writer.WriteAttributeString(LevelAttributeName, Converter.ToString(serializedObject.Level));
		}

		private static string IndentAttributeName = "indent";
		private static string LevelAttributeName = "level";
	}
}
