using Sidenote.DOM;
using Sidenote.Serialization;
using System.Management.Automation;

namespace Sidenote.Client
{
	[Cmdlet("Get", "NotebookRoot")]
	public class GetNotebookRootCmdlet : Cmdlet
	{
		protected override void ProcessRecord()
		{
			INode root = new Node(null);
			IFormatter notebooksFormatter = FormatterManager.RootContentFormatter;
			notebooksFormatter.Deserialize(root);
			WriteObject(root);
		}
	}
}
