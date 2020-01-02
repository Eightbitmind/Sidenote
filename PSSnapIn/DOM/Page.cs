using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sidenote.DOM
{
	internal class Page : Node, IIdentifiableObject, INamedObject, IUserCreatedObject, IPage
	{
		#region INode members

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
					IFormatter formatter = FormatterManager.PageContentFormatter;
					bool success = formatter.Deserialize(this.App, this);
					Debug.Assert(success);
				}

				return this.children;
			}
		}

		#endregion

		#region IIdentifiableObject members

		public string ID { get; }

		#endregion

		#region INamedObject members

		public string Name { get; }

		#endregion

		#region IUserCreatedObject members

		public string Author { get; }
		public string AuthorInitials { get; }
		public DateTime CreationTime { get; }
		public DateTime LastModifiedTime { get; }

		#endregion

		#region IPage members

		public DateTime DateTime { get; }
		public uint PageLevel { get; }

		#endregion

		internal Page(Application app, INode parent, string name, string id, DateTime lastModifiedTime, DateTime dateTime, uint pageLevel)
			: base(app, parent)
		{
			this.ID = id;
			this.Name = name;
			this.LastModifiedTime = lastModifiedTime;
			this.DateTime = dateTime;
			this.PageLevel = pageLevel;
		}
	}
}
