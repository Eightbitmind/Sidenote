namespace Sidenote.DOM
{
	internal class BulletListItem : IListItem
	{
		internal BulletListItem()
		{
		}

		#region IListItem members

		public virtual ListItemType Type
		{
			get { return ListItemType.BulletListItem; }
		}

		#endregion
	}
}
