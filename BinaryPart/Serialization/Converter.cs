namespace Sidenote.Serialization
{
	internal static class Converter
	{
		internal static string ToString(bool val)
		{
			return val ? "true" : "false";
		}

		internal static string ToString(float val)
		{
			return val.ToString("N1");
		}
	}
}
