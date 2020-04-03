using Sidenote.DOM;
using System.Diagnostics;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebooksParser : ParserBase<NonexistentNode, NotebooksParser>
	{
		public NotebooksParser() : base("Notebooks") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			bool parsedAtLeastOneNotebook = false;
			while (NotebookEntryParser.Instance.Parse(reader, parent))
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
