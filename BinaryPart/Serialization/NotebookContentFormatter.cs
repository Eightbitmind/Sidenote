using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebookContentFormatter : FormatterBase<NotebookContentFormatter>
	{
		public NotebookContentFormatter() : base("Notebook") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is Notebook;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			while (reader.IsStartElement())
			{
				if (!(
					SectionEntryFormatter.Instance.Deserialize(reader, parent, patchStore) ||
					SectionGroupFormatter.Instance.Deserialize(reader, parent, patchStore)
				))
				{
					throw new Exception("unexpected Notebook child " + reader.LocalName);
				}
			}

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
