
using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Text;

namespace Sidenote.Serialization
{
	interface IFormatter
	{
		void Serialize(INode node, StringBuilder xml);
		bool Deserialize(Application app, INode parent);
	}
}
