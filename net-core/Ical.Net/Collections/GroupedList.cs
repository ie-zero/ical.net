using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ical.Net.Collections
{
    /// <summary>
    /// A list of objects that are keyed.
    /// </summary>
    public class GroupedCollection<T> : IGroupedCollection<T> where T : class, IGroupedObject
    {
        private readonly List<IList<T>> _lists = new List<IList<T>>();
        private readonly Dictionary<string, IList<T>> _dictionary = new Dictionary<string, IList<T>>();

        private IList<T> EnsureList(string group)
        {
            if (group == null)
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

        private IList<T> ListForIndex(int index, out int relativeIndex)
        {
            foreach (var list in _lists.Where(list => 0 <= index && list.Count > index))
            {
                relativeIndex = index;
                return list;
            }
            relativeIndex = -1;
            return null;
        }

        public event EventHandler<ItemProcessedEventArgs<T>> ItemAdded;

        protected void OnItemAdded(T obj, int index)
        {
            ItemAdded?.Invoke(this, new ItemProcessedEventArgs<T>(obj, index));
        }

        public void Add(T item)
        {
            if (item == null) { return; }

            // Add a new list if necessary
            var list = EnsureList(item.Group);
            list.Add(item);
            OnItemAdded(item, list.Count);
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

        public void Clear()
        {
            _dictionary.Clear();
            _lists.Clear();
        }

        public bool ContainsKey(string group) => _dictionary.ContainsKey(group);

        public int Count => _lists.Sum(list => list.Count);

        public IEnumerable<T> Values() => _dictionary.Values.SelectMany(i => i);

        public IEnumerable<T> AllOf(string group) => _dictionary.ContainsKey(@group)
            ? (IEnumerable<T>) _dictionary[@group]
            : new T[0];

        public bool Remove(T obj)
        {
            var group = obj.Group;
            if (!_dictionary.ContainsKey(group))
            {
                return false;
            }

            var items = _dictionary[group];
            var index = items.IndexOf(obj);

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
            for (var i = list.Count - 1; i >= 0; i--)
            {
                list.RemoveAt(i);
            }
            return true;
        }

        public bool Contains(T item)
        {
            var group = item.Group;
            return _dictionary.ContainsKey(group) && _dictionary[group].Contains(item);
        }

        public void Insert(int index, T item)
        {
            int relativeIndex;
            var list = ListForIndex(index, out relativeIndex);
            if (list == null)
            {
                return;
            }

            list.Insert(relativeIndex, item);
            OnItemAdded(item, index);
        }

        public void RemoveAt(int index)
        {
            int relativeIndex;
            var list = ListForIndex(index, out relativeIndex);
            if (list == null)
            {
                return;
            }
            var item = list[relativeIndex];
            list.RemoveAt(relativeIndex);
        }

        public T this[int index]
        {
            get
            {
                int relativeIndex;
                var list = ListForIndex(index, out relativeIndex);
                return list?[relativeIndex];
            }
            private set
            {
                int relativeIndex;
                var list = ListForIndex(index, out relativeIndex);
                if (list == null)
                {
                    return;
                }

                // Remove the item at that index and replace it
                var item = list[relativeIndex];
                list.RemoveAt(relativeIndex);
                list.Insert(relativeIndex, value);
                OnItemAdded(item, index);
            }
        }

        public IEnumerator<T> GetEnumerator() => new GroupedListEnumerator<T>(_lists);

        IEnumerator IEnumerable.GetEnumerator() => new GroupedListEnumerator<T>(_lists);
    }    
}
