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
        IGroupedCollection<TNew>
        where TOriginal : class, IGroupedObject
        where TNew : class, TOriginal
    {
        private readonly Func<TNew, bool> _predicate;

        public GroupedCollectionProxy(IGroupedCollection<TOriginal> realObject, Func<TNew, bool> predicate = null)
        {
            _predicate = predicate ?? (o => true);
            SetProxiedObject(realObject);
        }

        public event EventHandler<ItemProcessedEventArgs<TNew>> ItemAdded;
        public event EventHandler<ItemProcessedEventArgs<TNew>> ItemRemoved;

        protected void OnItemAdded(TNew item, int index)
        {
            ItemAdded?.Invoke(this, new ItemProcessedEventArgs<TNew>(item, index));
        }

        protected void OnItemRemoved(TNew item, int index)
        {
            ItemRemoved?.Invoke(this, new ItemProcessedEventArgs<TNew>(item, index));
        }

        public bool Remove(string group) => RealObject.Remove(group);

        public void Clear(string group)
        {
            RealObject.Clear(group);
        }

        public bool ContainsKey(string group) => RealObject.ContainsKey(group);

        public int CountOf(string group) => RealObject.OfType<string>().Count();

        public IEnumerable<TNew> AllOf(string group) => RealObject
            .AllOf(group)
            .OfType<TNew>()
            .Where(_predicate);

        public void Add(TNew item)
        {
            RealObject.Add(item);
        }

        public void Clear()
        {
            // Only clear items of this type
            // that match the predicate.

            var items = RealObject
                .OfType<TNew>()
                .ToArray();

            foreach (var item in items)
            {
                RealObject.Remove(item);
            }
        }

        public bool Contains(TNew item) => RealObject.Contains(item);

        public void CopyTo(TNew[] array, int arrayIndex)
        {
            var i = 0;
            foreach (var item in this)
            {
                array[arrayIndex + (i++)] = item;
            }
        }

        public int Count => RealObject
            .OfType<TNew>()
            .Count();

        public bool IsReadOnly => false;

        public bool Remove(TNew item) => RealObject.Remove(item);

        public IEnumerator<TNew> GetEnumerator() => RealObject
            .OfType<TNew>()
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => RealObject
            .OfType<TNew>()
            .GetEnumerator();

        public IGroupedCollection<TOriginal> RealObject { get; private set; }

        public void SetProxiedObject(IGroupedCollection<TOriginal> realObject)
        {
            RealObject = realObject;
        }
    }
}
