using System;
using System.Collections.Generic;

namespace Sidenote.DOM
{
	internal class Table : Node, IIdentifiableObject, IUserCreatedObject, ITable
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

		#region ITable members

		public bool BordersAreVisible { get; }
		public bool HasHeaderRow { get; }

		public int RowCount { get; internal set; }
		public int ColumnCount { get; internal set; }

		public INode GetCell(int row, int column)
		{
			return this.rows[row][column];
		}

		#endregion

		internal Table(
			uint depth,
			INode parent,
			string id,
			string author,
			string authorInitials,
			DateTime creationTime,
			DateTime lastModifiedTime,
			bool bordersAreVisible,
			bool hasHeaderRow)
			: base(type: "Table", depth: depth, parent: parent)
		{
			this.ID = id;
			this.Author = author;
			this.AuthorInitials = authorInitials;
			this.CreationTime = creationTime;
			this.LastModifiedTime = lastModifiedTime;
			this.BordersAreVisible = bordersAreVisible;
			this.HasHeaderRow = hasHeaderRow;
		}

		internal void AddRow()
		{
			this.rows.Add(new List<TableCell>());
		}

		internal void AddCell(TableCell cell)
		{
			this.rows[this.rows.Count - 1].Add(cell);
		}

		private List<List</*TableCell*/ TableCell>> rows = new List<List<TableCell>>();
	}
}
