using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections.Interfaces;
using Ical.Net.Collections.Proxies;

namespace Ical.Net.Collections
{
    public class GroupedValueList<TItem, TValueType> :
        GroupedCollection<TItem>
        where TItem : class, IGroupedObject, IValueObject<TValueType>, new()        
    {
        public void Set(string group, TValueType value)
        {
            Set(group, new[] { value });
        }

        public void Set(string group, IEnumerable<TValueType> values)
        {
            if (ContainsKey(group))
            {
                AllOf(group)?.FirstOrDefault()?.SetValue(values);
                return;
            }

            // No matching item was found, add a new item to the list
            var obj = Activator.CreateInstance(typeof(TItem)) as TItem;
            obj.Group = group;
            Add(obj);
            obj.SetValue(values);
        }

        public TType Get<TType>(string group)
        {
            var firstItem = AllOf(group).FirstOrDefault();
            if (firstItem?.Values != null)
            {
                return firstItem
                    .Values
                    .OfType<TType>()
                    .FirstOrDefault();
            }
            return default(TType);
        }

        public IList<TType> GetMany<TType>(string group)
        {
            return new GroupedValueListProxy<TItem, TValueType, TType>(this, group);
        }
    }
}
