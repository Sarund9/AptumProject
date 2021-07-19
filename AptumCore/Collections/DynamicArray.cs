using System;
using System.Collections.Generic;
using System.Text;

namespace AptumEngine.Core
{
    public class DynamicArray<T> where T : struct
    {
        T[] items;
        int count;

        /// <summary> Creates a new DynamicArray </summary>
        public DynamicArray(int size = 32)
        {
            items = new T[size];
            count = 0;
        }

        /// <summary> Reference an Item in the Actual Array. Meant for Iteration </summary>
        public ref T this[int i] => ref items[i];

        /// <summary> Artificial count </summary>
        public int Count => count;

        /// <summary> Actual lenght of the Array </summary>
        public int Lenght => items.Length;

        /// <summary> Get the next value from the Array. Resize the Array if nesesary </summary>
        public ref T Get()
        {
            if (count >= items.Length)
            {
                Array.Resize(ref items, items.Length * 2);
            }

            return ref items[count++];
        }

        /// <summary> Ensure the array is at least of a surten size </summary>
        public bool EnsureCapacity(int size)
        {
            if (size >= items.Length)
            {
                Array.Resize(ref items, size);
                return true;
            }
            else return false;
        }

        /// <summary> Sets all data in the Array to be overriden </summary>
        public void Reset(int? shrinkTo = null)
        {
            count = 0;

            if (shrinkTo is int @int)
            {
                Array.Resize(ref items, @int);
            }
        }

    }
}
