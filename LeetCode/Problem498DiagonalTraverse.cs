namespace LeetCode
{
	internal static class Problem498DiagonalTraverse
	{
		internal static int[] FindDiagonalOrder(int[][] mat)
		{
			var indexReverseHorizontal = mat[0].Length - 1;
			var indexReverseVertical = mat.Length - 1;
			var lengthTotal = (indexReverseHorizontal + 1) * (indexReverseVertical + 1);
			var indexHorizontal = 0;
			var indexVertical = 0;
			var indexTotal = 0;
			var rising = true;
			var values = new int[lengthTotal];
			while (true)
			{
				values[indexTotal++] = mat[indexVertical][indexHorizontal];
				if (indexTotal == lengthTotal)
				{
					return values;
				}
				if (rising)
				{
					if (indexHorizontal == indexReverseHorizontal)
					{
						indexVertical++;
						rising = false;
					}
					else if (indexVertical == 0)
					{
						indexHorizontal++;
						rising = false;
					}
					else
					{
						indexHorizontal++;
						indexVertical--;
					}
				}
				else
				{
					if (indexVertical == indexReverseVertical)
					{
						indexHorizontal++;
						rising = true;
					}
					else if (indexHorizontal == 0)
					{
						indexVertical++;
						rising = true;
					}
					else
					{
						indexHorizontal--;
						indexVertical++;
					}
				}
			}
		}
	}
}
