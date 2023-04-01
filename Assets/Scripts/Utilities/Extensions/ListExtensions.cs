using System;
using System.Collections.Generic;

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
	}
}