using Microsoft.Office.Interop.OneNote;

namespace Sidenote.DOM
{
	internal class Title : Node, ITitle
	{
		#region ITitle members

		public string Language
		{
			get;
			internal set;
		}

		#endregion

		internal Title(uint depth, INode parent)
			: base(type: "Title", depth: depth, parent: parent)
		{
		}
	}
}
