namespace Sidenote.Serialization
{
	static internal class FormatterManager
	{
		public const string Xml2013 = "http://schemas.microsoft.com/office/onenote/2013/onenote";

		public static IFormatter RootContentFormatter
		{
			get { return new RootContentFormatter(); }
		}

		public static IFormatter NotebookContentFormatter
		{
			get { return new NotebookContentFormatter(); }
		}

		public static IFormatter SectionContentFormatter
		{
			get { return new SectionContentFormatter(); }
		}
	}
}
