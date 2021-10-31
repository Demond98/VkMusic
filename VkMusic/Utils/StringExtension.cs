using System;

namespace VkMusic
{
	public static class StringExtension
	{
		public static string Truncate(this string value, int maxLength)
		{
			if (value == null)
				return null;

			return value.Length > maxLength
				? value[..maxLength]
				: value;
		}
	}
}