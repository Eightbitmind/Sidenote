using System;

namespace Sidenote.DOM
{
	public interface IPage
	{
		uint PageLevel { get; }
		string Language { get; }
		void Save();
	}
}
