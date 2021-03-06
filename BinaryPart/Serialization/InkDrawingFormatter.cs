﻿using System.Xml;

namespace Sidenote.Serialization
{
	internal class InkDrawingFormatter : FormatterBase<InkDrawingFormatter>
	{
		public InkDrawingFormatter() : base("InkDrawing") { }

		internal override bool Deserialize(XmlReader reader, object parent, PatchStore patchStore)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore InkDrawing elements for now
			reader.Skip();

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			return false;
		}
	}
}
