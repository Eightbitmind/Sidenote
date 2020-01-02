using System;

namespace Sidenote.DOM
{
	public interface IPage
	{
		DateTime DateTime { get; }
		uint PageLevel { get; }
	}
}
