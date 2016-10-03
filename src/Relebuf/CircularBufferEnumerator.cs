using System;
using System.Collections;
using System.Collections.Generic;

namespace Relebuf
{
    public sealed class CircularBufferEnumerator<T> : IEnumerator<T>
    {
        private readonly CircularBufferRecord<T>[] _records;
        private uint _cursor;
        private uint _lastOccupiedIndex;
        private bool _looped;
        private bool _moved = false;

        public CircularBufferEnumerator(CircularBufferRecord<T>[] records, uint firstItemIndex, uint lastOccupiedIndex)
        {
            _records = records;
            _lastOccupiedIndex = lastOccupiedIndex;

            _cursor = firstItemIndex == records.Length ? 0 : firstItemIndex;
            _looped = lastOccupiedIndex < firstItemIndex;
        }

        public T Current { get { return _records[_cursor].Record; } }

        object IEnumerator.Current { get { return _records[_cursor].Record; } }

        public bool MoveNext()
        {
            if (!_moved)
            {
                _moved = true;
                if (_records[_cursor].State == CircularBufferState.Occupied)
                {
                    return true;
                }
            }

            var i = _cursor;
            while (i != _lastOccupiedIndex)
            {
                if (++i == _records.Length) { i = 0; }

                if (_records[i].State == CircularBufferState.Occupied)
                {
                    _cursor = i;
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
