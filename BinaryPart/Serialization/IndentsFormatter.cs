using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class IndentsFormatter : FormatterBase<Outline, IndentsFormatter>
	{
		public IndentsFormatter() : base("Indents") { }

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!IndentFormatter.Instance.Deserialize(reader, parent, patchStore))
			{
				throw new Exception("expecting at least one Indent element");
			}
			while (IndentFormatter.Instance.Deserialize(reader, parent, patchStore)) ;
			return true;
		}

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			var outline = (IOutline)obj;

			foreach (Indent indent in outline.Indents)
			{
				IndentFormatter.Instance.Serialize(indent, writer);
			}
		}
	}
}
