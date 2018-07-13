using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.stubs.utils
{
	using collections;

	public class ArrayPool<T> 
	{
		const int bucketCount = 16;

		static Array<Array<Array<T>>> buckets = new Array<Array<Array<T>>>(bucketCount);

		static readonly int[] _lookup = {
			32, 0, 1, 26, 2, 23, 27, 0, 3, 16, 24, 30, 28, 11, 0, 13, 4, 7, 17,
			0, 25, 22, 31, 15, 29, 10, 12, 6, 0, 21, 14, 9, 5, 20, 8, 19, 18
		};

		// trailing zero
		static int IndexOfBucket(int bucketSize)
		{
			return _lookup[(bucketSize & -bucketSize) % 37];
		}

		public static Array<T> Get(uint capacity = Array<T>.initialCap)
		{
			var bucketSize = Mathf.NextPowerOfTwo((int)capacity);
			var index = IndexOfBucket(bucketSize);
			for (int i = index; i < buckets.length; ++i)
			{
				var b = buckets[i];
				if (b != null && b.length > 0)
				{
					return b.Pop();
				}
			}
			return new Array<T>((uint)bucketSize, Array<T>.Growth.TwiceCapacity);
		}

		public static void ClearAndRelease(Array<T> arr)
		{
			arr.Clear();
			Release(arr);
		}

		public static void Release(Array<T> arr)
		{
			arr.Resize(0);
			var bucketSize = Mathf.NextPowerOfTwo((int)arr.capacity);
			// in case released array not create by pool
			arr.growth = Array<T>.Growth.TwiceCapacity;
			arr.EnsureCapacity((uint)bucketSize);
			var index = IndexOfBucket(bucketSize);
			var b = buckets.EnsureElementAt((uint)index);
			if (b == null)
			{
				b = new Array<Array<T>>();
				buckets[index] = b;
			}
			b.Push(arr);
		}
	}
}
