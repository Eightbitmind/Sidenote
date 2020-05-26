using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageSizeFormatter : FormatterBase<PageSize, PageSizeFormatter>
	{
		public PageSizeFormatter() : base("PageSize") { }

		protected override bool DeserializeAttributes(XmlReader reader, INode parent, PatchStore patchStore)
		{
			this.deserializedObject = new PageSize(parent.Depth + 1, parent);
			parent.Children.Add(this.deserializedObject);
			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, INode parent, PatchStore patchStore)
		{
			if (AutomaticFormatter.Instance.Deserialize(reader, this.deserializedObject, patchStore))
			{
				return true;
			}
			else if (/* TODO: Orientation, Dimensions, Margins*/ false)
			{
				return true;
			}

			return false;
		}

		protected override void SerializeChildren(INode node, XmlWriter writer)
		{
			var pageSize = (PageSize)node;
			if (!AutomaticFormatter.Instance.Serialize(pageSize, writer))
			{
				foreach (INode child in pageSize.Children)
				{
					if(!(/* TODO: Orientation, Dimensions, Margins */ false))
					{
						throw new System.Exception("not expected/implemented");
					}
				}
			}
		}

		private PageSize deserializedObject;
	}
}
