using Sidenote.DOM;
using Sidenote.Serialization;
using System.Management.Automation;

namespace Sidenote.PowerShell
{
	[Cmdlet("Get", "ONRoot")]
	public class GetONRootCmdlet : Cmdlet
	{
		protected override void ProcessRecord()
		{
			INode root = new Node(type: "Root", depth: 0, parent: null);
			RootContentFormatter.Deserialize(root);
			WriteObject(root);
		}
	}
}
