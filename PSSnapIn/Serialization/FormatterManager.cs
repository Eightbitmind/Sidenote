using Sidenote.DOM;
using System.Collections.Generic;

namespace Sidenote.Serialization
{
	static internal class FormatterManager
	{
		public const string Xml2013 = "http://schemas.microsoft.com/office/onenote/2013/onenote";

		public static IFormatter<IRoot> NotebooksFormatter
		{
			get { return new NotebooksFormatter(); }
		}

		public static IFormatter<IList<INode>> SectionsFormatter
		{
			get { return new SectionsFormatter(); }
		}

		public static IFormatter<IList<INode>> PagesFormatter
		{
			get { return new PagesFormatter(); }
		}

		public static IFormatter<IList<INode>> PageContentFormatter
		{
			get { return new PageContentFormatter(); }
		}
	}
}
