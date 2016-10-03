using System;
using System.Collections;
using System.Collections.Generic;

namespace Relebuf
{
    public sealed class CircularBufferEnumerator<T> : IEnumerator<T>
    {
        private readonly CircularBufferRecord<T>[] _records;
        private int _cursor;
        private uint _lastOccupiedIndex;

        public CircularBufferEnumerator(CircularBufferRecord<T>[] records, uint lastOccupiedIndex)
        {
            _records = records;
            _lastOccupiedIndex = lastOccupiedIndex;

            _cursor = -1;
            for (var i = 0; i < _lastOccupiedIndex; i++)
            {
                if (_records[i].State == CircularBufferState.Occupied)
                {
                    _cursor = i - 1;
                    break;
                }
            }
        }

        public T Current { get { return _records[_cursor].Record; } }

        object IEnumerator.Current { get { return _records[_cursor].Record; } }

        public bool MoveNext()
        {
            if (_cursor == _lastOccupiedIndex) { return false; }

            for (var i = _cursor + 1; i < _lastOccupiedIndex; i++)
            {
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
