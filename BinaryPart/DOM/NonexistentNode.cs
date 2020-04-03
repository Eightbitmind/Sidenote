namespace Sidenote.DOM
{
	// Development staging artefact.
	internal class NonexistentNode : Node
	{
		internal NonexistentNode(
			uint depth,
			INode parent)
			: base(type: "NonexistentNode", depth: depth, parent: parent)
		{
			throw new System.Exception("this type should never get instantiated");
		}
	}
}
