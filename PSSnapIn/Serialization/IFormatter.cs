
using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Text;

namespace Sidenote.Serialization
{
	interface IFormatter<T>
	{
		void Serialize(T obj, StringBuilder xml);
		T Deserialize(Application app, INode parent);
	}
}
