using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PageSizeFormatter : FormatterBase<PageSizeFormatter>
	{
		public PageSizeFormatter() : base("PageSize") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is PageSize;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var parentNode = (INode)parent;
			this.deserializedObject = new PageSize(parentNode.Depth + 1, parentNode);
			parentNode.Children.Add(this.deserializedObject);
			return true;
		}

		protected override bool DeserializeChildren(XmlReader reader, object parent, PatchStore patchStore)
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

		protected override void SerializeChildren(object obj, XmlWriter writer)
		{
			var pageSize = (PageSize)obj;
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
