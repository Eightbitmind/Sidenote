using System.Management.Automation;

namespace Sidenote.PowerShell
{
	[Cmdlet("Open", "ONHierarchy")]
	public class OpenONHierarchyCmdlet : Cmdlet
	{
		[Parameter(
			HelpMessage = "Path",
			ValueFromPipeline = false,
			Mandatory = true,
			Position = 0)]
		public string Path;

		[Parameter(
			HelpMessage = "Path",
			ValueFromPipeline = false,
			Mandatory = false,
			Position = 0)]
		public string RelativeToObjectId;

		protected override void ProcessRecord()
		{
			string objectId;
			ApplicationManager.Application.OpenHierarchy(Path, RelativeToObjectId, out objectId);
		}
	}
}
