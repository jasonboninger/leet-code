namespace LeetCode
{
	internal static class Problem10RegularExpressionMatching
	{
		internal static bool IsMatch(string s, string p)
		{
			return _IsMatch(s, 0, _GetExpression(p), 0);
		}

		private static bool _IsMatch(string @string, int indexString, (char character, bool zeroOrMore)[] expression, int indexExpression)
		{
			if (indexString == @string.Length)
			{
				while (indexExpression < expression.Length)
				{
					if (!expression[indexExpression++].zeroOrMore)
					{
						return false;
					}
				}
				return true;
			}
			if (indexExpression == expression.Length)
			{
				return false;
			}
			var characterString = @string[indexString];
			var (characterExpression, zeroOrMore) = expression[indexExpression];
			var match = characterExpression == characterString || characterExpression == '.';
			if (zeroOrMore)
			{
				if (match && _IsMatch(@string, indexString + 1, expression, indexExpression))
				{
					return true;
				}
				return _IsMatch(@string, indexString, expression, indexExpression + 1);
			}
			if (match)
			{
				return _IsMatch(@string, indexString + 1, expression, indexExpression + 1);
			}
			return false;
		}

		private static (char character, bool zeroOrMore)[] _GetExpression(string expression)
		{
			var zeroOrMoreCharactersCount = 0;
			for (int i = 0; i < expression.Length; i++)
			{
				if (expression[i] == '*')
				{
					zeroOrMoreCharactersCount++;
				}
			}
			var expressionCharacters = new (char character, bool zeroOrMore)[expression.Length - zeroOrMoreCharactersCount];
			var expressionCharactersIndex = 0;
			for (int i = 0; i < expression.Length; i++)
			{
				var character = expression[i];
				if (character == '*')
				{
					expressionCharacters[expressionCharactersIndex - 1].zeroOrMore = true;
				}
				else
				{
					expressionCharacters[expressionCharactersIndex++] = (character, zeroOrMore: false);
				}
			}
			return expressionCharacters;
		}
	}
}
