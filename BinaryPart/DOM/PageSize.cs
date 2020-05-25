using System;

namespace Sidenote.DOM
{
	internal class PageSize : Node, IPageSize
	{
		#region IPageSize members

		public bool IsAutomatic {
			get
			{
				return this.isAutomatic;
			}
			set
			{
				if (this.Children.Count > 0 && value)
				{
					throw new InvalidOperationException();
				}
				this.isAutomatic = value;
			}
		}

		#endregion

		internal PageSize(
			uint depth,
			INode parent)
			: base(type: "PageSize", depth: depth, parent: parent)
		{
		}

		private bool isAutomatic;
	}
}
