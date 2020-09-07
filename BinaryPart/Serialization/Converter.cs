using System;

namespace Sidenote.Serialization
{
	internal static class Converter
	{
		internal static string ToString(bool val)
		{
			return val ? "true" : "false";
		}

		internal static string ToString(uint val)
		{
			return val.ToString();
		}

		internal static string ToString(float val)
		{
			return val.ToString("N1");
		}

		internal static string ToString(double val)
		{
			if (val - (int)val == 0)
			{
				// This mimics ON serialization behavior. This case should only be needed when
				// we're analyzing serialization fidelity.
				return val.ToString("F1");
			}

			return val.ToString();
		}

		internal static string ToString(DateTime val)
		{
			// https://en.wikipedia.org/wiki/ISO_8601#Combined_date_and_time_representations, "Z" - zero timezone
			return val.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}
	}
}
