namespace Sidenote.DOM
{
	internal class RuleLines : Node, IRuleLines
	{
		#region IRuleLines members

		public bool IsVisible { get; }

		#endregion

		internal RuleLines(
			bool isVisible,
			uint depth,
			INode parent)
			: base(type: "RuleLines", depth: depth, parent: parent)
		{
			this.IsVisible = isVisible;
		}
	}
}
