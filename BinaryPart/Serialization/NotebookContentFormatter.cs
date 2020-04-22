using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookContentFormatter : FormatterBase<Notebook, NotebookContentFormatter>
	{
		public NotebookContentFormatter() : base("Notebook") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					SectionEntryFormatter.Instance.Deserialize(reader, parent) ||
					SectionGroupFormatter.Instance.Deserialize(reader,parent)
				))
				{
					throw new Exception("unexpected Notebook child " + reader.LocalName);
				}
			}

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
