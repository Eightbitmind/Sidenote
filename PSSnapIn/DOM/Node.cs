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

		internal Node (INode parent)
		{
			this.Parent = parent;
		}

		protected IList<INode> children;
	}
}
