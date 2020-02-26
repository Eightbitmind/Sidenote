using System.Diagnostics;
using System.Management.Automation;

namespace Sidenote.Client
{
	// Objects addressable with the 'HierarchyObjectId' parameter
	// - Section
	// - Page
	// Objects that cannot be addressed with  the 'HierarchyObjectId' parameter
	// - Outline (HRESULT 0x80042014)
	// - OutlineElement (HRESULT 0x80042014)
	//
	// HierarchyObjectId | ObjectId | Description
	//         -         |     x    | no effect
	//         x         |     x    | switches to HierarchyObject and selects Object
	//         x         |     -    | switches to HierarchyObject (regardless of selection state of parent hierarchy objects),
	//                   |          | leaves Object selection w/in HierarchyObject unchanged

	[Cmdlet("Select", "ONObject")]
	public class SelectONObject : Cmdlet
	{
		[Parameter(
			HelpMessage = "Object",
			ValueFromPipeline = true,
			Mandatory = false,
			Position = 0)]
		public DOM.INode Object { get; set; }

		[Parameter(
			HelpMessage = "NewWindow",
			ValueFromPipeline = true,
			Mandatory = false,
			Position = 2)]
		public bool NewWindow = false;

		protected override void ProcessRecord()
		{
			string hierarchyObjectId = null;
			string objectId = null;

			for (DOM.INode current = this.Object; current != null; current = current.Parent)
			{
				var identifiableObject = current as DOM.IIdentifiableObject;
				if (identifiableObject == null) continue;

				if (current is DOM.Notebook || current is DOM.Section || current is DOM.Page)
				{
					if (hierarchyObjectId == null) hierarchyObjectId = identifiableObject.ID;
					continue;
				}

				if (objectId == null)
				{
					objectId = identifiableObject.ID;
				}
			}

			Debug.Assert(hierarchyObjectId != null);

			ApplicationManager.Application.NavigateTo(hierarchyObjectId, objectId, this.NewWindow);
		}
	}
}
