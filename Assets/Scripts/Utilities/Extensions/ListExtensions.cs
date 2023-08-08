using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.Utilities;

namespace SaturnRPG.Utilities.Extensions
{
	public static class ListExtensions
	{
		// Taken from https://stackoverflow.com/questions/273313/randomize-a-listt
		
		private static readonly Random _rng = new Random();
		
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = _rng.Next(n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}  
		}

		public static bool IsInRange(this IList list, int index)
		{
			return index >= 0 && index < list.Count;
		}

		public static T GetIfInRange<T>(this IList<T> list, int index)
		{
			return index >= 0 && index < list.Count ? list[index] : default;
		}

		public static void Retain<T>(this IList<T> list, [NotNull] Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
				if (!predicate(list[i]))
					list.RemoveAt(i--);
		}

		public static T PopEnd<T>(this IList<T> list)
		{
			if (list.Count == 0) return default;

			var item = list[^1];
			list.RemoveAt(list.Count - 1);
			return item;
		}

		public static int FirstIndexWhere<T>(this IList<T> list, [NotNull] Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
				if (predicate(list[i]))
					return i;

			return -1;
		}
		
		public static T FirstWhere<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate)
		{
			foreach (var item in enumerable)
				if (predicate(item))
					return item;

			return default;
		}

		public static bool TryFirstWhere<T>(this IEnumerable<T> enumerable, [NotNull] Func<T, bool> predicate, out T first)
		{
			foreach (var item in enumerable)
			{
				if (!predicate(item)) continue;
				
				first = item;
				return true;
			}

			first = default;
			return false;
		}
	}
}