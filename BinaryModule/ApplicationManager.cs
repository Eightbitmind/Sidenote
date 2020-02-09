using Microsoft.Office.Interop.OneNote;

namespace Sidenote
{
	internal static class ApplicationManager
	{
		public static Application Application
		{
			get
			{
				if (ApplicationManager.app == null)
				{
					ApplicationManager.app = new Application();
				}

				return ApplicationManager.app;
			}
		}

		private static Application app;
	}
}
