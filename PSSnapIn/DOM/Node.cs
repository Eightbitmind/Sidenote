using Microsoft.Office.Interop.OneNote;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Node : INode
	{
		#region INode members

		public INode Parent { get; }
		public virtual IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
				}

				return this.children;
			}
		}

		#endregion

		internal Node (Application app, INode parent)
		{
			this.App = app;
			this.Parent = parent;
		}

		protected Application App { get; }
		protected IList<INode> children;
	}
}
