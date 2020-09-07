using Sidenote.DOM;
using System.Diagnostics;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebooksFormatter : FormatterBase<NonexistentNode, NotebooksFormatter>
	{
		public NotebooksFormatter() : base("Notebooks") { }

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
		{
			bool parsedAtLeastOneNotebook = false;
			while (NotebookEntryFormatter.Instance.Deserialize(reader, parent, patchStore))
			{
				parsedAtLeastOneNotebook = true;
			}
			Debug.Assert(parsedAtLeastOneNotebook);

			return true;
		}

		internal override bool Serialize(object obj, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
