namespace Relebuf
{
    public sealed class CircularBufferRecord<T>
    {
        public T Record { get; set; }
        public CircularBufferState State { get; set; }

        public CircularBufferRecord(T record, CircularBufferState? state = null)
        {
            Record = record;
            State = state ?? CircularBufferState.Empty;
        }
    }
}
