using System;
using System.Collections;
using System.Collections.Generic;

namespace Ical.Net.Collections
{
    public interface IGroupedCollection<T> : IEnumerable<T>, IEnumerable where T : IGroupedObject
    {
        /// <summary>
        /// Fired after an item is added to the collection.
        /// </summary>
        event EventHandler<ItemProcessedEventArgs<T>> ItemAdded;

        /// <summary>
        /// Removes all items with the matching group from the collection.
        /// </summary>        
        /// <returns>True if the object was removed, false otherwise.</returns>
        bool Remove(string group);

        /// <summary>
        /// Returns true if the list contains at least one 
        /// object with a matching group, false otherwise.
        /// </summary>
        bool ContainsKey(string group);
        
        /// <summary>
        /// Returns a list of objects that
        /// match the specified group.
        /// </summary>
        IEnumerable<T> Values(string group);

        int Count { get; }       
        bool Contains(T item);    
        void Add(T item);
        bool Remove(T item);
    }
}
