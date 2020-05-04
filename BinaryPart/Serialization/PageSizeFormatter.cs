using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageSizeFormatter : FormatterBase<PageSize, PageSizeFormatter>
	{
		public PageSizeFormatter() : base("PageSize") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent)
		{
			var deserializedObject = new PageSize(parent.Depth + 1, parent);
			parent.Children.Add(deserializedObject);
			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent)
		{
			while(reader.IsStartElement()) reader.Skip();
			return true;
		}
	}
}
