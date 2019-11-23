using System.Management.Automation;

namespace Sidenote.Client
{
	[Cmdlet("Test", "Sidenote")]
	public class TestCmdlet : Cmdlet
	{
		protected override void ProcessRecord()
		{
			WriteObject("Hello from Sidenote");
			base.ProcessRecord();
		}
	}
}
