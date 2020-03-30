namespace Sidenote.DOM
{
	public interface ITable
	{
		bool BordersAreVisible { get; }
		bool HasHeaderRow { get; }

		int RowCount { get; }
		int ColumnCount { get; }

		INode GetCell(int row, int column);
	}
}
