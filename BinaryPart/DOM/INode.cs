using System.Collections.Generic;

namespace Sidenote.DOM
{
	public interface INode
	{
		string Type { get; }
		uint Depth { get; }
		INode Parent { get; }
		IList<INode> Children { get; }
	}
}
