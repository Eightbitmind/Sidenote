using Sidenote.DOM;
using System.Diagnostics;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebooksFormatter : FormatterBase<NonexistentNode, NotebooksFormatter>
	{
		public NotebooksFormatter() : base("Notebooks") { }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			bool parsedAtLeastOneNotebook = false;
			while (NotebookEntryFormatter.Instance.Deserialize(reader, parent))
			{
				parsedAtLeastOneNotebook = true;
			}
			Debug.Assert(parsedAtLeastOneNotebook);

			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			throw new System.Exception("not expected/implemented");
		}
	}
}
