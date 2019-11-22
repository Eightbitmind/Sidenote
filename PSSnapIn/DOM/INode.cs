using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	public interface INode
	{
		string Name { get; }
		string ID { get; }
		DateTime LastModifiedTime { get; }

		INode Parent { get; }
		IList<INode> Children { get; }
	}
}
