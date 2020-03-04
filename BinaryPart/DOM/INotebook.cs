namespace Sidenote.DOM
{
	public interface INotebook
	{
		string Nickname { get; }
		string Path { get; }
		string Color { get; }
		bool IsCurrentlyViewed { get; }
	}
}
