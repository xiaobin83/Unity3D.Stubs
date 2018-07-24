using UnityEngine; 

namespace x600d1dea.stubs.collections
{
	public class Array<T>
	{
		public const int initialCap = 8;

		public enum Growth
		{
			OneAndHalfCapacity,
			TwiceCapacity,
		}
		public Growth growth = Growth.OneAndHalfCapacity;


		public T[] array { get; private set;}
		public uint length { get; private set; }
		public uint capacity { get; private set; }

		public Array()
		{
			Clear();
		}

		public Array(uint capacity = initialCap, Growth growth = Growth.OneAndHalfCapacity)
		{
			this.growth = growth;
			if (growth == Growth.TwiceCapacity)
			{
				capacity = (uint)Mathf.NextPowerOfTwo((int)capacity);
			}
			array = new T[capacity];
			length = 0;
			this.capacity = capacity;
		}

		public Array(T elem, uint count, Growth growth = Growth.OneAndHalfCapacity)
		{
			this.growth = growth;
			if (growth == Growth.TwiceCapacity)
			{
				capacity = (uint)Mathf.NextPowerOfTwo((int)count);
			}
			else
			{
				capacity = count;
			}
			array = new T[capacity];
			for (int i = 0; i < count; ++i)
			{
				array[i] = elem;
			}
			length = count;
			capacity = count;
		}

		public void Push(T elem)
		{
			EnsureOne();
			array[length] = elem;
			++length;
		}

		public T Pop()
		{
			if (length > 0)
			{
				var t = array[length - 1];
				--length;
				return t;
			}
			throw new System.IndexOutOfRangeException();
		}

		public T EnsureElementAt(uint index)
		{
			EnsureCapacity(index + 1);
			if (index >= length)
				length = index + 1;
			return array[index];
		}

		public void Resize(uint size)
		{
			EnsureCapacity(size);
			length = size;
		}

		public void Clear()
		{
			array = new T[initialCap];
			capacity = initialCap;
			length = 0;
		}

		public T this[int i]
		{
			get
			{
				CheckRange(i);
				if (i >= 0)
					return array[i];
				else
					return array[length+i];
			}
			set
			{
				CheckCapacity(i);
				if (i >= length)
				{
					length = (uint)i + 1;
				}
				if (i >= 0)
				{
					array[i] = value;
				}
				else
				{
					CheckRange(i);
					array[length+i] = value;
				}
			}
		}

		public void EnsureCapacity(uint newCap)
		{
			if (newCap > capacity)
			{
				if (growth == Growth.TwiceCapacity)
					newCap = (uint)Mathf.NextPowerOfTwo((int)newCap);
				var newArray = new T[newCap];
				System.Array.Copy(array, newArray, length);
				array = newArray;
				capacity = newCap;
			}
		}

		void EnsureOne()
		{
			if (length + 1 >= capacity)
			{
				if (growth == Growth.TwiceCapacity)
				{
					EnsureCapacity(capacity + capacity);
				}
				else if (growth == Growth.OneAndHalfCapacity)
				{
					EnsureCapacity(capacity + capacity/2);
				}
				else
				{
					throw new System.InvalidOperationException();
				}
			}
		}

		void CheckCapacity(int c)
		{
			if (c >= capacity)
			{
				throw new System.IndexOutOfRangeException();
			}
		}

		void CheckRange(int i)
		{
			if (i >= length || i < -length)
			{
				throw new System.IndexOutOfRangeException();
			}
		}
	}
}
