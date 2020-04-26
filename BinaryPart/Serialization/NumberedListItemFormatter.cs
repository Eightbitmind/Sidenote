﻿using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NumberedListItemFormatter : FormatterBase<NonexistentNode, NumberedListItemFormatter>
	{
		public NumberedListItemFormatter() : base("Number") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			// TODO: read more attributes
			string text = reader.GetAttribute("text");
			((OutlineElement)parent).ListItem = new NumberedListItem(text);
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}