using System.Collections.Generic;

namespace Sidenote.DOM
{
	public interface INode
	{
		INode Parent { get; }
		IList<INode> Children { get; }
	}
}
