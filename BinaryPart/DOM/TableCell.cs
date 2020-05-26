using System;

namespace Sidenote.DOM
{
	internal class TableCell : Node, IIdentifiableObject, IUserCreatedObject, ITableCell
	{
		#region IIdentifiableObject members

		public string ID { get; }

		#endregion

		#region IUserCreatedObject members

		public string Author { get; }

		public string AuthorInitials { get; }

		public DateTime CreationTime { get; }

		public DateTime LastModifiedTime { get; }

		public string LastModifiedBy
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string LastModifiedByInitials
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region ITableCell members

		public int RowIndex { get; }
		public int ColumnIndex { get; }

		public string ShadingColor { get; }

		#endregion

		internal TableCell(
			uint depth,
			INode parent,
			string id,
			string author,
			string authorInitials,
			DateTime creationTime,
			DateTime lastModifiedTime,
			string shadingColor)
			: base(type: "TableCell", depth: depth, parent: parent)
		{
			this.ID = id;

			this.Author = author;
			this.AuthorInitials = authorInitials;
			this.CreationTime = creationTime;
			this.LastModifiedTime = lastModifiedTime;
			this.ShadingColor = shadingColor;
		}

	}
}
