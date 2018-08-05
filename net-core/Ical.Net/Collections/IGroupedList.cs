using System.Collections.Generic;

namespace Ical.Net.Collections
{
    public interface IGroupedList<TItem> :
        IGroupedCollection<string, TItem>,
        IList<TItem>
        where TItem : class, IGroupedObject
    {
        /// <summary>
        /// Returns the index of the given item within the list, or -1 if the item is not found in
        /// the list.
        /// </summary>
        new int IndexOf(TItem obj);

        /// <summary>
        /// Gets the object at the specified index.
        /// </summary>
        new TItem this[int index] { get; }
    }
}
