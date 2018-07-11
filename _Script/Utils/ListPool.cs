using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.stubs.utils
{
	public static class ListPool<T>
	{
		static List<List<T>> pool = new List<List<T>>();
		public static List<T> Get()
		{
			if (pool.Count > 0)
			{
				var r = pool[pool.Count - 1];
				pool.RemoveAt(pool.Count - 1);
				return r;
			}
			else
			{
				return new List<T>();
			}
		}
		public static void Release(List<T> list)
		{
			list.Clear();
			pool.Add(list);
		}

		public static void Clear()
		{
			pool.Clear();
		}
	}
}
