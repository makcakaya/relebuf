using System;
using System.Collections;
using System.Collections.Generic;

namespace Relebuf
{
    public sealed class CircularBuffer<T> : IEnumerable<T>
    {
        private readonly uint _capacity;
        private T[] _records = new T[0];
        private uint _cursor = 0;

        public CircularBuffer(uint capacity)
        {
            if (capacity == 0) { throw new ArgumentOutOfRangeException(nameof(capacity)); }

            _capacity = capacity;
        }

        public void Insert(T item)
        {
            if (_cursor == _capacity)
            {
                _cursor = 0;
            }
            else
            {
                EnsureSize(_cursor + 1);
            }

            _records[_cursor] = item;
            _cursor++;
        }

        private void EnsureSize(uint size)
        {
            if (_records.Length >= size)
            {
                return;
            }

            if (size > _capacity)
            {
                size = _capacity;
            }
            else
            {
                size *= 2;
            }

            // Create a bigger array and copy current records to it.
            var newRecords = new T[size];
            Array.Copy(_records, newRecords, _records.Length);

            // Register new array.
            _records = newRecords;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_records).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _records.GetEnumerator();
        }
    }
}
