using System;

namespace LeetCode
{
	public static class Problem5LongestPalindromicSubstring
	{
		public static string LongestPalindrome(string s)
		{
			var indexOddBest = 0;
			var offsetOddBest = 0;
			var indexEvenBest = -1;
			var offsetEvenBest = -1;
			for (int i = 0; i < s.Length; i++)
			{
				var offsetOdd = _GetBestPalindromeOffset(s, i, _IsPalindromicOdd);
				if (offsetOdd > offsetOddBest)
				{
					indexOddBest = i;
					offsetOddBest = offsetOdd;
				}
				var offsetEven = _GetBestPalindromeOffset(s, i, _IsPalindromicEven);
				if (offsetEven > offsetEvenBest)
				{
					indexEvenBest = i;
					offsetEvenBest = offsetEven;
				}
			}
			return offsetOddBest > offsetEvenBest
				? _GetPalindromeOdd(s, indexOddBest, offsetOddBest)
				: _GetPalindromeEven(s, indexEvenBest, offsetEvenBest);
		}

		private static int _GetBestPalindromeOffset(string @string, int index, Func<string, int, int, bool> isPalindromic)
		{
			var offset = 0;
			while (isPalindromic(@string, index, offset))
			{
				offset++;
			}
			offset--;
			return offset;
		}

		private static bool _IsPalindromicOdd(string @string, int index, int offset)
		{
			var indexLeft = index - offset;
			var indexRight = index + offset;
			return indexLeft >= 0 && indexRight < @string.Length && @string[indexLeft] == @string[indexRight];
		}

		private static bool _IsPalindromicEven(string @string, int index, int offset)
		{
			var indexLeft = index - offset;
			var indexRight = index + offset + 1;
			return indexLeft >= 0 && indexRight < @string.Length && @string[indexLeft] == @string[indexRight];
		}

		private static string _GetPalindromeOdd(string @string, int index, int offset)
		{
			return @string.Substring(index - offset, offset * 2 + 1);
		}

		private static string _GetPalindromeEven(string @string, int index, int offset)
		{
			return @string.Substring(index - offset, offset * 2 + 2);
		}
	}
}
