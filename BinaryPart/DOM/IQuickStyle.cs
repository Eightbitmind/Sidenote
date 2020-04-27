namespace Sidenote.DOM
{
	public interface IQuickStyle
	{
		uint Index { get; }

		// expressed through INamedObject
		// string Name { get; }

		string FontColor { get; }
		string HighlightColor { get; }

		string FontName { get; }
		double FontSize { get; }

		bool Bold { get; }
		bool Italic { get; }
		bool Underline { get; }
		bool Strikethrough { get; }
		bool Superscript { get; }
		bool Subscript { get; }

		float SpaceBefore { get; }
		float SpaceAfter { get; }
	}
}
