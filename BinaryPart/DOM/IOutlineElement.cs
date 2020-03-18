namespace Sidenote.DOM
{
	public interface IOutlineElement
	{
		/// <summary>
		/// Gets an optional list item annotation for the outline element.
		/// </summary>
		IListItem ListItem { get; }

		// TODO: 'enum' instead of 'string'
		string Alignment { get; }
		int QuickStyleIndex { get; set; }
		string Text { get; }
	}
}
