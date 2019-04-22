using System;

namespace Ical.Net.Collections
{
    public sealed class ItemRemovedEventArgs<T> : EventArgs
    {
        public ItemRemovedEventArgs(T item, int itemIndex)
        {
            Item = item;
            ItemIndex = itemIndex;
        }

        public T Item { get; }

        public int ItemIndex { get; }
    }
}
