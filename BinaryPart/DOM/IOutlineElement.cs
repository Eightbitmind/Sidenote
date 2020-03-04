namespace Sidenote.DOM
{
	public interface IOutlineElement
	{
		// TODO: 'enum' instead of 'string'
		string Alignment { get; }
		int QuickStyleIndex { get; set; }
		string Text { get; }
	}
}
