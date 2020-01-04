using Microsoft.Office.Interop.OneNote;
using System;

namespace Sidenote.DOM
{
	internal class OutlineElement : Node, IIdentifiableObject, IUserCreatedObject, IOutlineElement
	{
		#region IIdentifiableObject members

		public string ID { get; }

		#endregion

		#region IUserCreatedObject members

		public string Author { get; }
		public string AuthorInitials { get; }
		public DateTime CreationTime { get; }
		public DateTime LastModifiedTime { get; }

		#endregion

		#region IOutlineElement members

		public string Alignment { get; }
		public int QuickStyleIndex { get; set; }
		public string Text { get; private set; }

		#endregion

		internal OutlineElement(
			INode parent,
			string id,
			string author,
			string authorInitials,
			DateTime creationTime,
			DateTime lastModifiedTime,
			string alignment)
			: base(parent)
		{
			this.ID = id;

			this.Author = author;
			this.AuthorInitials = authorInitials;
			this.CreationTime = creationTime;
			this.LastModifiedTime = lastModifiedTime;

			this.Alignment = alignment;
			this.QuickStyleIndex = -1;
		}

		internal void SetText(string text)
		{
			this.Text = text;
		}
	}
}
