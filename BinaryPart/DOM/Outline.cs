using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Outline : Node, IIdentifiableObject, IUserCreatedObject, IPositionedObject, IOutline
	{
		#region IIdentifiableObject members

		public string ID { get; }

		#endregion

		#region IUserCreatedObject members

		public string Author { get; }

		public string AuthorInitials { get; }

		public DateTime CreationTime { get; }

		public DateTime LastModifiedTime { get; }

		public string LastModifiedBy { get; set; }

		public string LastModifiedByInitials { get; set; }

		#endregion

		#region IPositionedObject members

		public Position Position { get; set; }
		public Size Size { get; set; }

		#endregion

		#region IOutline

		public IList<Indent> Indents
		{
			get { return this.indents; }
		}

		#endregion

		internal Outline(
			uint depth,
			INode parent,
			string id,
			string author,
			string authorInitials,
			DateTime creationTime,
			DateTime lastModifiedTime)
			: base(type: "Outline", depth: depth, parent: parent)
		{
			this.ID = id;

			this.Author = author;
			this.AuthorInitials = authorInitials;
			this.CreationTime = creationTime;
			this.LastModifiedTime = lastModifiedTime;
		}

		private List<Indent> indents = new List<Indent>();
	}
}
