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
		IQuickStyle QuickStyle { get; set; }
		string Text { get; set; }

		// part of the "EditedByAttributes" attribute group in the schema. Should the go into the
		// IUserCreatedObject interface instead?
		string LastModifiedBy { get; set; }
		string LastModifiedByInitials { get; set; }
	}
}
