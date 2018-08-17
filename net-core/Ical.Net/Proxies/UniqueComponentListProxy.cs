﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.CalendarComponents;
using Ical.Net.Collections;

namespace Ical.Net.Proxies
{
    public class UniqueComponentListProxy<T> :
        CalendarObjectListProxy<T>,
        IUniqueComponentList<T>
        where T : class, IUniqueComponent
    {
        private readonly Dictionary<string, T> _lookup;

        public UniqueComponentListProxy(IGroupedCollection<ICalendarObject> children) : base(children)
        {
            _lookup = new Dictionary<string, T>();
        }

        private T Search(string uid)
        {
            if (_lookup.TryGetValue(uid, out var componentType))
            {
                return componentType;
            }

            var item = this.FirstOrDefault(c => string.Equals(c.Uid, uid, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                return default(T);
            }

            _lookup[uid] = item;
            return item;
        }

        public T this[string uid]
        {
            get => Search(uid);
            set
            {
                // Find the item matching the UID
                var item = Search(uid);

                if (item != null)
                {
                    Remove(item);
                }

                if (value != null)
                {
                    Add(value);
                }
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var element in collection)
            {
                Add(element);
            }
        }
    }
}