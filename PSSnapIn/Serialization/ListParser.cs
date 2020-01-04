﻿using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class ListParser : ParserBase<ListParser>
	{
		public ListParser() : base("List") { }

		internal override bool Parse(XmlReader reader, INode parent)
		{
			if (!reader.IsStartElement() || string.CompareOrdinal(reader.LocalName, this.tagName) != 0)
			{
				return false;
			}

			// ignore List elements for now
			reader.Skip();

			return true;
		}
	}
}
