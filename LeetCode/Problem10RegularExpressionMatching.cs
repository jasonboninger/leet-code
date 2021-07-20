namespace LeetCode
{
	internal static class Problem10RegularExpressionMatching
	{
		internal static bool IsMatch(string s, string p)
		{
			return _IsMatch(s, 0, p, 0);
		}

		private static bool _IsMatch(string @string, int indexString, string expression, int indexExpression)
		{
			if (indexString == @string.Length)
			{
				if (indexExpression == expression.Length)
				{
					return true;
				}
				var optionals = 0;
				while (optionals < expression.Length && expression[^(optionals + 1)] == '*')
				{
					optionals += 2;
				}
				return indexExpression >= expression.Length - optionals;
			}
			if (indexExpression == expression.Length)
			{
				return false;
			}
			var characterString = @string[indexString];
			var characterExpression = expression[indexExpression];
			if 
			(
				(
					characterString == characterExpression
					|| characterExpression == '.'
				)
				&& _IsMatch(@string, indexString + 1, expression, indexExpression + 1)
			)
			{
				return true;
			}
			if
			(
				characterExpression == '*'
				&& _IsMatch(@string, indexString, expression, indexExpression - 1)
			)
			{
				return true;
			}
			if
			(
				indexExpression < expression.Length - 1
				&& expression[indexExpression + 1] == '*'
				&& _IsMatch(@string, indexString, expression, indexExpression + 2)
			)
			{
				return true;
			}
			return false;
		}
	}
}
