using System;
using System.Collections.Generic;

namespace LeetCode
{
	internal static class Problem3LongestSubstringWithoutRepeatingCharacters
	{
		private class DistinctQueue<TValue>
		{
			public int Count => _queue.Count;

			private readonly HashSet<TValue> _hashSet = new();
			private readonly Queue<TValue> _queue = new();

			public bool Enqueue(TValue value)
			{
				if (_hashSet.Add(value))
				{
					_queue.Enqueue(value);
					return true;
				}
				return false;
			}

			public bool TryDequeue(out TValue value)
			{
				if (_queue.TryDequeue(out value))
				{
					_hashSet.Remove(value);
					return true;
				}
				return false;
			}
		}

		internal static int LengthOfLongestSubstring(string s)
		{
			var run = new DistinctQueue<char>();
			var best = 0;
			for (int i = 0; i < s.Length; i++)
			{
				var character = s[i];
				if (!run.Enqueue(character))
				{
					best = Math.Max(best, run.Count);
					while (run.TryDequeue(out var characterExisting))
					{
						if (characterExisting == character)
						{
							break;
						}
					}
				}
				run.Enqueue(character);
			}
			return Math.Max(best, run.Count);
		}
	}
}
