using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
	internal static class Problem118PascalsTriangle
	{
		internal static void Debug(int numRows)
		{
			var rows = Generate(numRows);
			var numWidth = rows.Max(r => r.Max(n => n.ToString().Length));
			for (int i = 0; i < rows.Count; i++)
			{
				var row = rows[i];
				var offset = (rows.Count - i - 1) * numWidth + 1;
				Console.WriteLine
					(
						string.Empty.PadLeft(offset)
						+ string.Join
							(
								string.Empty.PadLeft(numWidth),
								row.Select(n =>
								{
									var numString = n.ToString();
									var remainingWidth = numWidth - numString.Length;
									return string.Empty.PadLeft(remainingWidth / 2)
										+ numString
										+ string.Empty.PadLeft(remainingWidth / 2 + (remainingWidth % 2));
								})
							)
					);
			}
		}

		internal static IList<IList<int>> Generate(int numRows)
		{
			var rows = new int[numRows][];
			var rowPrevious = new int[1];
			rowPrevious[0] = 1;
			rows[0] = rowPrevious;
			for (int i = 1; i < numRows; i++)
			{
				var row = new int[i + 1];
				row[0] = 1;
				for (int k = 1; k < i; k++)
				{
					row[k] = rowPrevious[k - 1] + rowPrevious[k];
				}
				row[^1] = 1;
				rows[i] = row;
				rowPrevious = row;
			}
			return rows;
		}
	}
}
