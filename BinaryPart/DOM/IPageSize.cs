namespace Sidenote.DOM
{
	public interface IPageSize
	{
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// This property represents the property- and child-less "Automatic" subelement. Judging
		/// from the schema, the "Automatic" subelement is mutually exclusice with the
		/// "Orientation", "Dimensions" and "Margins" subelements. Therefore, the children of an
		/// IPageSize object should only be used if the IsAutomatic property is false.
		/// </remarks>
		bool IsAutomatic { get; }
	}
}
