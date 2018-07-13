using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.stubs.utils
{
	public class ListPool<T> : Pool<List<T>>
	{
		new public static void Release(List<T> list)
		{
			list.Clear();
			Pool<List<T>>.Release(list);
		}
	}
}
