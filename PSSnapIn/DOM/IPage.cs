using System;

namespace Sidenote.DOM
{
	public interface IPage : INode
	{
		DateTime DateTime { get; }
		uint PageLevel { get; }
	}
}
