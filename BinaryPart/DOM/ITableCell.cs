namespace Sidenote.DOM
{
	public interface ITableCell
	{
		int RowIndex { get; }
		int ColumnIndex { get; }

		string ShadingColor { get; }
	}
}
