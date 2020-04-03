using System;

namespace Sidenote.DOM
{
	public interface IPage
	{
		uint PageLevel { get; }
		void Save();
	}
}
