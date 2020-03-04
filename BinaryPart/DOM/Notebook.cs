using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sidenote.DOM
{
	internal class Notebook : Node, IIdentifiableObject, INamedObject, IUserCreatedObject, INotebook
	{
		#region INode members

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
					IFormatter formatter = FormatterManager.NotebookContentFormatter;
					bool success = formatter.Deserialize(this);
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

		#region INotebook members

		public string Nickname { get; }
		public string Path { get; }
		public string Color { get; }
		public bool IsCurrentlyViewed { get; }

		#endregion

		internal Notebook(INode parent, string name, string id, DateTime lastModifiedTime)
			: base(type: "Notebook", depth: 1, parent: parent)
		{
			this.Name = name;
			this.ID = id;
			this.LastModifiedTime = lastModifiedTime;
		}
	}
}
