using Microsoft.Office.Interop.OneNote;
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
				throw new NotImplementedException();
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
