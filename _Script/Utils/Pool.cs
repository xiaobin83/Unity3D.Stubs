using System.Collections.Generic;

namespace x600d1dea.stubs.utils
{
	public class Pool<Collection> where Collection : new()
	{
		protected static List<Collection> pool = new List<Collection>();

		public static Collection Get()
		{
			if (pool.Count > 0)
			{
				var r = pool[pool.Count - 1];
				pool.RemoveAt(pool.Count - 1);
				return r;
			}
			return new Collection();
		}

		public static void Release(Collection c)
		{
			pool.Add(c);
		}

		public static void Clear()
		{
			pool.Clear();
		}

	}
}
