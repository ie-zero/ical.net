using System.Collections;
using System.Collections.Generic;

namespace Ical.Net.Collections
{
    internal class GroupedCollectionEnumerator<T> : IEnumerator<T>
    {
        private readonly IList<IList<T>> _lists;

        private IEnumerator<T> _itemEnumerator;
        private IEnumerator<IList<T>> _listsEnumerator;

        public GroupedCollectionEnumerator(IList<IList<T>> lists)
        {
            _lists = lists;
        }

        public T Current
        {
            get
            {
                return _itemEnumerator == null
                    ? default(T)
                    : _itemEnumerator.Current;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (true)
            {
                if (_itemEnumerator == null)
                {
                    if (MoveNextList())
                    {
                        continue;
                    }
                }
                else
                {
                    if (_itemEnumerator.MoveNext())
                    {
                        return true;
                    }

                    DisposeItemEnumerator();
                    if (MoveNextList())
                    {
                        continue;
                    }
                }
                return false;
            }
        }

        private bool MoveNextList()
        {
            if (_listsEnumerator == null)
            {
                _listsEnumerator = _lists.GetEnumerator();
            }

            if (_listsEnumerator == null)
            {
                return false;
            }

            if (!_listsEnumerator.MoveNext())
            {
                return false;
            }

            DisposeItemEnumerator();
            if (_listsEnumerator.Current == null)
            {
                return false;
            }

            _itemEnumerator = _listsEnumerator.Current.GetEnumerator();
            return true;
        }

        public void Reset()
        {
            if (_listsEnumerator == null) { return; }

            _listsEnumerator.Dispose();
            _listsEnumerator = null;
        }

        public void Dispose()
        {
            Reset();
        }

        private void DisposeItemEnumerator()
        {
            if (_itemEnumerator == null) { return; }

            _itemEnumerator.Dispose();
            _itemEnumerator = null;
        }
    }
}
