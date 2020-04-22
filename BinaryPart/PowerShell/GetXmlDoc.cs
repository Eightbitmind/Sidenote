using System.Management.Automation;
using System.Xml;

namespace Sidenote.PowerShell
{
	[Cmdlet("Get", "XmlDoc")]
	public class GetXmlDoc : Cmdlet
	{
		[Parameter(
			HelpMessage = "InnerXml",
			ValueFromPipeline = true,
			Mandatory = true,
			Position = 0)]
		public string InnerXml;

		protected override void ProcessRecord()
		{
			var doc = new XmlDocument();
			doc.LoadXml("<InnerXmlRoot>" + this.InnerXml + "</InnerXmlRoot>");
			WriteObject(doc);
		}
	}
}
