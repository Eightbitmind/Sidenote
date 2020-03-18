namespace Sidenote.DOM
{
	internal class NumberedListItem : INumberedListItem
	{
		internal NumberedListItem(string text)
		{
			this.Text = text;
		}

		#region INumberedListItem members

		public virtual string Text { get; }

		#endregion

		#region IListItem members

		public virtual ListItemType Type
		{
			get { return ListItemType.NumberedListItem; }
		}

		#endregion
	}
}
