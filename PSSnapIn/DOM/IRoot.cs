using System.Collections.Generic;

namespace Sidenote.DOM
{
	public interface IRoot
	{
		IList<INotebook> Notebooks { get; }
	}
}
