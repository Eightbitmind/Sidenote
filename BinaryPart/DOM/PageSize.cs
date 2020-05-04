namespace Sidenote.DOM
{
	internal class PageSize : Node
	{
		internal PageSize(
			uint depth,
			INode parent)
			: base(type: "PageSize", depth: depth, parent: parent)
		{
		}
	}
}
