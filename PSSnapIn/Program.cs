using Microsoft.Office.Interop.OneNote;
using Sidenote.DOM;
using Sidenote.Serialization;
using System;

namespace Sidenote
{
	class Program
	{
		static void Main1(string[] args)
		{
			var app = new Application();

			foreach (Window win in app.Windows)
			{
				// Window win
				Console.WriteLine("CurrentNotebookId=\"{0}\"", win.CurrentNotebookId);
				Console.WriteLine("CurrentPageId=\"{0}\"", win.CurrentPageId);
				Console.WriteLine("CurrentSectionGroupId=\"{0}\"", win.CurrentSectionGroupId);
				Console.WriteLine("CurrentSectionId=\"{0}\"", win.CurrentSectionId);
			}

			string hierarchyXml;
			app.GetHierarchy(string.Empty, HierarchyScope.hsPages, out hierarchyXml);


			string pageXml;
			app.GetPageContent("{D8EFE8E3-3707-0A89-280B-4256D6EF8345}{1}{E1951647735768292541881927263794625654060841}", out pageXml, PageInfo.piBasic);
			return;
		}

		static void Main(string[] args)
		{
			var app = new Application();

			IFormatter<IRoot> notebooksFormatter = FormatterManager.NotebooksFormatter;

			IRoot root = notebooksFormatter.Deserialize(app, null);

			foreach (INotebook notebook in root.Notebooks)
			{
				Console.WriteLine("notebook=\"{0}\"", notebook.Name);

				foreach(ISection section in notebook.Children)
				{
					Console.WriteLine("\tsection=\"{0}\"", section.Name);

					foreach(IPage page in section.Children)
					{
						Console.WriteLine("\t\tpage=\"{0}\"", page.Name);
					}
				}
			}
		}
	}
}
// Purple App Scripting Access PASCA
