using Microsoft.Office.Interop.OneNote;
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
			var app = new Application();
			IFormatter<IRoot> notebooksFormatter = FormatterManager.NotebooksFormatter;
			IRoot root = notebooksFormatter.Deserialize(app, null);
			WriteObject(root);
		}
	}
}
