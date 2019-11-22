using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Notebook : Node, INotebook
	{
		#region INotebook members

		public string Nickname { get; }
		public string Path { get; }
		public string Color { get; }
		public bool IsCurrentlyViewed { get; }

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					IFormatter<IList<INode>> formatter = FormatterManager.SectionsFormatter;
					this.children = (IList<INode>)formatter.Deserialize(this.App, this);
				}

				return this.children;
			}
		}

		#endregion

		internal Notebook(Application app, INode parent, string name, string id, DateTime lastModifiedTime)
			: base(app, parent, name, id, lastModifiedTime)
		{

		}
	}
}
