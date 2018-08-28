using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ical.Net.Collections.Proxies
{
    /// <summary>
    /// A proxy for a keyed list.
    /// </summary>
    public class GroupedCollectionProxy<TOriginal, TNew> :
        IGroupedCollection<string, TNew>
        where TOriginal : class, IGroupedObject<string>
        where TNew : class, TOriginal
    {
        public GroupedCollectionProxy(IGroupedCollection<string, TOriginal> realObject)
        {
            RealObject = realObject;
        }

        public IGroupedCollection<string, TOriginal> RealObject { get; }

        public event EventHandler<ItemProcessedEventArgs<TNew>> ItemAdded;

        protected void OnItemAdded(TNew item, int index)
        {
            ItemAdded?.Invoke(this, new ItemProcessedEventArgs<TNew>(item, index));
        }

        public bool Contains(string group)
        {
            return RealObject.Contains(group);
        }

        public int CountOf(string group)
        {
            // TODO: The implementation of CountOf() is incorrect. 
            return RealObject.OfType<string>().Count();
        }

        public IEnumerable<TNew> Values(string group)
        {
            return RealObject.Values(group).OfType<TNew>();
        }

        public void Add(TNew item)
        {
            RealObject.Add(item);
        }

        public bool Remove(string group)
        {
            return RealObject.Remove(group);
        }

        public void Clear()
        {
            var items = RealObject
                .OfType<TNew>()
                .ToArray();

            foreach (var item in items)
            {
                RealObject.Remove(item);
            }
        }

        public bool Contains(TNew item)
        {
            return RealObject.Contains(item);
        }

        public int Count
        {
            get { return RealObject.OfType<TNew>().Count(); }
        }

        public bool Remove(TNew item)
        {
            return RealObject.Remove(item);
        }

        public IEnumerator<TNew> GetEnumerator()
        {
            return RealObject.OfType<TNew>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
