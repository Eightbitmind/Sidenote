using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Node : INode
	{
		#region INode members

		public string Type { get; }
		public uint Depth { get; }
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

		internal Node (string type, uint depth, INode parent)
		{
			this.Type = type;
			this.Depth = depth;
			this.Parent = parent;
		}

		protected IList<INode> children;
	}
}
