using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Root : IRoot
	{
		#region IRoot members

		public IList<INotebook> Notebooks { get { return this.notebooks; } }

		#endregion

		internal Root(IList<INotebook> notebooks)
		{
			this.notebooks = notebooks;
		}

		private IList<INotebook> notebooks;
	}
}
