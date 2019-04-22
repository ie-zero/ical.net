using System;

namespace Ical.Net.Collections
{
    public sealed class ItemAddedEventArgs<T> : EventArgs
    {
        public ItemAddedEventArgs(T item, int itemIndex)
        {
            Item = item;
            ItemIndex = itemIndex;
        }

        public T Item { get; }

        public int ItemIndex { get; }
    }
}
