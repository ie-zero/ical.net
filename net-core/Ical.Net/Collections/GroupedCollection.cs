using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ical.Net.Collections
{
    /// <summary>
    /// A list of objects that are keyed.
    /// </summary>
    public class GroupedCollection<T> : IGroupedCollection<T> where T : IGroupedObject
    {
        private readonly List<IList<T>> _lists;
        private readonly Dictionary<string, IList<T>> _dictionary;

        public GroupedCollection()
        {
            _lists = new List<IList<T>>();
            _dictionary = new Dictionary<string, IList<T>>();
        }

        public T this[int index]
        {
            get { return GetItem(index); }
        }

        private T GetItem(int index)
        {
            if (index < 0 && index >= Count) { return default(T); }

            var allItems = Values().ToList();
            return allItems[index];
        }

        public IEnumerable<T> this[string group]
        {
            get { return Values(group); }
        }

        public event EventHandler<ItemProcessedEventArgs<T>> ItemAdded;

        protected void OnItemAdded(T obj, int index)
        {
            ItemAdded?.Invoke(this, new ItemProcessedEventArgs<T>(obj, index));
        }

        public void Add(T item)
        {
            if (item == null) { return; }

            var list = GetOrCreateList(item.Group);
            if (list == null) { return; }

            list.Add(item);
            OnItemAdded(item, list.Count);
        }

        private IList<T> GetOrCreateList(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                return null;
            }

            if (_dictionary.ContainsKey(group))
            {
                return _dictionary[group];
            }

            var list = new List<T>();
            _dictionary[group] = list;

            _lists.Add(list);
            return list;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _lists.Clear();
        }

        public void Clear(string group)
        {
            if (!_dictionary.ContainsKey(group))
            {
                return;
            }

            // Clear the list (note that this also clears the list in the _Lists object).
            _dictionary[group].Clear();
        }

        public bool Contains(T item)
        {
            var group = item.Group;
            return _dictionary.ContainsKey(group) && _dictionary[group].Contains(item);
        }

        public bool Contains(string group)
        {
            return _dictionary.ContainsKey(group);
        }

        /// <summary>
        /// Returns the total number of elements stored for all the groups.
        /// </summary>
        public int Count
        {
            get { return _lists.Sum(list => list.Count); }
        }

        public IEnumerable<T> Values()
        {
            return _dictionary.Values.SelectMany(item => item);
        }

        public IEnumerable<T> Values(string group)
        {
            return _dictionary.ContainsKey(group) ? _dictionary[group].ToArray() : new T[0];
        }

        public bool Remove(T item)
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

        public bool Remove(string group)
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

        public IEnumerator<T> GetEnumerator()
        {
            return new GroupedCollectionEnumerator<T>(_lists);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }    
}
