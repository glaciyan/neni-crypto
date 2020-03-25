using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace crypto.Desktop.Cnsl
{
    public class ArrayEnumerator<T> : IEnumerator<T>, IEnumerable<T> where T : class
    {
        public int CurrentIndex { get; set; } = -1;
        private readonly T[] _innerArray;
        
        public T Current => _innerArray[CurrentIndex];
        object? IEnumerator.Current => Current;

        public ArrayEnumerator(T[] array)
        {
            _innerArray = array;
        }
        
        public T? NextOrNull()
        {
            if (MoveNext())
            {
                return Current;
            }

            return null;
        }

        public bool MoveNext()
        {
            return _innerArray.Length > ++CurrentIndex;
        }

        public void Reset()
        {
            CurrentIndex = 0;
        }

        public void Dispose()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}