using System;

namespace Sidenote.Utilities
{
	internal static class Validator
	{
		public static void ValidateArgNotNull(object arg, string paramName)
		{
			if (arg == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		public static void ValidateArgNotNullOrEmpty(string arg, string paramName)
		{
			if (string.IsNullOrEmpty(arg))
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ValidateArgNotIntPtrZero(IntPtr arg, string paramName)
		{
			if (arg == IntPtr.Zero)
			{
				throw new ArgumentException(paramName);
			}
		}
	}

}
