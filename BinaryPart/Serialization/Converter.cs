using System;

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

		internal static string ToString(DateTime val)
		{
			// https://en.wikipedia.org/wiki/ISO_8601#Combined_date_and_time_representations, "Z" - zero timezone
			return val.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}
	}
}
