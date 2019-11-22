namespace Sidenote.DOM
{
	public interface INotebook : INode
	{
		string Nickname { get; }
		string Path { get; }
		string Color { get; }
		bool IsCurrentlyViewed { get; }
	}
}
