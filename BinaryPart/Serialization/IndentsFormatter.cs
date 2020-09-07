using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class IndentsFormatter : FormatterBase<NonexistentNode, IndentsFormatter>
	{
		public IndentsFormatter() : base("Indents") { }


		//internal override bool Deserialize(XmlReader reader, object parent, PatchStore patchStore)
		//{
		//	if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
		//	{
		//		return false;
		//	}

		//	// ignore Indents elements for now
		//	// reader.Skip();
		//	while (reader.IsStartElement())
		//	{
		//		if (!IndentFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore))
		//		{
		//			throw new Exception("unexpected Indents child " + reader.LocalName);
		//		}
		//	}

		//	return true;
		//}

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
				IndentFormatter.Instance.Serialize()
			}
		}
	}
}
