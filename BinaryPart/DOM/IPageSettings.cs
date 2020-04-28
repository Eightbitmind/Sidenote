namespace Sidenote.DOM
{
	public interface IPageSettings
	{
		// PageSize and RuleLines expressed via INode.Children
		bool Rtl { get; }
		string Color { get; }
	}
}
