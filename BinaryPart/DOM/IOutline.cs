using System.Collections.Generic;

namespace Sidenote.DOM
{
	public interface IOutline
	{
		IList<Indent> Indents { get; }
	}
}
