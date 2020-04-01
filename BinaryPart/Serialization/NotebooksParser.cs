using Sidenote.DOM;
using System.Diagnostics;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class NotebooksParser : ParserBase<NotebooksParser>
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
	}
}
