using Sidenote.DOM;
using System.Xml;
using System.Text.RegularExpressions;

namespace Sidenote.Serialization
{
	internal class TextParser : ParserBase<TextParser>
	{
		public TextParser() : base("T") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			((OutlineElement)parent).SetText(ReplaceCharacterEntityReferences(reader.ReadContentAsString()));
			return true;
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
