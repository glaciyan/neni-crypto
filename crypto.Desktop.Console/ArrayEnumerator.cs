using System.Collections;
using System.Collections.Generic;

namespace crypto.Desktop.Cnsl
{
    public class ArrayEnumerator<T> : IEnumerator<T>, IEnumerable<T> where T : class
    {
        private readonly T[] _innerArray;

        public ArrayEnumerator(T[] array)
        {
            _innerArray = array;
        }

        public int CurrentIndex { get; set; } = -1;

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Current => _innerArray[CurrentIndex];
        object? IEnumerator.Current => Current;

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

        public T? NextOrNull()
        {
            return MoveNext() ? Current : null;
        }
    }
}