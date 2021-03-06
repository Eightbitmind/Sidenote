﻿using Sidenote.DOM;
using System.Xml;
using System.Text.RegularExpressions;

namespace Sidenote.Serialization
{
	internal class TextFormatter : FormatterBase<TextFormatter>
	{
		public TextFormatter() : base("T") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is OutlineElement;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			// Big TODO: As Omer pointed out, the content is actually HTML (or presumably a subset
			// thereof).

			((OutlineElement)parent).SetText(ReplaceCharacterEntityReferences(reader.ReadContentAsString()));
			return true;
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			// TODO: encode characters that need it as charecter entity references
			// big TODO: Write HTML
			writer.WriteCData(((OutlineElement)obj).Text);
		}

		private static Regex characterEntityReferencePattern = new Regex("&(?<name>\\w+);", RegexOptions.Compiled);

		private static string ReplaceCharacterEntityReferences(string s)
		{
			return characterEntityReferencePattern.Replace(s, (Match m) =>
			{
				switch(m.Groups["name"].Value)
				{
					case "amp": return "&";
					case "apos": return "'";
					case "gt": return ">";
					case "lt": return "<";
					case "quot": return "\"";
					default: return m.Value;
				}
			});

		}
	}
}
