using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections.Interfaces;
using Ical.Net.Collections.Proxies;

namespace Ical.Net.Collections
{
    public class GroupedValueList<TItem, TValue> : GroupedCollection<string, TItem>
        where TItem : class, IGroupedObject<string>, IValueObject<TValue>, new()        
    {
        public void Set(string group, TValue value)
        {
            Set(group, new[] { value });
        }

        public void Set(string group, IEnumerable<TValue> values)
        {
            if (Contains(group))
            {
                Values(group)?.FirstOrDefault()?.SetValue(values);
                return;
            }

            // No matching item was found, add a new item to the list
            var obj = Activator.CreateInstance(typeof(TItem)) as TItem;
            obj.Group = group;
            Add(obj);
            obj.SetValue(values);
        }

        public T Get<T>(string group)
        {
            var firstItem = Values(group).FirstOrDefault();
            if (firstItem?.Values != null)
            {
                return firstItem
                    .Values
                    .OfType<T>()
                    .FirstOrDefault();
            }
            return default(T);
        }

        public IList<T> GetMany<T>(string group)
        {
            return new GroupedValueListProxy<TItem, TValue, T>(this, group);
        }
    }
}
