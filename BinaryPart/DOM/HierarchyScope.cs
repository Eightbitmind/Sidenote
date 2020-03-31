namespace Sidenote.DOM
{
	public enum HierarchyScope
	{
		Notebooks = Microsoft.Office.Interop.OneNote.HierarchyScope.hsNotebooks,
		Sections = Microsoft.Office.Interop.OneNote.HierarchyScope.hsSections,
		Pages = Microsoft.Office.Interop.OneNote.HierarchyScope.hsPages,
		Self = Microsoft.Office.Interop.OneNote.HierarchyScope.hsSelf,
		Children = Microsoft.Office.Interop.OneNote.HierarchyScope.hsChildren
	}
}
