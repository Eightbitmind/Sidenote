using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.OneNote;

namespace Sidenote.DOM
{
	#region Implementations

	internal abstract class Node : INode
	{
		#region INode members

		public string Name { get; }
		public string ID { get; }
		public DateTime LastModifiedTime { get; }

		public INode Parent { get; }
		public abstract IList<INode> Children { get; }

		#endregion

		protected Node (Application app, INode parent, string name, string id, DateTime lastModifiedTime)
		{
			this.App = app;
			this.Parent = parent;
			this.Name = name;
			this.ID = id;
			this.LastModifiedTime = lastModifiedTime;
		}

		protected Application App { get; }

		protected IList<INode> children;
	}

	#endregion 
}
