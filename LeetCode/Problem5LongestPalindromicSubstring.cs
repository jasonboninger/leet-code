namespace LeetCode
{
	public static class Problem5LongestPalindromicSubstring
	{
		private delegate bool FIsPalindromic(string @string, int index, int offset);

		public static string LongestPalindrome(string s)
		{
			var indexOddBest = 0;
			var offsetOddBest = 0;
			FIsPalindromic isPalindromicOdd = _IsPalindromicOdd;
			var indexEvenBest = -1;
			var offsetEvenBest = -1;
			FIsPalindromic isPalindromicEven = _IsPalindromicEven;
			for (int i = 0; i < s.Length; i++)
			{
				_SetBestPalindromeIndexAndOffset(ref indexOddBest, ref offsetOddBest, s, i, isPalindromicOdd);
				_SetBestPalindromeIndexAndOffset(ref indexEvenBest, ref offsetEvenBest, s, i, isPalindromicEven);
			}
			return offsetOddBest > offsetEvenBest
				? _GetPalindromeOdd(s, indexOddBest, offsetOddBest)
				: _GetPalindromeEven(s, indexEvenBest, offsetEvenBest);
		}

		private static void _SetBestPalindromeIndexAndOffset
		(
			ref int indexBest,
			ref int offsetBest,
			string @string,
			int index,
			FIsPalindromic isPalindromic
		)
		{
			if (!isPalindromic(@string, index, offsetBest + 1))
			{
				return;
			}
			var offset = 0;
			while (isPalindromic(@string, index, offset))
			{
				offset++;
			}
			offset--;
			if (offset > offsetBest)
			{
				indexBest = index;
				offsetBest = offset;
			}
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
