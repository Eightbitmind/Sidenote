using Sidenote.DOM;
using System;
using System.Collections.Generic;
namespace Sidenote.Serialization
{
	internal class PatchStore
	{
		internal void AddPatchOperation(Action<INode> patchOperation)
		{
			if (this.patchOperations == null)
			{
				this.patchOperations = new List<Action<INode>>();
			}

			this.patchOperations.Add(patchOperation);
		}

		internal void PerformPatchOperations(INode root)
		{
			if (this.patchOperations == null || this.patchOperations.Count == 0)
			{
				return;
			}

			var queue = new Queue<INode>();
			queue.Enqueue(root);
			while (queue.Count > 0)
			{
				INode current = queue.Dequeue();
				foreach (Action<INode> patchOperation in this.patchOperations)
				{
					patchOperation(current);
				}
				foreach (INode child in current.Children)
				{
					queue.Enqueue(child);
				}
			}
		}

		private List<Action<INode>> patchOperations;
	}
}
