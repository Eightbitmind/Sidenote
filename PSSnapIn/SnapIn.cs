using System.ComponentModel;
using System.Management.Automation;

namespace Sidenote
{
	[RunInstaller(true)]
	public class SnapIn : PSSnapIn
	{
		public SnapIn() : base()
		{
		}

		public override string Name
		{
			get { return "Sidenote"; }
		}

		public override string Vendor
		{
			get { return "Microsoft"; }
		}

		public override string Description
		{
			get { return "Enables PowerShell access to OneNote notebooks."; }
		}
	}
}
