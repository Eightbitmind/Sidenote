using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Page : Node, IPage
	{
		#region IPage members

		public DateTime DateTime { get; }
		public uint PageLevel { get; }

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					IFormatter<IList<INode>> formatter = FormatterManager.PageContentFormatter;
					this.children = (IList<INode>)formatter.Deserialize(this.App, this);
				}

				return this.children;
			}
		}


		#endregion

		internal Page(Application app, INode parent, string name, string id, DateTime lastModifiedTime, DateTime dateTime, uint pageLevel)
			: base(app, parent, name, id, lastModifiedTime)
		{
			this.DateTime = dateTime;
			this.PageLevel = pageLevel;
		}
	}
}
