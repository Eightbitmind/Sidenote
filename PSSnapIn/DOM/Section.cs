using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Section : Node, ISection
	{
		#region ISection members

		public string Path { get; }
		public string Color { get; }

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					IFormatter<IList<INode>> formatter = FormatterManager.PagesFormatter;
					this.children = (IList<INode>)formatter.Deserialize(this.App, this);
				}

				return this.children;
			}
		}

		#endregion

		internal Section(Application app, INode parent, string name, string id, DateTime lastModifiedTime, string path, string Color)
			: base(app, parent, name, id, lastModifiedTime)
		{
			this.Path = path;
			this.Color = Color;
		}
	}
}
