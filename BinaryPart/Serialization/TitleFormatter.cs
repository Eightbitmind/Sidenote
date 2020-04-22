using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class TitleFormatter : FormatterBase<Title, TitleFormatter>
	{
		public TitleFormatter() : base("Title") { }

		// protected override bool DeserializeAttributes(XmlReader reader, Application app, INode parent)
		// {
		//	// Do something with 'lang' attribute?
		// }

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			Title title = new Title(parent.Depth + 1, parent);

			if (!OEFormatter.Instance.Deserialize(reader, title))
			{
				throw new Exception("Title element missing OE");
			}

			parent.Children.Add(title);
			return true;
		}

		internal override bool Serialize(INode node, XmlWriter writer)
		{
			// throw new System.Exception("not expected/implemented");
			// return false;
			// return true;

			return (node is Title);
		}
	}
}
