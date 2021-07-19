using System;
using System.Collections.Generic;

namespace LeetCode
{
	internal static class Problem1TwoSum
	{
		internal static int[] TwoSum(int[] nums, int target)
		{
			var numNeededToIndexMappings = new Dictionary<int, int>();
			for (int i = 0; i < nums.Length; i++)
			{
				var num = nums[i];
				if (numNeededToIndexMappings.TryGetValue(num, out var index))
				{
					return new int[] { index, i };
				}
				numNeededToIndexMappings[target - num] = i;
			}
			throw new InvalidOperationException("No two numbers sum to target.");
		}
	}
}
