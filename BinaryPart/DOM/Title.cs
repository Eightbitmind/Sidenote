using Microsoft.Office.Interop.OneNote;

namespace Sidenote.DOM
{
	internal class Title : Node
	{
		internal Title(uint depth, INode parent)
			: base(type: "Title", depth: depth, parent: parent)
		{
		}
	}
}
