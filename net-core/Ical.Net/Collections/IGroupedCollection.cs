using System;
using System.Collections;
using System.Collections.Generic;

namespace Ical.Net.Collections
{
    public interface IGroupedCollection<TKey, TItem> : IEnumerable<TItem>, IEnumerable where TItem : IGroupedObject<TKey>
    {
        /// <summary>
        /// Fired after an item is added to the collection.
        /// </summary>
        event EventHandler<ItemProcessedEventArgs<TItem>> ItemAdded;

        int Count { get; }

        void Add(TItem item);

        bool Contains(TItem item);

        /// <summary>
        /// Returns true if the list contains at least one object with a matching group, false otherwise.
        /// </summary>
        bool Contains(TKey group);

        bool Remove(TItem item);

        /// <summary>
        /// Removes all items with the matching group from the collection.
        /// </summary>        
        /// <returns>True if the object was removed, false otherwise.</returns>
        bool Remove(TKey group);

        /// <summary>
        /// Returns a list of objects that
        /// match the specified group.
        /// </summary>
        IEnumerable<TItem> Values(TKey group);
    }
}
