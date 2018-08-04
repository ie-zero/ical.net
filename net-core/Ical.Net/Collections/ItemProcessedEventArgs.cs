using System;

namespace Ical.Net.Collections
{
    public sealed class ItemProcessedEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public int Index { get; }

        public ItemProcessedEventArgs(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}
