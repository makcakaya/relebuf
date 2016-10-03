using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Relebuf
{
    public sealed class CircularBuffer<T> : IEnumerable<T>
    {
        private readonly uint _capacity;
        private CircularBufferRecord<T>[] _records = new CircularBufferRecord<T>[] { new CircularBufferRecord<T>(default(T), CircularBufferState.Empty) };
        private uint _cursor = 0;

        public CircularBuffer(uint capacity)
        {
            if (capacity == 0) { throw new ArgumentOutOfRangeException(nameof(capacity)); }

            _capacity = capacity;
        }

        public void Insert(T item)
        {
            var removed = _records.Where(r => r.State == CircularBufferState.Removed);
            if (removed.Count() > _capacity / 2)
            {
                ArrangeMemory();
            }

            var empty = _records.Where(r => r.State == CircularBufferState.Empty);
            if (empty.Count() == 0)
            {
                // If we have room to increase the size, claim more memory.
                if (_records.Length < _capacity)
                {
                    ArrangeMemory();
                }
                else // We've hit the limit.
                {
                    if (removed.Count() > 0)
                    {
                        ArrangeMemory();
                    }
                    else
                    {
                        if (_cursor == _capacity)
                        {
                            _cursor = 0;
                        }
                    }
                }
            }

            _records[_cursor].Record = item;
            _records[_cursor].State = CircularBufferState.Occupied;
            _cursor++;
        }

        private void ArrangeMemory()
        {
            var newSize = Math.Max(Math.Min(_capacity, _records.Length * 2), 16);
            var newRecords = new CircularBufferRecord<T>[newSize];
            var recordsCursor = 0;
            for (var i = 0; i < newRecords.Length; recordsCursor++)
            {
                if (i >= _records.Length)
                {
                    newRecords[i] = new CircularBufferRecord<T>(default(T), CircularBufferState.Empty);
                    i++;
                }
                else if (_records[recordsCursor].State == CircularBufferState.Occupied)
                {
                    newRecords[i] = new CircularBufferRecord<T>(_records[recordsCursor].Record, CircularBufferState.Occupied);
                    i++;
                }
            }

            _records = newRecords;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CircularBufferEnumerator<T>(_records, _cursor);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CircularBufferEnumerator<T>(_records, _cursor);
        }
    }
}
