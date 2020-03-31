using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using System.Management.Automation;

namespace Sidenote.PowerShell
{
	[Cmdlet("Get", "ONXml")]
	public class GetONXml : Cmdlet
	{
		[Parameter(
			HelpMessage = "Object",
			ValueFromPipeline = true,
			Mandatory = false,
			Position = 0)]
		public DOM.INode Object { get; set; }

		[Parameter(
			HelpMessage = "ID",
			ValueFromPipeline = false,
			Mandatory = false,
			Position = 0)]
		public string ID;

		[Parameter(
			HelpMessage = "Scope",
			ValueFromPipeline = false,
			Mandatory = false,
			Position = 0)]
		public Sidenote.DOM.HierarchyScope Scope = Sidenote.DOM.HierarchyScope.Children;

		protected override void ProcessRecord()
		{
			// start node ID 'null' gets root node XML
			string startNodeId = null;

			if (this.Object != null)
			{
				startNodeId = ((IIdentifiableObject)this.Object).ID;
			}
			else if(this.ID != null)
			{
				startNodeId = this.ID;
			}

			Microsoft.Office.Interop.OneNote.HierarchyScope rawScope = (Microsoft.Office.Interop.OneNote.HierarchyScope)this.Scope;

			string xml;
			ApplicationManager.Application.GetHierarchy(startNodeId, rawScope, out xml);

			WriteObject(xml);
		}
	}
}
