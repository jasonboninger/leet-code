namespace LeetCode
{
	public static class Problem4MedianOfTwoSortedArrays
	{
		public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
		{
			var length1 = nums1.Length;
			var length2 = nums2.Length;
			var lengthMedian = length1 + length2;
			var lengthMedianStop = lengthMedian / 2 + 1;
			var averageMedian = (lengthMedian % 2) == 0;
			var index1 = 0;
			var index2 = 0;
			var value1 = index1 < length1 ? nums1[index1] : int.MaxValue;
			var value2 = index2 < length2 ? nums2[index2] : int.MaxValue;
			var valuePrevious = int.MinValue;
			var valueCurrent = int.MinValue;
			for (int i = 0; i < lengthMedianStop; i++)
			{
				valuePrevious = valueCurrent;
				if (value1 < value2)
				{
					valueCurrent = value1;
					value1 = ++index1 < length1 ? nums1[index1] : int.MaxValue;

				}
				else
				{
					valueCurrent = value2;
					value2 = ++index2 < length2 ? nums2[index2] : int.MaxValue;
				}
			}
			if (averageMedian)
			{
				return (valuePrevious + valueCurrent) / (double)2;
			}
			return valueCurrent;
		}
	}
}
