﻿using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookContentParser : ParserBase<NotebookContentParser>
	{
		public NotebookContentParser() : base("Notebook") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					SectionEntryParser.Instance.Parse(reader, parent) ||
					SectionGroupParser.Instance.Parse(reader,parent)
				))
				{
					throw new Exception("unexpected Notebook child " + reader.LocalName);
				}
			}

			return true;
		}
	}
}
