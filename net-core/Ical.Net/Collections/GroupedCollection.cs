using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ical.Net.Collections
{
    /// <summary>
    /// A list of objects that are keyed.
    /// </summary>
    public class GroupedCollection<TKey, TItem> : IGroupedCollection<TKey, TItem> where TItem : IGroupedObject<TKey>
    {
        private readonly List<IList<TItem>> _lists;
        private readonly Dictionary<TKey, IList<TItem>> _dictionary;

        public GroupedCollection()
        {
            _lists = new List<IList<TItem>>();
            _dictionary = new Dictionary<TKey, IList<TItem>>();
        }

        /// <summary>
        /// Returns the total number of elements stored for all the groups.
        /// </summary>
        public int Count
        {
            get { return _lists.Sum(list => list.Count); }
        }

        public TItem this[int index]
        {
            get { return GetItem(index); }
        }

        private TItem GetItem(int index)
        {
            if (index < 0 || index >= Count) { return default(TItem); }

            var allItems = Values().ToList();
            return allItems[index];
        }

        public IEnumerable<TItem> this[TKey group]
        {
            get { return Values(group); }
        }

        public event EventHandler<ItemProcessedEventArgs<TItem>> ItemAdded;

        protected void OnItemAdded(TItem obj, int index)
        {
            ItemAdded?.Invoke(this, new ItemProcessedEventArgs<TItem>(obj, index));
        }

        public void Add(TItem item)
        {
            if (item == null) { return; }

            var list = GetOrCreateList(item.Group);
            if (list == null) { return; }

            list.Add(item);
            OnItemAdded(item, list.Count);
        }

        private IList<TItem> GetOrCreateList(TKey group)
        {
            if (group == null)
            {
                return null;
            }

            if (_dictionary.ContainsKey(group))
            {
                return _dictionary[group];
            }

            var list = new List<TItem>();
            _dictionary[group] = list;

            _lists.Add(list);
            return list;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _lists.Clear();
        }

        public void Clear(TKey group)
        {
            if (!_dictionary.ContainsKey(group))
            {
                return;
            }

            // Clear the list (note that this also clears the list in the _Lists object).
            _dictionary[group].Clear();
        }

        public bool Contains(TItem item)
        {
            if (item == null) { return false; }

            var group = item.Group;
            return _dictionary.ContainsKey(group) && _dictionary[group].Contains(item);
        }

        public bool Contains(TKey group)
        {
            if (group == null) { return false; }
            return _dictionary.ContainsKey(group);
        }

        public IEnumerable<TItem> Values()
        {
            return _dictionary.Values.SelectMany(item => item);
        }

        public IEnumerable<TItem> Values(TKey group)
        {
            if (group == null) { return Enumerable.Empty<TItem>(); }

            return _dictionary.ContainsKey(group) ? _dictionary[group].ToArray() : Enumerable.Empty<TItem>();
        }

        public bool Remove(TItem item)
        {
            var group = item.Group;
            if (!_dictionary.ContainsKey(group))
            {
                return false;
            }

            var items = _dictionary[group];
            var index = items.IndexOf(item);

            if (index < 0)
            {
                return false;
            }

            items.RemoveAt(index);
            return true;
        }

        // TODO: Consider what is the difference between Remove(TKey) and Clear(TKey).
        public bool Remove(TKey group)
        {
            if (!_dictionary.ContainsKey(group))
            {
                return false;
            }

            var list = _dictionary[group];
            for (var index = list.Count - 1; index >= 0; index--)
            {
                list.RemoveAt(index);
            }
            return true;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return new GroupedCollectionEnumerator<TItem>(_lists);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }    
}
