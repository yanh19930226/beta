using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util
{
	public static class RandomHelper
	{
		private const string CharSet = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";

		private const string HasSpecialCharSet = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";

		private static readonly Random Random = new Random();

		public static double NextDouble()
		{
			return Random.NextDouble();
		}

		public static string NextString(int length, bool isHasSpecialChar = true)
		{
			char[] array = new char[length];
			string text = isHasSpecialChar ? "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ" : "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
			for (int i = 0; i < length; i++)
			{
				int index = Random.Next(text.Length);
				array[i] = text[index];
			}
			return string.Join<char>("", array);
		}

		public static int Next()
		{
			return Random.Next();
		}

		public static int Next(int maxValue)
		{
			return Random.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return Random.Next(minValue, maxValue);
		}
	}
}
