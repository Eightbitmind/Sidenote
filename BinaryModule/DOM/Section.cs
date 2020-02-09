using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sidenote.DOM
{
	internal class Section : Node, IIdentifiableObject, INamedObject, IUserCreatedObject, ISection
	{
		#region INode members

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
					IFormatter formatter = FormatterManager.SectionContentFormatter;
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

		#region ISection members

		public string Path { get; }
		public string Color { get; }

		#endregion

		internal Section(INode parent, string name, string id, DateTime lastModifiedTime, string path, string Color)
			: base(parent)
		{
			this.ID = id;
			this.Name = name;
			this.LastModifiedTime = lastModifiedTime;
			this.Path = path;
			this.Color = Color;
		}
	}
}
